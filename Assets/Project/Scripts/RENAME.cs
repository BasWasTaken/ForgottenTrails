using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Bas.Utility;
using DataService;
using Ink.Runtime;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using TMPro;

namespace ForgottenTrails.InkFacilitation
{
    partial class StoryController : MonoSingleton<StoryController>
    {
        public class SCDummyState : BaseState<StoryController>
        {

        }
    } 
}
namespace ForgottenTrails.InkFacilitation
{
    partial class StoryController : MonoSingleton<StoryController>
    {
        // Public Properties
        #region Public Properties
        public bool LoadingFromDisk { get; protected set; }
        public bool SavingToDisk { get; protected set; }
        public bool InteractingWithDisk => LoadingFromDisk | SavingToDisk;

        #endregion

        // Events
        #region Events
        public static event Action<Story> OnCreateStory;

        #endregion

        public class SCSuperState : BaseState<StoryController>
        {
            // Private Properties
            #region Private Properties
            protected float TimeSinceAdvance { get; set; } = 0;
            private bool GoForStart
            {
                get
                {
                    if (_goForStart == true)
                    {
                        _goForStart = false; // reset flag
                        return true;
                    }
                    else
                    {
                        return false;
                    };
                }
            }
            private bool _goForStart = false;
            private void FlagGoForStart()
            {
                _goForStart = true;
            }
            #endregion
            // Public Methods
            #region Public Methods
            public override void OnEnter()
            {
                PrepStory();
                PrepData();
                PrepScene();
                FlagGoForStart();
            }
            public override void OnUpdate()
            {
                base.OnUpdate();
                TimeSinceAdvance += Time.unscaledDeltaTime;
                if (GoForStart)
                {
                    StartStory();
                }

                if (Controller.StateMachine.CurrentState == this)
                {
                    Debug.Log("test message a from " + this);
                }
                if (Controller.StateMachine.CurrentState.GetType() == this.GetType())
                {
                    Debug.Log("test message b from " + this);
                }
            }
            public override void OnExit()
            {
                // Do whatever you need to exit playmode and return to main menu or whatever
            }

            #endregion
            // Private Methods
            #region Private Methods
            /// <summary>
            /// Preps story for play. Should be called after <see cref="InkData"/> object has been initialised or loaded.
            /// </summary>
            private void PrepStory()
            {
                Controller.Story = new Story(Controller.InkStoryAsset.text);
                BindAndObserve(Controller.Story);
            }

            private void BindAndObserve(Story story)
            {
                story.BindExternalFunction("Print", (string text) => PerformInkFunction(() => Controller.ConsoleLogInk(text, false)));
                story.BindExternalFunction("PrintWarning", (string text) => PerformInkFunction(() => Controller.ConsoleLogInk(text, true)));
                story.BindExternalFunction("Spd", (float mod) => PerformInkFunction(() => Controller.TextProducer.Spd(mod / 100)));
                story.BindExternalFunction("Clear", () => PerformInkFunction(() => Controller.TextProducer.ClearPage()));
                story.BindExternalFunction("Halt", (float dur) => PerformInkFunction(() => PauseText(dur)));
                story.BindExternalFunction("Bg", (string fileName, float dur) => PerformInkFunction(() => Controller.SetDresser.SetBackdrop(fileName, dur)));
                story.BindExternalFunction("FadeTo", (string color, float dur) => PerformInkFunction(() => Controller.SetDresser.SetColor(color, dur)));
                story.BindExternalFunction("Sprites", (string fileNames) => PerformInkFunction(() => Controller.SetDresser.SetSprites(fileNames)));
                story.BindExternalFunction("Vox", (string fileName, float relVol) => PerformInkFunction(() => Controller.SetDresser.ParseAudio(fileName, AudioHandler.AudioGroup.Voice, relVol)));
                story.BindExternalFunction("Sfx", (string fileName, float relVol) => PerformInkFunction(() => Controller.SetDresser.ParseAudio(fileName, AudioHandler.AudioGroup.Sfx, relVol)));
                story.BindExternalFunction("Ambiance", (string fileName, float relVol) => PerformInkFunction(() => Controller.SetDresser.ParseAudio(fileName, AudioHandler.AudioGroup.Ambiance, relVol)));
                story.BindExternalFunction("Music", (string fileName, float relVol) => PerformInkFunction(() => Controller.SetDresser.ParseAudio(fileName, AudioHandler.AudioGroup.Music, relVol)));
            }
            internal void PerformInkFunction(Action function)
            {
                Controller.TextProducer.PendingFunctions.Enqueue(function);
                Controller.StateMachine.TransitionToState(Controller.functionState);
            }
            internal void PauseText(float seconds)
            {
                Controller.TextProducer.additionalPause += seconds;
            }

