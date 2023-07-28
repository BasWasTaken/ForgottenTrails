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

namespace ForgottenTrails.InkFacilitation
{
    /// <summary>
    /// <para>Summary not provided.</para>
    /// </summary>
    public class InterfaceBroker : MonoSingleton<InterfaceBroker>
    {
        // Inspector Properties
        #region Inspector Properties
        [field: SerializeField, BoxGroup("Prefabs"), Required]
        [Tooltip("Prefab used for ink choices.")]
        public Button ButtonPrefab { get; private set; }

        [field: SerializeField, BoxGroup("Scene References"), Required]
        private Transform ButtonAnchor { get; set; }

        [field:SerializeField, BoxGroup("Scene References"), Required]
        public Image FloatingMarker { get; private set; }

        [field:SerializeField, BoxGroup("Settings")]
        [Tooltip("Delay after which space button advances dialogue.")]
        public float AdvanceDialogueDelay { get; private set; } = .1f;

        [field: SerializeField, BoxGroup("Settings")]
        [Tooltip("Delay after which space button skips new dialogue.")]
        public float SkipDelay { get; private set; } = .2f;
        #endregion
        // Public Properties
        #region Public Properties



        #endregion
        // Private Properties
        #region Private Properties
        StoryController StoryController { get; set; }
        #endregion
        // MonoBehaviour LifeCycle Methods
        #region MonoBehaviour LifeCycle Methods
        protected override void Awake()
        {
            base.Awake();
            StoryController = GetComponent<StoryController>();
        }

        

        #endregion
        // Public Methods
        #region Public Methods

        #endregion
        // Private Methods
        #region Private Methods

        #endregion

        // UNRESOLVED
        




        public void RemoveOptions()/// Destroys all the buttons from choices
        {
            foreach (Button child in ButtonAnchor.GetComponentsInChildren<Button>())
            {
                Destroy(child.gameObject);
            }
        }
        public bool CanPresentChoices()
        {
            if (StoryController.Story.canContinue)
            {
                //Debug.Log("no choices detected at this point");
                return false;
            }
            else if (StoryController.Story.currentChoices.Count > 0)
            {
                return true;
            }
            else
            {
                return true; // technically, if ending the dialogue is a choise
            }
        }
        public void PresentButtons()
        {
            if (StoryController.Story.canContinue)
            {
                //Debug.Log("no choices detected at this point");
                return;
            }
            else if (StoryController.Story.currentChoices.Count > 0) /// Display all the choices, if there are any!
            {
                //Debug.Log("Choices detected!");
                for (int i = 0; i < StoryController.Story.currentChoices.Count; i++)
                {

                    Choice choice = StoryController.Story.currentChoices[i];
                    Button button = PresentButton(choice.text.Trim());
                    /// Tell the button what to do when we press it
                    button.onClick.AddListener(delegate {
                        OnClickChoiceButton(choice);
                    });
                }
                //scrollbar.value = 0;
                return;
            }
            /// If we've read all the content and there's no choices, the story is finished!
            else
            {
                Button choice = PresentButton("End of story.");
                choice.onClick.AddListener(delegate {
                    RemoveOptions();
                    StoryController.OnInteractionEnd();
                });
                return;
            }
        }
        /// Creates a button showing the choice text
		private Button PresentButton(string text)
        {
            Debug.Log("make button for " + text);
            /// Creates the button from a prefab
            Button choice = Instantiate(ButtonPrefab) as Button;
            choice.transform.SetParent(ButtonAnchor, false);

            /// Gets the text from the button prefab
            TextMeshProUGUI choiceText = choice.GetComponentInChildren<TextMeshProUGUI>();
            choiceText.text = text;


            /// Make the button expand to fit the text
                        /* we don't want that, i want the reverse
            HorizontalLayoutGroup layoutGroup = choice.GetComponent<HorizontalLayoutGroup>();
            layoutGroup.childForceExpandHeight = false;
                        */

            return choice;
        }
        /// When we click the choice button, tell the story to choose that choice!
        void OnClickChoiceButton(Choice choice)
        {
            story.ChooseChoiceIndex(choice.index); /// feed the choice
            InkDataAsset.storyStateJson = story.state.ToJson(); /// record the story state
            StoryController.AdvanceStory(); /// next bit
		}
    }
}
