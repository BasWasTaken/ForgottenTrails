using Ink.Runtime;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using VVGames.Common;
using VVGames.ForgottenTrails.InkConnections.Items;
using VVGames.ForgottenTrails.InkConnections.Party;
using VVGames.ForgottenTrails.InkConnections.Travel;
using VVGames.ForgottenTrails.UI;
using static VVGames.ForgottenTrails.InkConnections.StoryController.InterfaceBroking.SCWaitingForChoiceState;

namespace VVGames.ForgottenTrails.InkConnections
{
    public partial class StoryController : MonoSingleton<StoryController>
    {
        #region Classes

        [Serializable]
        public partial class InterfaceBroking
        {
            // Inspector Properties

            #region Fields

            [SerializeField, Header("Scene References"), Required]
            public Inventory inventory;

            public PartyScreen partyScreen;

            private StoryController Controller;

            #endregion Fields

            #region Properties

            [field: SerializeField, Header("Prefabs"), BoxGroup("Prefabs"), Required]
            [Tooltip("Prefab used for ink choices.")]
            public Button ButtonPrefab { get; internal set; }

            [field: SerializeField, Header("Scene References"), BoxGroup("Scene References"), Required]
            public InGameMenuSwapper InGameMenu { get; set; }

            [field: SerializeField, BoxGroup("Scene References"), Required]
            public Image FloatingMarker { get; internal set; }

            [field: SerializeField, Header("Settings"), BoxGroup("Settings")]
            [Tooltip("Delay after which space button advances dialogue.")]
            public float AdvanceDialogueDelay { get; internal set; } = .1f;

            [field: SerializeField, BoxGroup("Settings")]
            [Tooltip("Delay after which space button skips new dialogue after advancing.")]
            public float SkipDelay { get; internal set; } = .2f;

            [field: SerializeField, Header("Scene References"), BoxGroup("Scene References"), Required]
            internal Transform ButtonAnchor { get; set; }

            [field: SerializeField, Header("Scene References"), BoxGroup("Scene References"), Required]
            internal GameObject mapButtonsContainer { get; set; }

            // Public Properties

            internal Dictionary<string, HiddenChoice> hiddenChoices { get; set; } = new();

            #endregion Properties

            // Private Properties
            // Constructor

            #region Public Methods

            // Public Methods
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

            public bool TryUseItem(InventoryItem item)
            {
                Choice discoveredChoice = null;
                foreach (KeyValuePair<string, HiddenChoice> keyValuePair in hiddenChoices)
                {
                    if (keyValuePair.Value.Type == ChoiceType.Item)
                    {
                        string keyPhrase = keyValuePair.Key;
                        Choice potentialChoice = keyValuePair.Value.Choice;
                        if (item.CanonicalName == keyPhrase)
                        {
                            discoveredChoice = potentialChoice;
                            Debug.LogFormat("Found exact match: {0} == {1}", item, keyPhrase);
                            break;
                        }
                        else
                        { // get one or more affordances
                            // Split the input string using the '&' separator
                            string[] affordances = keyPhrase.Split('&');

                            // Output each affordance
                            List<bool> HasAffordances = new();
                            string values = "";
                            foreach (string affordance in affordances) // for each affordance we need,
                            {
                                // see if the item has it.
                                if (item.ContainsAffordance(affordance))
                                {
                                    HasAffordances.Add(true);
                                    values += affordance + '&';
                                }
                                else
                                {
                                    HasAffordances.Add(false);
                                    string addendum = "It's not " + affordance;
                                    break;
                                }
                            }
                            if (!HasAffordances.Contains(false))
                            {
                                discoveredChoice = potentialChoice;
                                Debug.LogFormat("found item that is {0}", values);
                                break;
                            }
                        }
                    }
                    // else not item, so need not be considered.
                }

                if (discoveredChoice != null)
                {
                    Controller.inventoryState.DropCondition = true;
                    var newList = new Ink.Runtime.InkList("Items", StoryController.Instance.Story);
                    newList.AddItem(item.InkListItem);
                    Controller.Story.variablesState["UsedItem"] = newList;
                    OnClickChoiceButton(discoveredChoice);
                    Debug.LogFormat("used item {0} for choice {1}", item.name, discoveredChoice.text);
                    return true;
                }
                else
                {
                    Debug.Log("Nope, that item doesn't work!");
                    return false;
                }
            }

