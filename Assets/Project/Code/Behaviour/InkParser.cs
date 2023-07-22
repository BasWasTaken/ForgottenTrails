using DataService;
using Ink.Runtime;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utility;

namespace ForgottenTrails
{
    /// <summary>
    /// <para>Behaviour for displaying ink text and allowing the player to input choices.</para>
    /// </summary>
    /// This is a behaviour script based on an example script of how to play and display a ink story in Unity. Further functionality has been canibalised from mso project. 
    /// Taken from the Inky demo on 2023-03-08, 14:45 and adapted by Bas Vegt.
    [RequireComponent(typeof(TextProducer))]
    public class InkParser : MonoSingleton<InkParser>
    {
        #region INSPECTOR VARIABLES	

        /// ASSETS
        [SerializeField, BoxGroup("Assets"), Required]
        [Tooltip("Here drag the INK object containing the dialogue behaviour")]
        private TextAsset inkJSONAsset = null;

        [SerializeField, BoxGroup("Assets")]
        [Tooltip("Data object containing INK data.")]
        private InkData inkData;

        [SerializeField, BoxGroup("Assets"), Required]
        private Button buttonPrefab = null;
        [SerializeField, BoxGroup("Assets"), Required]
        private Image portraitPrefab = null;

        /// SCENE REFERENCES
        [BoxGroup("Scene References"), Required]
        [Tooltip("The main canvas used for GUI elements in this scene.")]
        public Canvas canvas = null;

        /*
        [BoxGroup("Scene References"), Required]
        [Tooltip("The scrollbar used for the paper scroll.")]
        public Scrollbar scrollbar;
        [BoxGroup("Scene References"), Required]
        [Tooltip("The spacer used at bottom of the paper scroll.")]
        public LayoutElement spacer;
        */
        [BoxGroup("Scene References")]
        public Image bgImage;
        [BoxGroup("Scene References")]
        public HorizontalLayoutGroup portraits;
        [BoxGroup("Scene References"), Required]
        public Transform buttonAnchor;
        [BoxGroup("Scene References")]
        public Image floatingMarker;

        [SerializeField, BoxGroup("Scene References"), Required]
        [Tooltip("Here drag the component used for sfx.")]
        private AudioSource audioSourceSfx;
        [SerializeField, BoxGroup("Scene References"), Required]
        [Tooltip("Here drag the component used for voice.")]
        private AudioSource audioSourceVox;
        [SerializeField, BoxGroup("Scene References"), Required]
        [Tooltip("Here drag the component used for music.")]
        private AudioSource audioSourceMusic;
        [SerializeField, BoxGroup("Scene References"), Required]
        [Tooltip("Here drag the component used for ambiance.")]
        private AudioSource audioSourceAmbiance;
        [SerializeField, BoxGroup("Scene References"), Required]
        [Tooltip("Here drag the component used for system sounds like ui.")]
        private AudioSource audioSourceSystem;

        private TextProducer textProducer;


        [SerializeField, BoxGroup("Settings")]
        [Tooltip("Delay after which space button advances dialogue.")]
        protected float advanceDialogueDelay = .1f;
        [SerializeField, BoxGroup("Settings")]
        [Tooltip("Delay after which space button skips new dialogue.")]
        protected float skipDelay = .2f;
        public float AdvanceDialogueDelay => advanceDialogueDelay;

        private bool playing = false;



        public enum TextSpeed
        {
            slow = 50,
            medium = 100,
            fast = 200
        }

        private TextSpeed _textSpeedBase = TextSpeed.medium;
        private TextSpeed TextSpeedBase 
        { get 
            { return _textSpeedBase; } 
            set 
            {
                _textSpeedBase = value;
                PlayerPrefs.SetInt("textSpeed", (int)_textSpeedBase);
            }
        }
        public float TextSpeedActual => ((float)TextSpeedBase) *  inkData.sceneState.textSpeedMod;


        #endregion INSPECTOR VARIABLES

        #region INSPECTOR_HELPERS

        public Dictionary<VariablesState, object> inkVars = new();


