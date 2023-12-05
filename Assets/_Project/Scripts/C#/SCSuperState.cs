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

        public class SCSuperState : SCDummyState
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
                Initialise();
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

            private void Initialise()
            {
                InitialiseStory();
                InitialiseData();
                PrepScene();
                FlagGoForStart();
            }

            /// <summary>
            /// Preps story for play. Should be called after <see cref="InkData"/> object has been initialised or loaded.
            /// </summary>
            private void InitialiseStory() 
            {
                Controller.Story = new Story(Controller.InkStoryAsset.text);
                BindAndObserve(Controller.Story); // this only needs to happen the first time!
            }

            private void InitialiseData()
            {
                // voorheen werd eventueel hier van disk gelezen, maar dat is niet meer zo. data wordt bij startup afgelezen en is daarna gewoon beschikbaar
                Controller.InkDataAsset = DataManager.Instance.GetDataOrMakeNew<StoryData>();
                ReadStoryStateFromData(Controller.InkDataAsset);
            }


            /// <summary>
            /// Prepares scene to contain story
            /// </summary>
            private void PrepScene()
            {
                AssetManager.Instance.CreateAssetLibraries();
                //Controller.TextProducer.SetMaxLines();
                Controller.TextProducer.maxVis = 20;
                Controller.InterfaceBroker.RemoveOptions();
                Controller.TextProducer.TextSpeedPreset = (TextProduction.TextSpeed)PlayerPrefs.GetInt("textSpeed", (int)TextProduction.TextSpeed.medium);
                PopulateSceneFromData(Controller.InkDataAsset);
            }


            private void BindAndObserve(Story story)
            {
                story.BindExternalFunction("Print", (string text) => PerformInkFunction(() => Controller.ConsoleLogInk(text, false)));
                story.BindExternalFunction("PrintWarning", (string text) => PerformInkFunction(() => Controller.ConsoleLogInk(text, true)));
                story.BindExternalFunction("_Spd", (float mod) => PerformInkFunction(() => Controller.TextProducer.Spd(mod / 100)));
                story.BindExternalFunction("_Clear", () => PerformInkFunction(() => Controller.TextProducer.ClearPage()));
                story.BindExternalFunction("_Halt", (float dur) => PerformInkFunction(() => PauseText(dur)));
                story.BindExternalFunction("_FadeToImage", (InkList image, float dur) => PerformInkFunction(() => Controller.SetDresser.SetBackground(ConvertListToItem(image), dur)));
                story.BindExternalFunction("_FadeToColor", (string color, float dur) => PerformInkFunction(() => Controller.SetDresser.SetColor(color, dur)));
                //story.BindExternalFunction("Sprites", (string fileNames) => PerformInkFunction(() => Controller.SetDresser.SetSprites(fileNames)));
                story.ObserveVariable("Portraits", (string varName, object newValue) => PerformInkFunction(() => Controller.SetDresser.SetSprites(newValue as InkList)));

                story.BindExternalFunction("_Vox_Play", (InkList clip, float relVol) => PerformInkFunction(() => Controller.SetDresser.InkRequestAudio(ConvertListToItem(clip), relVol)));
                story.BindExternalFunction("_Sfx_Play", (InkList clip, float relVol) => PerformInkFunction(() => Controller.SetDresser.InkRequestAudio(ConvertListToItem(clip), relVol)));
                story.BindExternalFunction("_Ambiance_Play", (InkList clip, float relVol) => PerformInkFunction(() => Controller.SetDresser.InkRequestAudio(ConvertListToItem(clip), relVol)));
                story.BindExternalFunction("_Ambiance_Remove", (InkList clip) => PerformInkFunction(() => Controller.SetDresser.RemoveAmbiance(ConvertListToItem(clip))));
                story.BindExternalFunction("_Ambiance_RemoveAll", () => PerformInkFunction(() => Controller.SetDresser.RemoveAmbianceAll()));

                story.BindExternalFunction("_Music_Play", (InkList clip, float relVol) => PerformInkFunction(() => Controller.SetDresser.InkRequestAudio(ConvertListToItem(clip), relVol)));
                story.BindExternalFunction("_Music_Stop", () => PerformInkFunction(() => Controller.SetDresser.StopMusic()));

                story.ObserveVariable("Inventory", (string varName, object newValue) => PerformInkFunction(() => Controller.InterfaceBroker.inventory.FetchItems(newValue as InkList)));
                //story.BindExternalFunction("AddInUnity", (string item) => PerformInkFunction(() => Debug.Log("Would have added item " + item)));
                //story.BindExternalFunction("RemoveInUnity", (string item) => PerformInkFunction(() => Debug.Log("Would have removed item " + item)));

                story.BindExternalFunction("PromptName", () => PerformInkFunction(() => Controller.PromptName()));



                // hoewel de map meestal met knop wordt opengemaakt, moet het ook uit verhaal kunnen:
                story.BindExternalFunction("_OpenMap", () => PerformInkFunction(() => Controller.InterfaceBroker.OpenMap()));
                    /*() =>
                    {
                        if (!Controller.TextProducer.Peeking)
                        {
                            Controller.InterfaceBroker.OpenMap();
                        }
                    });*/

                // a close map function doesn't make sense, becausei nk only controls while it is being run! if it paused, it can't do anything.
            }


            private InkListItem ConvertListToItem(InkList inkList)
            {
                if (inkList.Count == 1)
                {
                    InkListItem inkListItem = inkList.maxItem.Key;
                    //Debug.Log(String.Format("Found {0}", inkListItem));
                    return inkListItem;
                }
                else
                {
                    throw new Exception(String.Format("{0} contains more than 1 item!", inkList));
                }
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
                Controller.TextProducer.scriptedPause += seconds;
            }

            /// <summary>
            /// Feed the <paramref name="input"/>'s story state into the story we are currently loading.
            /// </summary>
            /// <param name="input">the data loaded from disk</param>
            private void ReadStoryStateFromData(StoryData input)
            {
                //Debug.Log(input.StoryStateJson);
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

                string message = "InkVars found:";
                foreach (string item in Controller.Story.state.variablesState)
                {
                    message += "\n" + item + ": " + Controller.Story.state.variablesState[item].ToString();
                }
                //Debug.Log(message);
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
                Controller.SetDresser.SetBackground(ConvertListToItem(background));


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
            protected void UpdateDataAsset()
            {
                // save all the things 
                Controller.InkDataAsset.CurrentText = Controller.TextProducer.CurrentText;
                Controller.InkDataAsset.HistoryText = Controller.TextProducer.PreviousText;
                Controller.InkDataAsset.StoryStateJson = Controller.Story.state.ToJson();
            }

            #endregion
        }
    }
}