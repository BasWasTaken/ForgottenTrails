using DataService;
using Ink.Runtime;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Bas.Utility;
using items;

namespace ForgottenTrails.InkFacilitation
{
    public partial class StoryController : MonoSingleton<StoryController>
    {
        [Serializable]
        /// <summary>
        /// <para>Summary not provided.</para>
        /// </summary>
        public partial class InterfaceBroking
        {
            // Inspector Properties
            #region Inspector Properties
            [field: SerializeField, Header("Prefabs"), BoxGroup("Prefabs"), Required]
            [Tooltip("Prefab used for ink choices.")]
            public Button ButtonPrefab { get; internal set; }

            [field: SerializeField, Header("Scene References"), BoxGroup("Scene References"), Required]
            internal Transform ButtonAnchor { get; set; }

            [field: SerializeField, BoxGroup("Scene References"), Required]
            public Image FloatingMarker { get; internal set; }

            [field: SerializeField, Header("Settings"), BoxGroup("Settings")]
            [Tooltip("Delay after which space button advances dialogue.")]
            public float AdvanceDialogueDelay { get; internal set; } = .1f;

            [field: SerializeField, BoxGroup("Settings")]
            [Tooltip("Delay after which space button skips new dialogue after advancing.")]
            public float SkipDelay { get; internal set; } = .2f;
            #endregion
            // Public Properties
            #region Public Properties
            readonly Dictionary<string, Choice> hiddenChoices = new();
            #endregion
            // Private Properties
            #region Private Properties
            private StoryController Controller;
            #endregion
            // Constructor
            #region Constructor
            internal void Assign()
            {
                Controller = Instance;
            }
            #endregion
            // Public Methods


            [SerializeField, Header("Scene References"), Required]
            public items.Inventory inventory;

            #region Public Methods
            public bool CanPresentChoices()
            {
                if (Controller.Story.canContinue)
                {
                    //Debug.Log("no choices detected at this point");
                    return false;
                }
                else if (Controller.Story.currentChoices.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false; // technically, if ending the dialogue is a choise
                }
            }

            /// When we click the choice button, tell the story to choose that choice!
            internal void OnClickChoiceButton(Choice choice)
            {
                Controller.Story.ChooseChoiceIndex(choice.index); /// feed the choice
                Controller.InkDataAsset.StoryStateJson = Controller.Story.state.ToJson(); /// record the story state NOTE why safe here, won't that cause delay?
                RemoveOptions();
                Controller.waitingForChoiceState.DropCondition = true;
                Controller.StateMachine.TransitionToState(Controller.savingState);

            }
            internal void RemoveOptions()// Destroys all the buttons from choices
            {
                Controller.InterfaceBroker.hiddenChoices.Clear();
                foreach (Button child in Controller.InterfaceBroker.ButtonAnchor.GetComponentsInChildren<Button>())
                {
                    Destroy(child.gameObject);
                }
            }
            public bool TryUseItem(InventoryItem item)
            {
                Choice discoveredChoice = null;
                foreach (KeyValuePair<string, Choice> keyValuePair in hiddenChoices)
                {

                    string keyPhrase = keyValuePair.Key;
                    Choice potentialChoice = keyValuePair.Value;
                    if(item.CanonicalName == keyPhrase)
                    {
                        discoveredChoice = potentialChoice;
                        break;
                    }
                    else
                    {
                        foreach (Affordance trait in item.Affordaces)
                        {
                            if (trait.ToString() == keyPhrase)
                            {
                                discoveredChoice = potentialChoice;
                                break;
                            }
                        }
                    }
                    
                }

                if (discoveredChoice!=null)
                {
                    Controller.inventoryState.DropCondition = true;
                    var newList = new Ink.Runtime.InkList("Items", StoryController.Instance.Story);
                    newList.AddItem(item.InkListItem);
                    Controller.Story.variablesState["UsedItem"] = newList;
                    OnClickChoiceButton(discoveredChoice);
                    return true;
                }
                else
                {
                    Debug.Log("Nope, that item doesn't work!");
                    return false;
                }
            }
            #endregion
            // Private Methods
            #region Private Methods
            
            #endregion
        }
    }    
}
