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
    [RequireComponent(typeof(InkInputHandler))]
    [RequireComponent(typeof(AudioHandler))]
    public class InkParser : MonoSingleton<InkParser>
    {
        // TODO: Rename to StoryController
        // Constants
        #region Constants
        private const string dataLabel = "BasicInkScript"; // NOTE:  isn't this ridiculous? if you'll be using this object as ink interface all the time, it should't itelf store particular data, you should have objets etc store data... else all will be under here, won't it? or will it just be settings?
        #endregion

        // Inspector Properties
        #region Inspector Properties
        [field:SerializeField, BoxGroup("Assets"), Required]
        [Tooltip("Here drag the JSON object containing the dialogue behaviour")]
        private TextAsset InkStoryAsset { get; set; }

        [SerializeField, BoxGroup("Assets"), ReadOnly]
        [Tooltip("Data object containing INK data.")]
        private InkDataClass inkDataAsset;

        //Inspector Helpers
        #region Inspector Helpers
        [Tooltip("Reset ink data in object. Note: does not remove data from file")]
        [Button("ResetInkData", EButtonEnableMode.Editor)]public void ResetInkDataButton() => ResetInkDataButton();

        [Tooltip("Load data from disk and reset scene.")]
        [Button("LoadData")] public void LoadDataButton() => LoadData();

        #endregion

        #endregion
        // Public Properties
        #region Public Properties
        public Story Story { get; set; }
        public Dictionary<VariablesState, object> InkVars { get; set; } = new();

        public InkFunctions Functions { get; set; } // TODO: Incorporate this into class or machine?

        public bool Halted { get; private set; } // TODO: Incorporate into statemachine
        public bool Peeking => TextProducer.peeking; // TODO: Incorporate into statemachine
        public bool Playing { get; private set; } = false; // TODO: incorporate into statemachine
        void StopPlayingStory() // TODO: Incorporate into statemachine
        {
            Playing = false;
        }

        #endregion
        // Private Properties
        #region Private Properties
        private AudioHandler AudioHandler;
        private TextProducer TextProducer;
        private SetDresser SetDresser;
        private InkInputHandler InputHandler;

        #endregion
        // Events
        #region Events
        public static event Action<Story> OnCreateStory; // TODO: Check if this is used anywhere.

        #endregion
        // MonoBehaviour LifeCycle Methods
        #region MonoBehaviour LifeCycle Methods
        override protected void Awake()
        {
            base.Awake();
            AudioHandler = GetComponent<AudioHandler>();
            Functions = new(this);
            TextProducer = GetComponent<TextProducer>();
            InputHandler = GetComponent<InkInputHandler>();
            SetDresser = GetComponent<SetDresser>();

            if (true) // NOTE: why do i need to make blank data here always? is it just so that i don't get nullerrors later?
            {
                inkDataAsset = CreateBlankData(true);
            }
        }

        private void Start()
        {
            transform.localPosition = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y); // NOTE: Why do I do this?
            if (InkStoryAsset != null)
            {
                PrepStory();
                StartStory();
            }
        }

        private void Update()
        {
            if (!false) // if not pause // TODO: Incorporate into Statemachine 
            {
                if (true) // if handling input // TODO: Incorporate into statemachine?
                {
                    switch (TextProducer.State) // check that state's input // TODO: Incorporate into statemachine
                    {
                        // dit kan mooier met een machine state en interne update loops daarin., like state.update() (met daarin een .oninpout en .always ofzo).
                        case TextProducer.PState.Booting:
                            break;
                        case TextProducer.PState.Idle:
                            if (Input.GetKeyDown(KeyCode.Space))
                            {
                                if (Story.canContinue)
                                {
                                    if (InputHandler.HandleAdvancement)
                                    {
                                        AdvanceStory();
                                    }
                                    else
                                    {
                                        Debug.LogWarning("Cannot continue");
                                    }
                                }
                            }
                            break;
                        case TextProducer.PState.Producing:
                            if (Input.GetKeyDown(KeyCode.Space))
                            {
                                if (InputHandler.TimeSinceAdvance > InputHandler.SkipDelay)
                                {
                                    TextProducer.SkipLines();
                                }
                            }
                            if (Input.GetKeyDown(KeyCode.Escape))
                            {
                                if (TextProducer.Skipping) TextProducer.ResetSkipping();
                            }
                            break;
                        case TextProducer.PState.Stuck:
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        #endregion
        // Public Methods
        #region Public Methods

        #endregion
        // Private Methods
        #region Private Methods

        #region Ink-Related Methods
        /// <summary>
        /// Preps story for play. Should be called after <see cref="InkData"/> object has been initialised or loaded.
        /// </summary>
        void PrepStory()
        {
            InputHandler.RemoveOptions();
            Story = new Story(InkStoryAsset.text);
            OnCreateStory?.Invoke(Story); // NOTE: Is it okay that I moved this to be before instead of after the next few lines?
            Story.state.variablesState["Name"] = DataManager.Instance.MetaData.playerName; // get name from metadata
            BindAndObserve(Story);
        }

        /// <summary>
        /// Creates a new Story object with the compiled story which we can then play!
        /// </summary>
        private void StartStory()
        {
            if (DataManager.Instance.DataAvailable(InkDataAsset.Key))
            {
                Debug.Log("found data! trying to load...");
                TryLoadData();
            }
            else
            {
                InkDataAsset = CreateBlankData(); // NOTE: you're making data a second time, there's already blank data made.
            }
            if (!Playing)
            {
                Playing = true;
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
            else
            {
                Debug.LogError("Still playing according to bool!");
            }
        }

        private void BindAndObserve(Story story)
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

        public void Spd(float speed)
        {
            InkDataAsset.SceneState.TextSpeedMod = speed;
            TextProducer.Spd(speed);
        }

        private IEnumerator HaltProcedings(float seconds)
        {
            Halted = true; // TODO: Incorporate into statemachine?
            yield return new WaitForSecondsRealtime(seconds);
            Halted = false;
        }




        #endregion
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

        /// <summary>
        /// preps data for saving. happens at the end of every bit of dialogue.
        /// </summary>
        private void PutDataIntoStash() // this should then be called every so often and whenever the game is saved
        {
            // save all the things 
            inkDataAsset.CurrentText = TextProducer.CurrentText; // NOTE: is deze nodig? is dat niet contained in story?
            inkDataAsset.HistoryText = TextProducer.PreviouslyDisplayed;
            inkDataAsset.StoryStateJson = Story.state.ToJson();
            inkDataAsset.StashData();
        }

        private void SaveData()
        {
            PutDataIntoStash();
            DataManager.Instance.WriteStashedDataToDisk();
        }

        public bool TryLoadData()
        {// TODO: If this needs to be public, move it accordingly.
            return TryLoadData(ref inkDataAsset);
        }

        private bool TryLoadData(ref InkDataClass output)
        {
            // NOTE: Does this need the ref variable? Can't it just affect a property? Using a buffer field if needed.
            if (!DataManager.Instance.DataAvailable(output.Key))
            {
                Debug.LogError("Error code 404: No data found available.");
                return false;
            }
            else
            {
                InkDataClass input = DataManager.Instance.FetchData<InkDataClass>(inkDataAsset.Key);
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


        #endregion
        #endregion
        // Peripheal
        #region Peripheal
        public class InkFunctions // NOTE: Can this be incorporated into statemachine?
        {
            // Private Properties
            #region Private Properties
            private InkParser Parser;

            #endregion
            // Constructors
            #region Constructors
            public InkFunctions() { }
            public InkFunctions(InkParser parser)
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

        }
        #endregion

        // Deprecated
        /* Deprecated
        private bool IsValidFolder(string path)
        {
            return Directory.Exists(path);
        }
        */
        // UNRESOLVED
        private void ResetInkData()
        {
            InkDataAsset = CreateBlankData();
            PrepStory();
        }

        


        private void LoadData() 
        {
            #if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlaying == true)
            {
                StopPlayingStory();
            }
            #endif 
            ResetInkDataButton();
            #if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlaying == true)
            {
                StartStory();
                SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
            }
            #endif
        }






        
        // -- continue here!
        
        

        

        

        



        
        private void ParseAudio(string fileName, GlobalAudioManager.AudioGroup audioGroup, float relVol =.5f)
        {
            if (peeking) return;
            AudioClip audioClip = null; /// clear audio if no other value is given
            if (!(fileName == "" | fileName == "null"))
            {
                try
                {
                    if (!AssetManager.Instance.AudioClips.TryGetValue(fileName.ToLower(), out AudioClip audioClip1))
                    {
                        throw new FileNotFoundException("File not found: " + fileName);
                    }
                    else
                    {
                        audioClip = audioClip1;
                    }
                }
                catch (Exception e)
                {
                    // Extract some information from this exception, and then
                    // throw it to the parent method.
                    if (e.Source != null)
                        Console.WriteLine("IOException source: {0}", e.Source);
                    Debug.LogException(e);
                }
            }
            if (relVol > 1)
            {
                Debug.LogWarning(string.Format("Relative volume of {0} exceeds accepted cap of 1.", relVol));
                relVol = 1;
            }
            else if (relVol < 0)
            {
                Debug.LogWarning(string.Format("Relative volume of {0} does not exceed minimum value of 0.", relVol));
                relVol = 0;
            }
            bool oneShot = false;
            bool loop = false;
            if(audioGroup == GlobalAudioManager.AudioGroup.Music)
            {
                //oneShot = false;
                loop = true;
                InkDataAsset.sceneState.activeMusic = fileName;
            }
            else if (audioGroup == GlobalAudioManager.AudioGroup.Ambiance)
            {
                //oneShot = false;
                loop = true;
                InkDataAsset.sceneState.activeAmbiance = fileName;
            }
            PlayAudio(audioClip, audioGroup, relVol, oneShot: oneShot,loop:loop);
        }
        private void PlayAudio(AudioClip audioClip, GlobalAudioManager.AudioGroup audioGroup, float relVol = .5f, bool oneShot = false, bool loop = false)
        {
            AudioSource audioSource = audioGroup switch
            {
                GlobalAudioManager.AudioGroup.Sfx => audioSourceSfx,
                GlobalAudioManager.AudioGroup.Ambiance => audioSourceAmbiance,
                GlobalAudioManager.AudioGroup.Music => audioSourceMusic,
                GlobalAudioManager.AudioGroup.Voice => audioSourceVox,
                GlobalAudioManager.AudioGroup.System => audioSourceSystem,
                _ => audioSourceSystem,
            };
            if (audioClip == null)
            {
                audioSource.clip = null;
                audioSource.Stop();
            }
            else
            {
                if (oneShot)
                {
                    audioSource.PlayOneShot(audioClip, relVol);
                }
                else
                {
                    if (audioSource.clip != audioClip) /// if it's a different clip than before, start playing at volume
                    {
                        audioSource.clip = audioClip;
                        audioSource.volume = relVol;
                        audioSource.Play();
                    }
                    else /// otherwise just apply the volume, but gradually
                    {
                        StartCoroutine(ShiftVolumeGradually(audioSource,audioClip,relVol));
                    }

                    if (loop)
                    {
                        audioSource.loop = true;
                    }
                    else
                    {
                        audioSource.loop = false;
                        StartCoroutine(RemoveClipWhenFinished(audioSource));
                    }
                }
            }
        }
        IEnumerator ShiftVolumeGradually(AudioSource audioSource, AudioClip audioClip, float relVol)
        {
            float t = .1f; ///how long to wait before each increment
            float d = .01f; ///size of an increment
            while (audioSource.clip == audioClip & audioSource.volume != relVol)
            {
                yield return new WaitForSecondsRealtime(t);
                audioSource.volume = Mathf.MoveTowards(audioSource.volume, relVol, d);
            }
        }

        IEnumerator RemoveClipWhenFinished(AudioSource audioSource)
        {
            if (audioSource.isPlaying)yield return new WaitWhile(() => audioSource.isPlaying);
            audioSource.clip = null;
        }

        private void ConsoleLogInk(string text, bool warning = false)
        {
            if (peeking) return;
            if (warning)
            {
                Debug.LogWarning("Warning from INK Script: " + text);
            }
            else
            {
                Debug.Log("Message from INK Script: " + text);
            }
        }

        
        
        

        protected void PopulateStoryVarsFromData(InkData input, ref InkData output)
        {
            string message = "InkVars:";

            
            story.state.LoadJson(input.storyStateJson); // get storystate from json

            output.storyStateJson = story.state.ToJson(); //put storystate into inkdata

            foreach (string item in story.state.variablesState)
            {
                message += "\n" + item + ": " + story.state.variablesState[item].ToString();
            }




            Debug.Log(message);
        }

        protected void PopulateSceneFromData(InkData input, ref InkData output) 
        {
            //Debug.Log("This is when the textpanel is set to the contents of inkdata: " + textPanel.text);
            ParseAudio(input.sceneState.activeMusic,GlobalAudioManager.AudioGroup.Music);
            ParseAudio(input.sceneState.activeAmbiance,GlobalAudioManager.AudioGroup.Ambiance);
            SetBackdrop(input.sceneState.background);
            SetSprites(input.sceneState.sprites);
            SetSprites(input.sceneState.sprites);
            Spd(input.sceneState.textSpeedMod);
            StartCoroutine(textProducer.FeedText(InkDataAsset.currentText));
            output.currentText = input.currentText;
            output.historyText = input.historyText;
        }

        #region STORY ADVANCEMENT
        /// <summary>
        /// Main function driving changes in what is being shown on screen based on the story.
        /// </summary>
        private void AdvanceStory()
        {
            if (!story.canContinue)
            {
                if (PresentButtons()) ///try to make buttons if any
                {

                }
                else
                {
                    OnInteractionEnd();
                }
            }
            else
            {
                StartCoroutine(Advance()); /// reset the waitmarkers, and prepare behaviour for at the end of the text.
            }
        }

        /// <summary>
        /// Remove the "next line" marker.
        /// Prepares behaviour for at the end: isplay a "next line" icon if story is continueable, create buttons if not
        /// </summary>
        /// <returns></returns>
        private IEnumerator Advance()
        {
            HandleAdvancement = false; /// prevent input while working
            PutDataIntoStash(); /// stash current scene state
            timeSinceAdvance = 0; /// reset timer for skip button
            yield return textProducer.ProduceTextOuter(); /// Run text generator (until next stop or choice point.)
            HandleAdvancement = true; /// activate continue marker or button
        }
        
        


        #region Choices
        public void RemoveOptions()/// Destroys all the buttons from choices
        {
            foreach (Button child in buttonAnchor.GetComponentsInChildren<Button>())
            {
                Destroy(child.gameObject);
            }
        }
        private bool PresentButtons()
        {
            if (story.canContinue) 
            {
                //Debug.Log("no choices detected at this point");
                return false;
            }
            else if (story.currentChoices.Count > 0) /// Display all the choices, if there are any!
            {
                //Debug.Log("Choices detected!");
                for (int i = 0; i < story.currentChoices.Count; i++)
                {
                    
                    Choice choice = story.currentChoices[i];
                    Button button = PresentButton(choice.text.Trim());
                    /// Tell the button what to do when we press it
                    button.onClick.AddListener(delegate {
                        OnClickChoiceButton(choice);
                    });
                }
                //scrollbar.value = 0;
                return true;
            }
            /// If we've read all the content and there's no choices, the story is finished!
            else
            {
                Button choice = PresentButton("End of story.");
                choice.onClick.AddListener(delegate {
                    RemoveOptions();
                    OnInteractionEnd();
                });
                return true;
            }
        }
        /// Creates a button showing the choice text
		Button PresentButton(string text)
        {
            Debug.Log("make button for " + text);
            /// Creates the button from a prefab
            Button choice = Instantiate(buttonPrefab) as Button;
            choice.transform.SetParent(buttonAnchor, false);

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
            AdvanceStory(); /// next bit
		}
        #endregion Choices

        
       
        protected void OnInteractionEnd()
        {
            story.RemoveVariableObserver();
            PutDataIntoStash();
            InkStoryAsset = null;
            story = null;
            Debug.Log(new NotImplementedException());
            // evt volgende story feeden
        }

        #endregion STORY ADVANCEMENT





        #endregion METHODS
    }
}