            private void PrepData()
            {
                Controller.LoadingFromDisk = true;
                if (DataManager.Instance.DataAvailable(Controller.InkDataAsset.Key))
                {
                    Debug.Log("found data! trying to load...");
                    if (TryLoadData(Controller.InkDataAsset.Key, out InkDataClass dummy))
                    {
                        Controller.InkDataAsset = dummy;
                    }
                    else
                    {
                        throw new Exception("Could not load data.");
                    }
                }
                Controller.LoadingFromDisk = false;
            }
            private bool TryLoadData(string key, out InkDataClass output)
            {
                output = Controller.CreateBlankData();
                if (!DataManager.Instance.DataAvailable(key))
                {
                    Debug.LogError("Error code 404: No data found.");
                    return false;
                }
                else
                {
                    InkDataClass input = DataManager.Instance.FetchData<InkDataClass>(Controller.InkDataAsset.Key);
                    try
                    {
                        ReadStoryStateFromData(input);
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                    Debug.Log("Successfully loaded data!");
                    output = input;
                    return true;
                }

            }
            /// <summary>
            /// Feed the <paramref name="input"/>'s story state into the story we are currently loading.
            /// </summary>
            /// <param name="input">the data loaded from disk</param>
            private void ReadStoryStateFromData(InkDataClass input)
            {
                if (input.StoryStateJson != "")
                {
                    Debug.Log("continueing from savepoint!");
                    Controller.Story.state.LoadJson(input.StoryStateJson); // get storystate from json
                }
                else
                {
                    Debug.Log("no save point detected, starting from start");
                    Controller.Story.state.GoToStart();
                }
                Controller.Story.state.variablesState["Name"] = DataManager.Instance.MetaData.playerName; // get name from metadata

                string message = "InkVars found:";
                foreach (string item in Controller.Story.state.variablesState)
                {
                    message += "\n" + item + ": " + Controller.Story.state.variablesState[item].ToString();
                }
                Debug.Log(message);
            }

            /// <summary>
            /// Prepares scene to contain story
            /// </summary>
            private void PrepScene()
            {
                Controller.waitingForChoiceState.RemoveOptions();
                Controller.TextProducer._textSpeedPreset = (TextProduction.TextSpeed)PlayerPrefs.GetInt("textSpeed", (int)Controller.TextProducer._textSpeedPreset);
                PopulateSceneFromData(Controller.InkDataAsset);
            }

            private void PopulateSceneFromData(InkDataClass input)
            {
                //Debug.Log("This is when the textpanel is set to the contents of inkdata: " + textPanel.text);
                Controller.SetDresser.ParseAudio(input.SceneState.ActiveMusic, AudioHandler.AudioGroup.Music);
                Controller.SetDresser.ParseAudio(input.SceneState.ActiveAmbiance, AudioHandler.AudioGroup.Ambiance);
                Controller.SetDresser.SetBackdrop(input.SceneState.Background);
                Controller.SetDresser.SetSprites(input.SceneState.Sprites);
                Controller.TextProducer.Spd(input.SceneState.TextSpeedMod);
                Controller.TextProducer.Init(input.CurrentText, input.HistoryText);
            }

            /// <summary>
            /// Call creation event and transition to production state for the first time.
            /// </summary>
            private void StartStory()
            {
                OnCreateStory?.Invoke(Controller.Story);
                Controller.StateMachine.TransitionToState(Controller.productionState);
            }

            /// <summary>
            /// preps data for saving. happens whenever input is giving, i.e. before every production cycle.
            /// </summary>
            protected void StashStoryState()
            {
                // save all the things 
                Controller.InkDataAsset.CurrentText = Controller.TextProducer.CurrentText;
                Controller.InkDataAsset.HistoryText = Controller.TextProducer.PreviousText;
                Controller.InkDataAsset.StoryStateJson = Controller.Story.state.ToJson();
                Controller.InkDataAsset.StashData();
            }

            #endregion
        }
    } 
}


namespace ForgottenTrails.InkFacilitation
{
    partial class StoryController : MonoSingleton<StoryController>
    { 
        partial class TextProduction
        {
            // Public Properties
            #region Public Properties