        [SerializeField, Tooltip("Reset ink data in object. Note: does not remove data from file"), Button("ResetInkData",EButtonEnableMode.Editor)]
        public void ResetInkDataButton() { inkData = CreateBlankData(); PrepStory(); }
        [SerializeField, Button("LoadData")]
        public void LoadDataButton() {
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

            protected bool IsValidFolder(string path)
        {
            return Directory.Exists(path);
        }

        #endregion INSPECTOR_HELPERS


        #region BACKEND FIELDS
        private const string dataLabel = "BasicInkScript"; // but isn't this ridiculous? if you'll be using this object as ink interface all the time, it should't itelf store particular data, you should have objets etc store data... else all will be under here, won't it? or will it just be settings?
        public Story story;
        public static event Action<Story> OnCreateStory;

        private bool _receptiveForInput = false;

        private float timeSinceAdvance = 0;

        #endregion BACKEND FIELDS

        #region LIFESPAN	

        override protected void Awake()
        {
            base.Awake();
            Functions = new(this);
            textProducer = GetComponent<TextProducer>();

            //Debug.Log("This is when the textpanel was set to blank: " + textPanel.text);
            //spacer.minHeight = Camera.main.scaledPixelHeight;
//            scrollbar.value = 1;
            if (true) // why do i need to make blank data here always? is it just so that i don't get nullerrors later?
            {
                inkData = CreateBlankData(true);
            }
            TextSpeedBase = (TextSpeed)PlayerPrefs.GetInt("textSpeed", (int)_textSpeedBase);
        }
        private void Start()
        {
            transform.localPosition = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);
            if (inkJSONAsset != null)
            {
                PrepStory();
                StartStory();
            }
        }
        /// <summary>
        /// Preps story for play. Should be called after <see cref="InkData"/> object has been initialised or loaded.
        /// </summary>
        void PrepStory()
        {
            RemoveOptions();
            story = new Story(inkJSONAsset.text);
            story.state.variablesState["Name"] = DataManager.Instance.MetaData.playerName; // get name from metadata
            //Debug.Log(story.state.variablesState["Name"]);
            BindAndObserve(story);
            OnCreateStory?.Invoke(story);
        }
        /// Creates a new Story object with the compiled story which we can then play!
        void StartStory()
        {
            if (DataManager.Instance.DataAvailable(inkData.Key))
            {
                Debug.Log("found data! trying to load...");
                TryLoadData();
            }
            else
            {
                inkData = CreateBlankData(); // you're making data a second time, there's already blank data made.
            }
            if (!playing)
            {
                playing = true;
                if (inkData.storyStateJson != "")
                {
                    Debug.Log("continueing from savepoint!");
                    story.state.LoadJson(inkData.storyStateJson);
                    StartCoroutine(textProducer.FeedText(story.currentText));
                }
                else
                {
                    Debug.Log("no save point detected, starting from start");
                    story.state.GoToStart();
                }
                AdvanceStory(); /// show the first bit of story

            }
            else
            {
                Debug.LogError("Still playing according to bool!");
            }

        }
        void StopPlayingStory()
        {
            playing = false;
        }
        #endregion LIFESPAN

        #region LOOP