            public bool TryTravelTo(MapLocationContainer location)
            {
                Choice discoveredChoice = null;
                foreach (KeyValuePair<string, HiddenChoice> keyValuePair in hiddenChoices)
                {
                    if (keyValuePair.Value.Type == ChoiceType.Map)
                    {
                        string keyPhrase = keyValuePair.Key;
                        Choice potentialChoice = keyValuePair.Value.Choice;
                        if (location.canonicalLocation == keyPhrase)
                        {
                            discoveredChoice = potentialChoice;
                            break;
                        }
                    }
                    // else not travel, so need not be considered.
                }

                if (discoveredChoice != null)
                {
                    Controller.mapState.DropCondition = true;
                    OnClickChoiceButton(discoveredChoice);
                    return true;
                }
                else
                {
                    Debug.Log("Nope, that location doesn't work!");
                    return false;
                }
            }

            public bool TryConverseMember(PartyMemberSO member)
            {
                Choice discoveredChoice = null;
                foreach (KeyValuePair<string, HiddenChoice> keyValuePair in hiddenChoices)
                {
                    if (keyValuePair.Value.Type == ChoiceType.Party)
                    {
                        string keyPhrase = keyValuePair.Key;
                        Choice potentialChoice = keyValuePair.Value.Choice;
                        if (member.CanonicalName == keyPhrase)
                        {
                            discoveredChoice = potentialChoice;
                            break;
                        }
                    }
                }

                if (discoveredChoice != null)
                {
                    Controller.partyState.DropCondition = true;
                    OnClickChoiceButton(discoveredChoice);
                    return true;
                }
                else
                {
                    Debug.Log("Nope, that party member doesn't work!");
                    return false;
                }
            }

            #endregion Public Methods

            #region Internal Methods

            internal void Assign()
            {
                Controller = Instance;
            }

            internal void FindHiddenChoices()
            {
                hiddenChoices.Clear();
                string message = "";
                foreach (Choice choice in Controller.Story.currentChoices)
                {
                    bool succes = TryAddHiddenChoice(choice);

                    message += string.Format("\n \"{0}\" was{1} a hidden choice", choice.text, succes ? "" : " not");
                }

                Debug.LogFormat("Found {0} hidden choices:\n{1}", hiddenChoices.Count, message);
            }

            internal bool TryAddHiddenChoice(Choice choice)
            {
                string input = choice.text;
                if (Regex.IsMatch(input, "^{.+Choice\\(")) // automatically gets itemchoices, mapchoices, etc
                {
                    string kind = input.Substring(1, input.IndexOf('C') - 1);
                    //Debug.Log(kind);
                    Enum.TryParse(kind, true, out ChoiceType choiceType);

                    string opener = input.Substring(0, input.IndexOf('(') + 1);
                    string closer = input.Substring(input.IndexOf(')'));
                    //Debug.Log(opener);
                    //Debug.Log(closer);
                    int startIndex = input.IndexOf(opener);
                    int endIndex = input.IndexOf(closer, startIndex);

                    if (startIndex != -1 && endIndex != -1 && endIndex > startIndex)
                    {
                        int substringLength = endIndex - startIndex - opener.Length;// (closer.Length-1);
                        string key = input.Substring(startIndex + opener.Length, substringLength); // Dev note: potential room for duplicate key-related errors here?
                        Debug.Log("Encountered hidden choice: " + key);

                        // I now have the kind as wel as the value of the choice.
                        HiddenChoice newHidden = new(choiceType, choice);
                        Controller.InterfaceBroker.hiddenChoices.Add(key, newHidden);
                        return true;
                    }
                    else
                    {
                        Debug.LogError("could not identify hidden choice");
                        return false;
                    }
                }
                else return false;
            }

            /// When we click the choice button, tell the story to choose that choice!
            internal void OnClickChoiceButton(Choice choice)
            {
                //Debug.Log("chose " + choice.text);
                Controller.Story.ChooseChoiceIndex(choice.index); // feed the choice
                Controller.InkDataAsset.StoryStateJson = Controller.Story.state.ToJson(); // record the story state
                RemoveOptions();
                Controller.waitingForChoiceState.DropCondition = true;
                Controller.StateMachine.TransitionToState(Controller.savingState); // save the game
            }

            internal void RemoveOptions()// Destroys all the buttons from choices
            {
                Controller.InterfaceBroker.hiddenChoices.Clear();
                foreach (Button child in Controller.InterfaceBroker.ButtonAnchor.GetComponentsInChildren<Button>())
                {
                    Destroy(child.gameObject);
                }
            }

            internal void OpenMap()
            {
                Controller.StateMachine.TransitionToState(Controller.mapState);
            }

            internal void CloseMap()
            {
                Controller.mapState.DropCondition = true;
            }

            internal void OpenPartyScreen()
            {
                Controller.StateMachine.TransitionToState(Controller.partyState);
            }

            internal void ClosePartyScreen()
            {
                Controller.partyState.DropCondition = true;
            }

            #endregion Internal Methods
        }

        #endregion Classes
    }
}