            public bool EncounteredStop { get; private set; } = false;
            public bool Peeking { get; private set; } = false;
            public bool Skipping { get; private set; } = false;
            public enum TextProducerStatus
            {
                Idle,
                Working_Base,
                Working_Typing,
                Paused,
                Done
            }

            public TextProducerStatus TPStatus = TextProducerStatus.Idle;

            internal float additionalPause = 0f;
            #endregion
            public class SCProductionState : SCSuperState
            {
                // Private Properties
                #region Private Properties
                private string NewText
                {
                    get; set;
                }
                Coroutine coroutine;
                #endregion
                // Public Methods
                #region Public Methods
                public override void OnEnter()
                {
                    //start advance?
                }
                public override void OnUpdate()
                {
                    base.OnUpdate();                   

                    if (Controller.TextProducer.TPStatus == TextProducerStatus.Working_Typing)
                    {
                        if (!Controller.TextProducer.Skipping)
                        {
                            if (Input.GetKeyDown(KeyCode.Space))
                            {
                                if (TimeSinceAdvance > Controller.InterfaceBroker.SkipDelay)
                                {
                                    Controller.TextProducer.Skipping = true;
                                }
                            }
                        }
                    }
                    else if (Controller.TextProducer.TPStatus == TextProducerStatus.Done)
                    {
                        Controller.TextProducer.TPStatus = TextProducerStatus.Idle;
                        StashStoryState(); // stash current scene state
                        DetermineNextTransition();
                    }
                    else if (Controller.TextProducer.TPStatus == TextProducerStatus.Idle)
                    {
                        AdvanceStory();
                    }
                }
                public override void OnExit()
                {
                    //suspend coroutine?
                }
                #endregion
                // Private Methods
                #region Private Methods
                private void AdvanceStory()
                {
                    TimeSinceAdvance = 0; // reset timer for skip button
                    Controller.TextProducer.TPStatus = TextProducerStatus.Working_Base; // NOTE OR transitionto writing
                    Controller.StartCoroutine(ProduceTextOuter()); // TODO perhaps move this method to here too
                }
                /// <summary>
                /// Runs the per line steps required for parsing and showing ink story
                /// </summary>
                public IEnumerator ProduceTextOuter()
                {
                    #region TryFit
                    Stopwatch stopwatch = new();
                    if (Controller.TextProducer.ClearWhenFull)
                    {
                        // TODO: construct future checker for all upcoming lines here
                        var storedState = Controller.Story.state.ToJson(); // set return point
                        Controller.TextProducer.Peeking = true; // begin traversal
                        string toBeAdded = "";
                        while (Controller.Story.canContinue) // continue maximally or to stop
                        {
                            toBeAdded += Controller.Story.Continue();
                            if (toBeAdded.Contains("{stop}")) break;
                        }

                        // check size and clear page if needed

                        string bufferText = Controller.TextProducer.CurrentText; // make backup
                        Controller.TextProducer.VisibleCharacters = Controller.TextProducer.TextBox.text.Length;

                        Controller.TextProducer.CurrentText += toBeAdded + '\n'; // test if the text will fit
                        yield return 0;// wait 1 frame
                        bool overflow = Controller.TextProducer.OverFlowTextBox.text.Length > 0; // store result

                        Controller.TextProducer.CurrentText = bufferText; // restore backup
                        Controller.Story.state.LoadJson(storedState); // return to original state, reverting from peaking
                        Controller.TextProducer.Peeking = false;

                        if (overflow) { Controller.TextProducer.ClearPage(); } // clear page if needed
                    }

                    #endregion TryFit
                    #region continueloop
                    do
                    {
                        if (Controller.StateMachine.CurrentState != this) yield return new WaitUntil(() => Controller.StateMachine.CurrentState == this);

                        // NOTE: do i want to add ifspace remaining in textbox check here per line?

                        #region InnerLoop
                        Controller.Story.ContinueAsync(0); // advance a bit 
                        string newLine = Controller.Story.currentText; // get the next line up until there
                                                                       // string newLine = story.Continue();

                        string parsedText = ParseText(newLine);

                        NewText = parsedText;
                        //Debug.Log("Write a line: " + newLine);
                        if (Controller.TextProducer.PendingFunctions.Count > 0) 
                        {
                            yield return new WaitUntil(() => Controller.TextProducer.PendingFunctions.Count==0); // wait till functions dfone
                        }
                        if (Controller.StateMachine.CurrentState != this) 
                        { 
                            yield return new WaitUntil(() => Controller.StateMachine.CurrentState == this); // only continue in right state
                        } 


                        Controller.TextProducer.TPStatus = TextProducerStatus.Working_Typing;  // Indicate we are now typing

                        if (Controller.TextProducer.VisibleCharacters < Controller.TextProducer.CurrentText.Length)
                        {
                            Controller.TextProducer.VisibleCharacters = Controller.TextProducer.CurrentText.Length;
                            Debug.LogWarning("Not all characters were done.");
                        }

                        string backup = Controller.TextProducer.CurrentText; // make backup
                        Controller.TextProducer.CurrentText += NewText; //add the new text, invisibly
                        if (Controller.TextProducer.OverFlowTextBox.text.Length > 0) // check for overflow
                        {
                            Controller.TextProducer.CurrentText = backup; // restore backup
                            Controller.TextProducer.ClearPage(); // save page and clear it
                            Controller.TextProducer.CurrentText = NewText; // add new text anyway
                        }

                        #region RevealLetters
                        string message = string.Format("On speed {0} (base {1}) wrote:\nChar\t\tDelay\t\tIntended\t\tExtra", Controller.TextProducer.TextSpeedActual, Controller.TextProducer.TextSpeedPreset); // prepare console message
                        int tagLevel = 0; ///int to remember if we go down any nested tags
                        char letter;///prepare marker             
                        while (Controller.TextProducer.VisibleCharacters < Controller.TextProducer.TextBox.text.Length) /// while not all characters are visible
                        {
                            if (!Controller.isActiveAndEnabled) yield return new WaitUntil(() => Controller.isActiveAndEnabled); // only continue if enabled
                            if (Controller.StateMachine.CurrentState != this) yield return new WaitUntil(() => Controller.StateMachine.CurrentState == this); // only continue in right state
                            letter = Controller.TextProducer.TextBox.text[Controller.TextProducer.VisibleCharacters]; /// store letter we're typing letter   
                            Controller.TextProducer.VisibleCharacters++; /// show the character
                            if (letter == '<')/// if we come across this we've entered a(nother) tag
                            {
                                tagLevel++;
                            }
                            else if (letter == '>')/// if we come acros this, we've exited one level of tag
                            {
                                tagLevel--;
                                if (tagLevel < 0)
                                {
                                    throw new("That's more closing than opening tag brackets!");
                                }
                            }
                            else if (tagLevel == 0) /// if we're not in a tag, apply potential delays for the typewriting effect
                            {
                                //Debug.Log("show me " + letter);
                                float delay = 0; /// initialize delay
                                if (!Controller.TextProducer.Skipping) /// in normal behaviour
                                {
                                    /// get the delay from the delay info object
                                    delay = Controller.TextProducer.PauseInfoNormal.GetPause(letter) / Controller.TextProducer.TextSpeedActual;
                                }
                                else if (Controller.TextProducer.AlwaysPause) /// if this setting is enabled, 
                                {
                                    ///get pause info while skipping text too
                                    delay = Controller.TextProducer.PauseInfoSkipping.GetPause(letter);
                                }
                                if (delay > 0) yield return new WaitForSecondsRealtime(delay); /// apply the delay if any
                                float delayMs = delay * 1000 + Controller.TextProducer.additionalPause;
                                float delayActual = stopwatch.ElapsedMilliseconds;
                                message += string.Format("\n\'{0}\'\t\t{1} ms\t\t{2} ms\t\t {3} ms", letter, delayActual, delayMs, delayActual - delayMs);
                                stopwatch.Restart();
                            }
                        }

                        Debug.Log(message);
                        #endregion RevealLetters

                        Controller.TextProducer.TPStatus = TextProducerStatus.Working_Base;
                        #endregion InnerLoop

                        if (Controller.TextProducer.EncounteredStop)  // if we encounter a stop
                        {
                            Controller.TextProducer.EncounteredStop = false;
                            // exit the loop or continue with a small delay
                            if (Controller.TextProducer.AutoAdvance)
                            {
                                yield return new WaitForFixedUpdate();
                            }
                            else
                            {
                                break;
                            }
                        }
                    } while (Controller.Story.canContinue); // while the story can continue without input on choices
                    #endregion continueloo
                    Controller.TextProducer.Skipping = false; // turn of skipping if it was on

                    Controller.TextProducer.TPStatus = TextProducerStatus.Done;
                }
                internal void ClearPage()
                {
                    if (Controller.TextProducer.Peeking) return;
                    Controller.TextProducer.HistoryTextBox.text = Controller.TextProducer.CurrentText; // move all text to the history log
                    Controller.TextProducer.CurrentText = ""; // clear current and prospective texts
                    Controller.TextProducer.VisibleCharacters = 0;
                }

