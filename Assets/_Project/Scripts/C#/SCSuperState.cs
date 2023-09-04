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
using System;

namespace ForgottenTrails.InkFacilitation
{
    public partial class StoryController : MonoSingleton<StoryController>
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
                    //Debug.Log("test message a from " + this);
                }
                if (Controller.StateMachine.CurrentState.GetType() == this.GetType())
                {
                    //Debug.Log("test message b from " + this);
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
                story.BindExternalFunction("_Spd", (float mod) => PerformInkFunction(() => Controller.TextProducer.Spd(mod / 100)));
                story.BindExternalFunction("_Clear", () => PerformInkFunction(() => Controller.TextProducer.ClearPage()));
                story.BindExternalFunction("_Halt", (float dur) => PerformInkFunction(() => PauseText(dur)));
                story.BindExternalFunction("_FadeToImage", (InkListItem image, float dur) => PerformInkFunction(() => Controller.SetDresser.SetBackground(image, dur)));
                story.BindExternalFunction("_FadeToColor", (string color, float dur) => PerformInkFunction(() => Controller.SetDresser.SetColor(color, dur)));
                //story.BindExternalFunction("Sprites", (string fileNames) => PerformInkFunction(() => Controller.SetDresser.SetSprites(fileNames)));
                story.ObserveVariable("Portraits", (string varName, object newValue) => PerformInkFunction(() => Controller.SetDresser.SetSprites(newValue as InkList)));

                story.BindExternalFunction("_Vox_Play", (InkListItem clip, float relVol) => PerformInkFunction(() => Controller.SetDresser.InkRequestAudio(clip, relVol)));
                story.BindExternalFunction("_Sfx_Play", (InkListItem clip, float relVol) => PerformInkFunction(() => Controller.SetDresser.InkRequestAudio(clip, relVol)));
                story.BindExternalFunction("_Ambiance_Play", (InkListItem clip, float relVol) => PerformInkFunction(() => Controller.SetDresser.InkRequestAudio(clip, relVol)));
                story.BindExternalFunction("_Ambiance_Remove", (InkListItem clip) => PerformInkFunction(() => Controller.SetDresser.RemoveAmbiance(clip)));
                story.BindExternalFunction("_Ambiance_RemoveAll", () => PerformInkFunction(() => Controller.SetDresser.RemoveAmbianceAll()));

                story.BindExternalFunction("_Music_Play", (InkListItem clip, float relVol) => PerformInkFunction(() => Controller.SetDresser.InkRequestAudio(clip, relVol)));
                story.BindExternalFunction("_Music_Stop", () => PerformInkFunction(() => Controller.SetDresser.StopMusic()));
                
                story.ObserveVariable("Inventory", (string varName, object newValue) => PerformInkFunction(() => Controller.InterfaceBroker.inventory.FetchItems(newValue as InkList)));
                //story.BindExternalFunction("AddInUnity", (string item) => PerformInkFunction(() => Debug.Log("Would have added item " + item)));
                //story.BindExternalFunction("RemoveInUnity", (string item) => PerformInkFunction(() => Debug.Log("Would have removed item " + item)));
            }
            internal void PerformInkFunction(Action function)
            {
                if (!Controller.TextProducer.Peeking)
                {
                    Controller.TextProducer.PendingFunctions.Enqueue(function);
                    Controller.StateMachine.TransitionToState(Controller.functionState);
                }
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
                    if (TryLoadData(Controller.InkDataAsset.Key, out StoryData dummy))
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
            private bool TryLoadData(string key, out StoryData output)
            {
                output = Controller.CreateBlankData();
                if (!DataManager.Instance.DataAvailable(key))
                {
                    Debug.LogError("Error code 404: No data found.");
                    return false;
                }
                else
                {
                    StoryData input = DataManager.Instance.FetchData<StoryData>(Controller.InkDataAsset.Key);
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
            private void ReadStoryStateFromData(StoryData input)
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

            private void PopulateSceneFromData(StoryData input)
            {
                //Debug.Log("This is when the textpanel is set to the contents of inkdata: " + textPanel.text);
                InkList music = Controller.Story.state.variablesState["Music"] as InkList;                
                Controller.SetDresser.InkRequestAudio(music.maxItem.Key);

                InkList ambiance = Controller.Story.state.variablesState["Ambiance"] as InkList;
                foreach (InkListItem item in ambiance.Keys)
                {
                    Controller.SetDresser.InkRequestAudio(item);
                }

                InkList background = Controller.Story.state.variablesState["Background"] as InkList;
                Controller.SetDresser.SetBackground(background.maxItem.Key);


                Controller.TextProducer.Spd((float)Controller.Story.state.variablesState["Speed"]);

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