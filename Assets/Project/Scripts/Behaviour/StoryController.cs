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
    /// <para>Behaviour for parsing content from <see cref="Story"/> files and passing it onto the appropriate monobehaviours.</para>
    /// </summary>
    [RequireComponent(typeof(TextProducer))]
    [RequireComponent(typeof(SetDresser))]
    [RequireComponent(typeof(InterfaceBroker))]
    public class StoryController : MonoSingleton<StoryController>
    {
        // Constants
        #region Constants
        private const string dataLabel = "BasicInkScript"; // NOTE:  isn't this ridiculous? if you'll be using this object as ink interface all the time, it should't itelf store particular data, you should have objets etc store data... else all will be under here, won't it? or will it just be settings?

        #endregion
        // Inspector Properties & Helpers
        #region Inspector Properties & Helpers
        [field:SerializeField, BoxGroup("Assets"), Required]
        [Tooltip("Here drag the JSON object containing the dialogue behaviour")]
        public TextAsset InkStoryAsset { get; private set; }

        [SerializeField, BoxGroup("Data"), ReadOnly] 
        [Tooltip("View data object containing INK data.")]
        private InkDataClass _inkDataAsset;
        public InkDataClass InkDataAsset { get { if(_inkDataAsset==null) _inkDataAsset=CreateBlankData(); return _inkDataAsset; } set { _inkDataAsset = value; } }
        

        [Tooltip("Reset ink data in object. Note: does not remove data from file")]
        [Button("ResetInkData", EButtonEnableMode.Editor)]public void ResetInkDataButton() => ResetInkDataAndStory();

        [Tooltip("Load data from disk and reset scene.")]
        [Button("ProcessData")] public void ProcessDataButton() => ApplyDiskData();

        #endregion
        // Public Properties
        #region Public Properties
        #region State Machine
        public StackFiniteStateMachine<StoryController_State> StateMachine { get; private set; }
        // unused atm public bool Playing => StateMachine.CurrentState.GetType() == typeof(StoryController_State.Active);

        public TextProducer TextProducer { get; private set; }
        public SetDresser SetDresser { get; private set; }
        public InterfaceBroker InputBroker { get; private set; }


        public float TimeSinceAdvance { get; set; } = 0;

        #endregion


        public Story Story { get; set; }
        public Dictionary<VariablesState, object> InkVars { get; set; } = new();

        public InkFunctions Functions { get; set; } // TODO: Incorporate this into class or machine?





        #endregion
        // Private Properties
        #region Private Properties

        #region States
        ControllerState.Entry entryState;
        #endregion

        #endregion
        // Events

        // LifeCycle Methods
        #region LifeCycle Methods
        override protected void Awake()
        {
            base.Awake();
            Functions = new(this); // remove?
            TextProducer = GetComponent<TextProducer>();
            InputBroker = GetComponent<InterfaceBroker>();
            SetDresser = GetComponent<SetDresser>();
            transform.localPosition = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y); // NOTE: Why do I do this?

            entryState = new(this);
            StateMachine = new(this,entryState);
            StateMachine.TransitionToState(entryState);
        }
        private void Update()
        {
            StateMachine.Update();
        }

        #endregion
        // Public Methods
        #region Public Methods

        #endregion
        // Private Methods
        #region Private Methods

        // Data Management
        #region Data Management
        /// <summary>
        /// Make a new <see cref="InkDataClass"/>object
        /// </summary>
        /// <returns>The freshly made blank data</returns>
        private InkDataClass CreateBlankData(bool forBootup = false)
        {// NOTE: Is this the optimal way of doing this?
            InkDataClass data = new(dataLabel + "DemoScene");
            if (!forBootup) Debug.Log("Created new data " + data.Label);
            return data;
        }

        private void ResetInkDataAndStory()
        {
            InkDataAsset = CreateBlankData();
            PrepStory();
        }

        private void SaveData()
        {
            Instance.PutDataIntoStash();
            StateMachine.TransitionToState(StoryController_State.savingState);
        }

        /// <summary>
        /// preps data for saving. happens at the end of every bit of dialogue.
        /// </summary>
        private void PutDataIntoStash() // this should then be called every so often and whenever the game is saved
        {
            // save all the things 
            InkDataAsset.CurrentText = TextProducer.CurrentText; // NOTE: is deze nodig? is dat niet contained in story?
            InkDataAsset.HistoryText = TextProducer.PreviousText;
            InkDataAsset.StoryStateJson = Story.state.ToJson();
            InkDataAsset.StashData();
        }


        private bool TryLoadData()
        {// NOTE: If this needs to be public, move it accordingly.
            return TryLoadData(ref _inkDataAsset);
        }

        private bool TryLoadData(ref InkDataClass output)
        {
            // TODO: Try to do this without the need for the out variable like this so you can restructure the property
            // NOTE: Does this need the ref variable (and thus abvoe method)? Can't it just affect a property? Using a buffer field if needed.
            if (!DataManager.Instance.DataAvailable(output.Key))
            {
                Debug.LogError("Error code 404: No data found available.");
                return false;
            }
            else
            {
                InkDataClass input = DataManager.Instance.FetchData<InkDataClass>(InkDataAsset.Key);
                if (output == input)
                {
                    Debug.LogError("Error code 11: input data is same as output data; nothing to load.");

                    // will this ever be the case if i don't use two refs?

                    /*Debug.Log("ah, no, that's just this data. gonna remove it from cache for testing");

                    DataManager.Instance.RemoveFromCache<InkData>(inkData.Key);*/
                    return false;
                }
                else
                {
                    output = CreateBlankData(); // again creating new data, to copy the existing data into i guess
                    // wait, why? so we don't touch the other data?
                    // i guess just because all the functions prior to the textline already set their data to the new datafile, so you want to make sure you read from the ifrst datafile so you read the original and not anything yo ujust added.
                    // okay, that just means we should manually add the textdata to the new outputdata
                    // TODO: Resolve the above comment.

                    try
                    {
                        PopulateStoryVarsFromData(input, ref output);

                        PopulateSceneFromData(input, ref output);
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                    Debug.Log("Successfully loaded data!");
                    return true;
                }
            }

        }

        private void PopulateStoryVarsFromData(InkDataClass input, ref InkDataClass output)
        {
            string message = "InkVars:";


            Story.state.LoadJson(input.StoryStateJson); // get storystate from json

            output.StoryStateJson = Story.state.ToJson(); //put storystate into inkdata

            foreach (string item in Story.state.variablesState)
            {
                message += "\n" + item + ": " + Story.state.variablesState[item].ToString();
            }
            Debug.Log(message);
        }

        private void PopulateSceneFromData(InkDataClass input, ref InkDataClass output)
        {
            //Debug.Log("This is when the textpanel is set to the contents of inkdata: " + textPanel.text);
            SetDresser.ParseAudio(input.SceneState.ActiveMusic, AudioHandler.AudioGroup.Music);
            SetDresser.ParseAudio(input.SceneState.ActiveAmbiance, AudioHandler.AudioGroup.Ambiance);
            SetDresser.SetBackdrop(input.SceneState.Background);
            SetDresser.SetSprites(input.SceneState.Sprites);
            SetDresser.SetSprites(input.SceneState.Sprites);
            TextProducer.Spd(input.SceneState.TextSpeedMod);
            TextProducer.StartCoroutine(TextProducer.FeedText(InkDataAsset.CurrentText));
            output.CurrentText = input.CurrentText;
            output.HistoryText = input.HistoryText;
        }

        private void ApplyDiskData()
        {
#if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlaying == true)
            {
                StateMachine.TransitionToState(StoryController_State.exitingState);
            }
#endif
            ResetInkDataAndStory();
#if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlaying == true)
            {
                StartStory();
                SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
            }
#endif
        }

        #endregion
        // Story Setup
        #region Story Setup


        /// <summary>
        /// Creates a new Story object with the compiled story which we can then play!
        /// </summary>
        private void StartStory()
        {
            StateMachine.TransitionToState(StoryController_State.loadingState);
            if (DataManager.Instance.DataAvailable(InkDataAsset.Key))
            {
                Debug.Log("found data! trying to load...");
                TryLoadData();
            }
            else
            {
                InkDataAsset = CreateBlankData(); // NOTE: you're making data a second time, there's already blank data made. why am i doing this again?
            }

            if (InkDataAsset.StoryStateJson != "")
            {
                Debug.Log("continueing from savepoint!");
                Story.state.LoadJson(InkDataAsset.StoryStateJson);
                StartCoroutine(TextProducer.FeedText(Story.currentText));
            }
            else
            {
                Debug.Log("no save point detected, starting from start");
                Story.state.GoToStart();
            }
            AdvanceStory(); // show the first bit of story
        }

        public void BindAndObserve(Story story)
        {
            story.BindExternalFunction("Print", (string text) => Functions.DoFunction(() => ConsoleLogInk(text, false)));
            story.BindExternalFunction("PrintWarning", (string text) => Functions.DoFunction(() => ConsoleLogInk(text, true)));
            story.BindExternalFunction("Spd", (float mod) => Functions.DoFunction(() => Spd(mod / 100)));
            story.BindExternalFunction("Clear", () => Functions.DoFunction(() => TextProducer.ClearPage()));
            story.BindExternalFunction("Halt", (float dur) => Functions.DoFunction(() => StartCoroutine(HaltProcedings(dur))));
            story.BindExternalFunction("Bg", (string fileName, float dur) => Functions.DoFunction(() => SetBackdrop(fileName, dur)));
            story.BindExternalFunction("FadeTo", (string color, float dur) => Functions.DoFunction(() => SetColor(color, dur)));
            story.BindExternalFunction("Sprites", (string fileNames) => Functions.DoFunction(() => SetSprites(fileNames)));
            story.BindExternalFunction("Vox", (string fileName, float relVol) => Functions.DoFunction(() => ParseAudio(fileName, AudioHandler.AudioGroup.Voice, relVol)));
            story.BindExternalFunction("Sfx", (string fileName, float relVol) => Functions.DoFunction(() => ParseAudio(fileName, AudioHandler.AudioGroup.Sfx, relVol)));
            story.BindExternalFunction("Ambiance", (string fileName, float relVol) => Functions.DoFunction(() => ParseAudio(fileName, AudioHandler.AudioGroup.Ambiance, relVol)));
            story.BindExternalFunction("Music", (string fileName, float relVol) => Functions.DoFunction(() => ParseAudio(fileName, AudioHandler.AudioGroup.Music, relVol)));
        }

        // TODO: Perhaps incorporate this into the functions class and make that a monobehaviour?
        // NOTE: Depends on how much other functionality is left on this 
        // NOTE: It might be befitting to place these methods in the setdressed and textproducer objects, if that wouldn't require too much back and forth of variables

        #endregion
        // Story Handling
        #region Story Handling
        /// <summary>
        /// Main function driving changes in what is being shown on screen based on the story.
        /// </summary>
        public void AdvanceStory()
        {
            if (!Story.canContinue)
            {
                if (InputBroker.CanPresentChoices()) {
                    // present choices
                    StateMachine.TransitionToState(StoryController_State.awaitingChoiceState);
                } // try to make buttons
                {// if no buttons
                    OnInteractionEnd();
                }
            }
            else
            {
                InputBroker.StartCoroutine(Advance()); // reset the waitmarkers, and prepare behaviour for at the end of the text.
            }
        }
        /// <summary>
        /// Remove the "next line" marker.
        /// Prepares behaviour for at the end: isplay a "next line" icon if story is continueable, create buttons if not
        /// </summary>
        /// <returns></returns>
        private IEnumerator Advance()
        {
            StateMachine.TransitionToState(StoryController_State.productionState);
            PutDataIntoStash(); // stash current scene state
            TimeSinceAdvance = 0; // reset timer for skip button
            yield return TextProducer.ProduceTextOuter(); // Run text generator (until next stop or choice point.)
            StateMachine.FinishTask(StoryController_State.productionState); // activate continue marker or button
            StateMachine.TransitionToState(StoryController_State.awaitingInputState);
        }

        private IEnumerator HaltProcedings(float seconds)
        {
            Halted = true; // TODO: Incorporate into statemachine?
            yield return new WaitForSecondsRealtime(seconds);
            Halted = false;
        }

        public void OnInteractionEnd()
        {
            Story.RemoveVariableObserver();
            PutDataIntoStash();
            InkStoryAsset = null;
            Story = null;
            Debug.Log(new NotImplementedException());
            // TODO: LATER: evt volgende story feeden
        }

        private void ConsoleLogInk(string text, bool warning = false)
        {
            if (warning)
            {
                Debug.LogWarning("Warning from INK Script: " + text);
            }
            else
            {
                Debug.Log("Message from INK Script: " + text);
            }
        }

        #endregion

        #endregion
        // Peripheral
        #region Peripheral
        public class InkFunctions // NOTE: Can this be incorporated into statemachine? Else maybe I should split it into other (own) script.
        {
            // Private Properties
            #region Private Properties
            private StoryController Parser;

            #endregion
            // Constructors
            #region Constructors
            public InkFunctions() { }
            public InkFunctions(StoryController parser)
            {
                this.Parser = parser;
            }

            #endregion
            // Public Methods
            #region Public Methods
            public void DoFunction(Action function) // TODO: Incorporate into statemachine?
            {
                if (Parser.Peeking) return;
                Parser.Halted = true; // TODO: Incorporate into statemachine?
                function();
                Parser.Halted = false; // TODO: Incorporate into statemachine?
            }

            #endregion
        }
        #endregion

        // Deprecated
        #region Deprecated
        /*
        private bool IsValidFolder(string path)
        {
            return Directory.Exists(path);
        }

        */
        #endregion
        // UNRESOLVED
    }
}