                internal string ParseText(string input)
                {
                    Regex tags = new(@"\{([^>]+)\}");

                    foreach (Match match in tags.Matches(input))
                    {
                        //Debug.Log(string.Format("Encountered command {0} in {1}",match.Value,output));

                        if (match.Value == "{stop}")
                        {
                            //Debug.Log("recognised stop command");
                            Controller.TextProducer.EncounteredStop = true;
                            //catchAfterStop = input.Substring(match.Index+match.Value.Length);

                            //input = input.Substring(0, match.Index); 
                        }
                        else if (match.Value == "{glue}")
                        {
                            input = input.TrimEnd('\n'); /// remove linebreak from this
                        }
                        else if (match.Value == "{aglue}")
                        {
                            Controller.TextProducer.TextBox.text = Controller.TextProducer.TextBox.text.TrimEnd('\n'); /// remove linebreak from previous
                        }

                        input = input.Replace(match.Value, ""); /// remove the command
                    }

                    string output = "";

                    if (input == "<br>\n" | input == "<br>") /// when hitting explicit linebreak //which is it?
                    {
                        output += "\n"; //add a newline
                    }
                    else
                    {
                        output = input; /// leave the input unaltered
                    }

                    //Debug.Log("Write line as: " + output);
                    return output;
                }