        private void Update()
        {
            if (!false)//if not pause
            {
                if(true)//if handling input
                {
                    switch (textProducer.State) /// check that state's input
                    {
                        // dit kan mooier met een machine state en interne update loops daarin., like state.update() (met daarin een .oninpout en .always ofzo).
                        case TextProducer.PState.Booting:
                            break;
                        case TextProducer.PState.Idle:

                            if (Input.GetKeyDown(KeyCode.Space))
                            {
                                if (story.canContinue)
                                {
                                    if (HandleAdvancement)
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
                                if (timeSinceAdvance > skipDelay)
                                {
                                    textProducer.SkipLines();
                                }
                            }
                            if (Input.GetKeyDown(KeyCode.Escape))
                            {
                                if (textProducer.Skipping) textProducer.ResetSkipping();
                            }
                            break;
                        case TextProducer.PState.Stuck:
                            break;
                        default:
                            break;
                    }
                }
                timeSinceAdvance += Time.unscaledDeltaTime; // note don't let overflow
            }
        }

        #endregion LOOP
        #region METHODS
        public bool Halted { get; private set; }
        public bool peeking => textProducer.peeking;
        #region INKY_EXTERNALS
        public InkFunctions Functions;
        public class InkFunctions
        {
            private InkParser parser;
            public InkFunctions() { }
            public InkFunctions(InkParser parser)
            {
                this.parser = parser;
            }
            public void StartFunctionMethod()
            {
                parser.Halted = true;
            }
            public void DoFunction(Action function)
            {
                if (parser.peeking) return;
                StartFunctionMethod();
                function();
                EndFunctionMethod();
            }
            public void EndFunctionMethod()
            {
                parser.Halted = false;
            }

        }

        
        private void BindAndObserve(Story story)
        {

            /* this was too slow
            story.ObserveVariable("spd", (variableName, newValue) =>
            {
                Spd(float.Parse(newValue.ToString()));
            });*/
            story.BindExternalFunction("Print", (string text) => Functions.DoFunction(() => ConsoleLogInk(text, false)));
            story.BindExternalFunction("PrintWarning", (string text) => Functions.DoFunction(() => ConsoleLogInk(text, true)));
            story.BindExternalFunction("Spd", (float mod) => Functions.DoFunction(() => Spd(mod / 100)));
            story.BindExternalFunction("Clear", () => Functions.DoFunction(() => textProducer.ClearPage()));
            story.BindExternalFunction("Halt", (float dur) => Functions.DoFunction(() => StartCoroutine(HaltText(dur))));
            story.BindExternalFunction("Bg", (string fileName) => Functions.DoFunction(() => SetBackdrop(fileName)));
            story.BindExternalFunction("Sprites", (string fileNames) => Functions.DoFunction(() => SetSprites(fileNames)));
            story.BindExternalFunction("Vox", (string fileName, float relVol) => Functions.DoFunction(() => ParseAudio(fileName,AudioManager.AudioGroup.Voice, relVol)));
            story.BindExternalFunction("Sfx", (string fileName, float relVol) => Functions.DoFunction(() => ParseAudio(fileName, AudioManager.AudioGroup.Sfx, relVol)));
            story.BindExternalFunction("Ambiance", (string fileName, float relVol) => Functions.DoFunction(() => ParseAudio(fileName, AudioManager.AudioGroup.Ambiance,relVol)));
            story.BindExternalFunction("Music", (string fileName, float relVol) => Functions.DoFunction(() => ParseAudio(fileName, AudioManager.AudioGroup.Music, relVol)));
        }
        /*
        public static class InkFunctionsStatic
        {
            //public delegate void StartFunctionAction();
            //public static event StartFunctionAction StartFunctionEvent;

            //public delegate void EndFunctionAction();
            //public static event EndFunctionAction EndFunctionEvent;

            public static void StartFunctionMethod()
            {
                InkParser.Halted = true;
            }
            public static void DoFunction(Action function)
            {

                if (peeking) return;
                //StartFunctionEvent?.Invoke();
                StartFunctionMethod();
                function();
                EndFunctionMethod();
                //EndFunctionEvent?.Invoke();
            }
            public static void EndFunctionMethod()
            {
                InkParser.Halted = false;
            }

        }
        */
       
        //public Action InkFunction;

        private void Spd(float speed)
        {
            inkData.sceneState.textSpeedMod = speed;
        }

        public IEnumerator HaltText(float seconds)
        {
            Halted = true;
            yield return new WaitForSecondsRealtime(seconds);
            Halted = false;
        }

        private void SetBackdrop(string fileName)
        {
            if (peeking) return;
            Sprite sprite = null; /// clear bg if no other value is given
            if (!(fileName == "" | fileName == "null"))
            {
                try
                {
                    if (!AssetManager.Instance.Sprites.TryGetValue(fileName, out Sprite sprite1))
                    {
                        throw new FileNotFoundException("File not found: " + fileName);
                    }
                    else
                    {
                        sprite = sprite1;
                    }
                }
                catch (Exception e)
                {
                    // Extract some information from this exception, and then
                    // throw it to the parent method.
                    if (e.Source != null)
                        Console.WriteLine("IOException source: {0}", e.Source);
                    throw;
                }
            }
            bgImage.sprite = sprite;
            inkData.sceneState.background = fileName;
        }

        private void SetSprites(string fileNames)
        {
            if (peeking) return;
            /// first clear all portraits
            foreach (Image item in portraits.GetComponentsInChildren<Image>())
            {
                Destroy(item.gameObject);
            }

            string[] fileNamesSplit = fileNames.Split(',');

            foreach (string fileName0 in fileNamesSplit)
            {
                Sprite sprite = null; /// clear if no other value is given

                string fileName = fileName0.ToLower().Trim(' '); // trim spaces
                if (!(fileName == "" | fileName == "null"))
                {
                    try
                    {
                        if (!AssetManager.Instance.Sprites.TryGetValue(fileName, out Sprite sprite1))
                        {
                            Debug.LogError(new FileNotFoundException("File not found: " + fileName));
                        }
                        else
                        {
                            sprite = sprite1;
                        }
                    }
                    catch (Exception e)
                    {
                        // Extract some information from this exception, and then
                        // throw it to the parent method.
                        if (e.Source != null)
                            Console.WriteLine("IOException source: {0}", e.Source);
                        throw;
                    }
                }

                Instantiate(portraitPrefab, portraits.transform).sprite=sprite;
                inkData.sceneState.sprites += ", " + fileName;
            }
        }

        #region AUDIO
        private void ParseAudio(string fileName, AudioManager.AudioGroup audioGroup, float relVol =.5f)
        {
            if (peeking) return;
            AudioClip audioClip = null; /// clear audio if no other value is given
            if (!(fileName == "" | fileName == "null"))
            {
                try
                {
                    if (!AssetManager.Instance.AudioClips.TryGetValue(fileName, out AudioClip audioClip1))
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
                    throw;
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
            if(audioGroup == AudioManager.AudioGroup.Music)
            {
                //oneShot = false;
                loop = true;
                inkData.sceneState.activeMusic = fileName;
            }
            else if (audioGroup == AudioManager.AudioGroup.Ambiance)
            {
                //oneShot = false;
                loop = true;
                inkData.sceneState.activeAmbiance = fileName;
            }
            PlayAudio(audioClip, audioGroup, relVol, oneShot: oneShot,loop:loop);
        }
        private void PlayAudio(AudioClip audioClip, AudioManager.AudioGroup audioGroup, float relVol = .5f, bool oneShot = false, bool loop = false)
        {
            AudioSource audioSource = audioGroup switch
            {
                AudioManager.AudioGroup.Sfx => audioSourceSfx,
                AudioManager.AudioGroup.Ambiance => audioSourceAmbiance,
                AudioManager.AudioGroup.Music => audioSourceMusic,
                AudioManager.AudioGroup.Voice => audioSourceVox,
                AudioManager.AudioGroup.System => audioSourceSystem,
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
        #endregion AUDIO

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

        /*Depricated: tags no longer used.
        private void DoFunction(string tag)
        {
            string[] split = tag.ToLower().Split(':');
            string command = split[0];
            string parameter = split[1].TrimStart(' ');

            if (command == "backdrop")
            {
                SetBackdrop(parameter);
            }
            if (command == "sprites")
            {
                SetSprites(parameter);
            }
            else if (command == "music")
            {
                SetMusic(parameter);
            }
            else if (command == "ambiance")
            {
                SetAmbiance(parameter);
            }
            else if (command == "sfx")
            {
                PlaySfx(parameter);
            }
            else if (command == "pause")
            {
                HaltTextFor(parameter);
                Debug.LogAssertion("Please Note: I am still working my way up to handling timing well in unity stories. The parsing of text by the computer is near-instantaneous, but it takes a while for the text to be presented on screen, particularly using our typewriter effect. I haven't yet thought of how to synch up any function to when it appears in the text. Timing related functions might not (yet) work properly or have the desired effect.");
            }
        }
        */

        #endregion InkyExternals

        #region Data
        /// <summary>
        /// Make a new inkdata object
        /// </summary>
        /// <returns>The freshly made blank data</returns>
        public InkData CreateBlankData(bool forBootup=false)
        {
            InkData data = new(dataLabel + "DemoScene");
            if(!forBootup)Debug.Log("Created new data " + data.Label);
            return data;
        }

        /// <summary>
        /// preps data for saving. happens at the end of every bit of dialogue.
        /// </summary>
        public void PutDataIntoStash() // this should then be called every so often and whenever the save button is pressed
        {
            /// save all the things 
            inkData.currentText = textProducer.CurrentText; // is deze nodig? is dat niet contained in story?
            inkData.historyText = textProducer.PreviouslyDisplayed; 
            inkData.storyStateJson = story.state.ToJson();
            inkData.StashData();
        }
        public void SaveData()
        {
            PutDataIntoStash();
            DataManager.Instance.WriteStashedDataToDisk();
        }
        public bool TryLoadData()
        {
            return TryLoadData(ref inkData);
        }
        private bool TryLoadData(ref InkData output) 
        {
            if (!DataManager.Instance.DataAvailable(output.Key))
            {
                Debug.LogError("Error code 404: No data found available.");
                return false;
            }
            else
            {
                InkData input = DataManager.Instance.FetchData<InkData>(inkData.Key);
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
                    catch (Exception)
                    {
                        throw;
                    }
                    Debug.Log("Successfully loaded data!");
                    return true;
                }
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
            ParseAudio(input.sceneState.activeMusic,AudioManager.AudioGroup.Music);
            ParseAudio(input.sceneState.activeAmbiance,AudioManager.AudioGroup.Ambiance);
            SetBackdrop(input.sceneState.background);
            SetSprites(input.sceneState.sprites);
            SetSprites(input.sceneState.sprites);
            Spd(input.sceneState.textSpeedMod);
            StartCoroutine(textProducer.FeedText(inkData.currentText));
            output.currentText = input.currentText;
            output.historyText = input.historyText;
        }
        #endregion DATA

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
        public bool HandleAdvancement
        {
            get { return _receptiveForInput; }
            protected set
            {
                _receptiveForInput = value; 
                if (_receptiveForInput)
                {
                   // Debug.Log("Turning input on.");
                    if (PresentButtons()) ///try to make buttons if any
                    {

                    }
                    else
                    {
                        floatingMarker.gameObject.SetActive(true); /// else set bouncing triangle at most recent line
                    }
                }
                else
                {

//                    Debug.Log("Turning input off.");
                    RemoveOptions(); /// Destroy old choices
                    floatingMarker.gameObject.SetActive(false); /// remove marker 

                }
            }
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
            inkData.storyStateJson = story.state.ToJson(); /// record the story state
            AdvanceStory(); /// next bit
		}
        #endregion Choices

        
       
        protected void OnInteractionEnd()
        {
            story.RemoveVariableObserver();
            PutDataIntoStash();
            inkJSONAsset = null;
            story = null;
            Debug.Log(new NotImplementedException());
            // evt volgende story feeden
        }

        #endregion STORY ADVANCEMENT





        #endregion METHODS
    }
    #region PERIFEAL
    [Serializable]
    public class InkData : DataClass
    {
        public InkData(string label) : base(label)
        {
        }


        public string storyStateJson = ""; /// string indicating most recently saved state of the ink object.
        public SceneState sceneState = new();
        public string currentText = "";
        public string historyText = "";
    }

    [Serializable]
    public class SceneState
    {

        public string text = "null";
        public string history = "null";

        public string background = "null";
        public string sprites = "null";


        public string activeMusic = "null";
        public string activeAmbiance = "null";

        public float textSpeedMod = 1;

    }


    #endregion
}