                private void DetermineNextTransition()
                {
                    if (Controller.Story.canContinue)
                    {
                        Machine.TransitionToState(Controller.waitingForContinueState);
                    }
                    else
                    {
                        if (Controller.InterfaceBroker.CanPresentChoices())
                        {
                            Machine.TransitionToState(Controller.waitingForChoiceState);
                        }
                        else
                        {
                            OnInteractionEnd();
                        }
                    }
                }
                private void OnInteractionEnd()
                {
                    // Disengage with current story / dialogue, as we have seen its end or chose a goodbye option.
                    Controller.Story.RemoveVariableObserver();
                    StashStoryState();
                    Controller.InkStoryAsset = null;
                    Controller.Story = null;
                    Debug.Log(new NotImplementedException());
                    // TODO: LATER: evt volgende story feeden
                    Controller.StateMachine.DropState(this);
                }

                #endregion
            }
        }
        
    } 
}
namespace ForgottenTrails.InkFacilitation
{
    partial class StoryController : MonoSingleton<StoryController>
    {
        partial class TextProduction
        {
            internal Queue<Action> PendingFunctions;
            public class SCFunctionState: SCProductionState
            {
                public override void OnEnter()
                {

                }
                public override void OnUpdate()
                {
                    base.OnUpdate();
                    int safetyInt = 0;
                    while (Controller.TextProducer.PendingFunctions.TryDequeue(out Action function))
                    {
                        function();
                        safetyInt++;
                        if (safetyInt > 100)
                        {
                            throw new OverflowException();
                        }
                    }
                    if(Controller.TextProducer.PendingFunctions.Count == 0) 
                    {
                        Machine.DropState(this);
                    }
                }
                public override void OnExit()
                {

                }
            }
        }
        
    }
}

namespace ForgottenTrails.InkFacilitation
{
    partial class StoryController : MonoSingleton<StoryController>
    {
        partial class InterfaceBroking
        {
            public class SCWaitingForInputState : SCSuperState
            {
                // Public Properties
                #region Public Properties

                #endregion
                // Private Properties
                #region Private Properties

                protected bool InputReceived
                {
                    get
                    {
                        if (_inputReceived == true)
                        {
                            _inputReceived = false; // reset flag
                            return true;
                        }
                        else
                        {
                            return false;
                        };
                    }
                }
                private bool _inputReceived = false;
                protected void RegisterInput()
                {
                    _inputReceived = true;
                }
                #endregion
                // Public Methods
                #region Public Methods
                public override void OnEnter()
                {
                    // Debug.Log("Turning input on.");
                }
                public override void OnUpdate()
                {
                    base.OnUpdate();
                    if (InputReceived)
                    {
                        BaseState<StoryController> state;
                        if (Machine.CurrentState == Controller.waitingForChoiceState)
                        {
                            state = Controller.waitingForChoiceState;
                        }
                        else if (Machine.CurrentState == Controller.waitingForContinueState)
                        {
                            state = Controller.waitingForContinueState;
                        }
                        else
                        {
                            state = this;
                        }
                        Machine.DropState(state);
                    }
                }
                public override void OnExit()
                {
                    // Debug.Log("Turning input off.");
                }
                #endregion
                // Private Methods
                #region Private Methods

                #endregion
            }
        }
    } 
}

namespace ForgottenTrails.InkFacilitation
{
    partial class StoryController : MonoSingleton<StoryController>
    {
        partial class InterfaceBroking
        {
            public class SCWaitingForChoiceState : SCWaitingForInputState
            {
                // Public Properties
                #region Public Properties

                #endregion
                // Private Properties
                #region Private Properties

                #endregion
                // Public Methods
                #region Public Methods
                public override void OnEnter()
                {
                    PresentButtons(); // create new choices
                }
                public override void OnUpdate()
                {
                    base.OnUpdate();
                }
                public override void OnExit()
                {
                    RemoveOptions(); // Destroy old choices
                }
                #endregion
                // Private Methods
                #region Private Methods
                internal void PresentButtons()
                {
                    if (Controller.Story.canContinue)
                    {
                        throw new Exception("no choices detected at this point");
                    }
                    else if (Controller.Story.currentChoices.Count > 0) /// Display all the choices, if there are any!
                    {
                        //Debug.Log("Choices detected!");
                        for (int i = 0; i < Controller.Story.currentChoices.Count; i++)
                        {

                            Choice choice = Controller.Story.currentChoices[i];
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
                        throw new NotImplementedException("No choices possible");
                    }
                }
                internal void RemoveOptions()// Destroys all the buttons from choices
                {
                    foreach (Button child in Controller.InterfaceBroker.ButtonAnchor.GetComponentsInChildren<Button>())
                    {
                        Destroy(child.gameObject);
                    }
                }

                // Creates a button showing the choice text
                private Button PresentButton(string text)
                {
                    Debug.Log("make button for " + text);
                    /// Creates the button from a prefab
                    Button choice = Instantiate(Controller.InterfaceBroker.ButtonPrefab) as Button;
                    choice.transform.SetParent(Controller.InterfaceBroker.ButtonAnchor, false);

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
                    Controller.Story.ChooseChoiceIndex(choice.index); /// feed the choice
                    Controller.InkDataAsset.StoryStateJson = Controller.Story.state.ToJson(); /// record the story state
                    RegisterInput();
                    Machine.TransitionToState(Controller.savingState);
                }
                #endregion
            }
        }
    }
}
namespace ForgottenTrails.InkFacilitation
{
    partial class StoryController : MonoSingleton<StoryController>
    {
        partial class InterfaceBroking
        {
            public class SCWaitingForContinueState : SCWaitingForInputState
            {
                // Inspector Properties
                #region Inspector Properties

                #endregion
                // Public Properties
                #region Public Properties

                #endregion
                // Private Properties
                #region Private Properties

                #endregion
                // Public Methods
                #region Public Methods
                public override void OnEnter()
                {
                    Controller.InterfaceBroker.FloatingMarker.gameObject.SetActive(true); // else set bouncing triangle at most recent line
                }
                public override void OnUpdate()
                {
                    base.OnUpdate();
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        if (Controller.Story.canContinue)
                        {
                            RegisterInput();
                            Machine.TransitionToState(Controller.savingState);
                        }
                    }                    
                }
                public override void OnExit()
                {
                    Controller.InterfaceBroker.FloatingMarker.gameObject.SetActive(false); // remove marker 
                }
                #endregion
                // Private Methods
                #region Private Methods

                #endregion
            }
        } 
        
    } 
}
namespace ForgottenTrails.InkFacilitation
{
    partial class StoryController : MonoSingleton<StoryController>
    {
        public class SCGameMenuState : SCSuperState
        {
            // Inspector Properties
            #region Inspector Properties

            #endregion
            // Public Properties
            #region Public Properties

            #endregion
            // Private Properties
            #region Private Properties

            #endregion
            // Public Methods
            #region Public Methods
            public override void OnEnter()
            {

            }
            public override void OnUpdate()
            {
                base.OnUpdate();
            }
            public override void OnExit()
            {

            }
            #endregion
            // Private Methods
            #region Private Methods

            #endregion
        } } }
namespace ForgottenTrails.InkFacilitation
{
    partial class StoryController : MonoSingleton<StoryController>
    {
        public class SCInventoryState : SCGameMenuState
        {
            // Inspector Properties
            #region Inspector Properties

            #endregion
            // Public Properties
            #region Public Properties

            #endregion
            // Private Properties
            #region Private Properties

            #endregion
            // Public Methods
            #region Public Methods
            public override void OnEnter()
            {

            }
            public override void OnUpdate()
            {
                base.OnUpdate();
            }
            public override void OnExit()
            {

            }
            #endregion
            // Private Methods
            #region Private Methods

            #endregion
        } } }
namespace ForgottenTrails.InkFacilitation
{
    partial class StoryController : MonoSingleton<StoryController>
    {

        public class SCSettingsState : SCGameMenuState
        {
            // Inspector Properties
            #region Inspector Properties

            #endregion
            // Public Properties
            #region Public Properties

            #endregion
            // Private Properties
            #region Private Properties

            #endregion
            // Public Methods
            #region Public Methods
            public override void OnEnter()
            {

            }
            public override void OnUpdate()
            {
                base.OnUpdate();
            }
            public override void OnExit()
            {

            }
            #endregion
            // Private Methods
            #region Private Methods

            #endregion
        } } }
namespace ForgottenTrails.InkFacilitation
{
    partial class StoryController : MonoSingleton<StoryController>
    {
        public class SCSavingState : SCSuperState
        {
            // Inspector Properties
            #region Inspector Properties

            #endregion
            // Public Properties
            #region Public Properties

            #endregion
            // Private Properties
            #region Private Properties

            #endregion
            // Public Methods
            #region Public Methods
            public override void OnEnter()
            {
                DataManager.Instance.OnDataSaved += Release;
                Controller.SavingToDisk = true;

                DataManager.Instance.WriteStashedDataToDisk();
            }
            public override void OnUpdate()
            {
                base.OnUpdate();
            }
            public override void OnExit()
            {
                DataManager.Instance.OnDataSaved -= Release;
            }
            #endregion
            // Private Methods
            #region Private Methods
            private void Release()
            {
                Controller.SavingToDisk = false;
                Machine.DropState(this);
            }
            #endregion
        }
    }
    
}
