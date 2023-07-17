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



        public TextMeshProUGUI textPanel = null;
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
        [Tooltip("Here drag the component used for music.")]
        private AudioSource audioSourceMusic;
        [SerializeField, BoxGroup("Scene References"), Required]
        [Tooltip("Here drag the component used for ambiance.")]
        private AudioSource audioSourceAmbiance;
        [SerializeField, BoxGroup("Scene References"), Required]
        [Tooltip("Here drag the component used for system sounds like ui.")]
        private AudioSource audioSourceSystem;

        private TextProducer textProducer;


        //[SerializeField, BoxGroup("Settings")]
        [Tooltip("Delay after which space button advances dialogue.")]
        protected float advanceDialogueDelay = .1f;
        public float AdvanceDialogueDelay => advanceDialogueDelay;

        private bool halted = false;

        private bool playing = false;

        public enum TextSpeed
        {
            slow = 10,
            medium = 50,
            fast = 200
        }

        public TextSpeed textSpeedBase = TextSpeed.medium;

        public float TextSpeedActual => ((float)textSpeedBase) * inkData.sceneState.textSpeedMod;

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

        public bool ReceptiveForInput { get; protected set; }
        public bool CompletedText => textPanel.maxVisibleCharacters == textPanel.text.Length; // this might be dangerous, easy to mess up by adding cahracters later
        private bool skip = false;
        private float timeSinceAdvance = 0;

        #endregion BACKEND FIELDS

        #region LIFESPAN	

        override protected void Awake()
        {
            base.Awake();
            textProducer = GetComponent<TextProducer>();

            textPanel.text = ""; //clear lorum ipsum
            //Debug.Log("This is when the textpanel was set to blank: " + textPanel.text);
            ReceptiveForInput = false;
            //spacer.minHeight = Camera.main.scaledPixelHeight;
//            scrollbar.value = 1;
            if (true) // why do i need to make blank data here always? is it just so that i don't get nullerrors later?
            {
                inkData = CreateBlankData();
            }
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
            Debug.Log(story.state.variablesState["Name"]);
            BindExternalFunctions(story);
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
                    textProducer.FeedText(story.currentText);
                    PresentButtons();
                }
                else
                {
                    Debug.Log("no save point detected, starting from start");
                    story.state.GoToStart();
                }
                if (story.canContinue) AdvanceStory(); /// show the first bit of story

            }
            else
            {
                Debug.LogError("Still playing according to bool!");
            }

        }
        void StopPlayingStory()
        {
            skip = true;
            playing = false;
        }
        #endregion LIFESPAN

        #region LOOP

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (CompletedText == false & timeSinceAdvance > advanceDialogueDelay)
                {
                    skip = true;
                }
                else if (story.canContinue & ReceptiveForInput & CompletedText)
                {
                    AdvanceStory();
                }
            }
            timeSinceAdvance += Time.unscaledDeltaTime; // note don't let overflow
        }

        #endregion LOOP

        #region METHODS

        #region INKY_EXTERNALS
        private void BindExternalFunctions(Story story)
        {
            story.BindExternalFunction("Print", (string text) => ConsoleLogInk(text, false));
            story.BindExternalFunction("PrintWarning", (string text) => ConsoleLogInk(text, true));
            story.BindExternalFunction("Spd", (float mod) => Spd(mod));
            story.BindExternalFunction("Halt", (float dur) => StartCoroutine(HaltText(dur)));
            story.BindExternalFunction("Bg", (string fileName) => SetBackdrop(fileName));
            story.BindExternalFunction("Sprite", (string fileNames) => SetSprites(fileNames));
            story.BindExternalFunction("Voice", (string fileName, float relVol) => ParseAudio(fileName,AudioManager.AudioGroup.Voice, relVol));
            story.BindExternalFunction("Sfx", (string fileName, float relVol) => ParseAudio(fileName, AudioManager.AudioGroup.Sfx, relVol));
            story.BindExternalFunction("Ambiance", (string fileName, float relVol) => ParseAudio(fileName, AudioManager.AudioGroup.Ambiance,relVol));
            story.BindExternalFunction("Music", (string fileName, float relVol) => ParseAudio(fileName, AudioManager.AudioGroup.Music, relVol));
        }

        private void Spd(float speed)
        {
            inkData.sceneState.textSpeedMod = speed;
        }

        IEnumerator HaltText(float seconds)
        {
            halted = true;
            yield return new WaitForSecondsRealtime(seconds);
            halted = false;
        }

        private void SetBackdrop(string fileName)
        {
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
            /*
            AudioSource audioSource = audioGroup switch
            {
                AudioManager.AudioGroup.Sfx => audioSourceSfx,
                AudioManager.AudioGroup.Ambiance => audioSourceAmbiance,
                AudioManager.AudioGroup.Music => audioSourceMusic,
                AudioManager.AudioGroup.Voice => AudioManager.Instance.GlobalAudio(AudioManager.AudioGroup.Voice),
                AudioManager.AudioGroup.System => audioSourceSystem,
                _ => audioSourceSystem,
            };
            */
            AudioSource audioSource = AudioManager.Instance.GlobalAudio(audioGroup); /// get correct audiosource
            
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
                yield return new WaitForSeconds(t);
                audioSource.volume = Mathf.MoveTowards(audioSource.volume, relVol, d);
            }
        }

        IEnumerator RemoveClipWhenFinished(AudioSource audioSource)
        {
            yield return new WaitWhile(() => audioSource.isPlaying);
            audioSource.clip = null;
        }
        #endregion AUDIO

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
        /// <returns>The freshly maked blank data</returns>
        public InkData CreateBlankData()
        {
            InkData data = new(dataLabel + "DemoScene");
            Debug.Log("Created new data " + data.Label);
            return data;
        }

        /// <summary>
        /// preps data for saving. happens at the end of every bit of dialogue.
        /// </summary>
        public void PutDataIntoStash() // this should then be called every so often and whenever the save button is pressed
        {
            /// save all the things //(scene and text does't need to be stashed, is always stashed ad hoc)
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
            textPanel.text = inkData.textLog;
            output.textLog = input.textLog;
            //Debug.Log("This is when the textpanel is set to the contents of inkdata: " + textPanel.text);
            ParseAudio(input.sceneState.activeMusic,AudioManager.AudioGroup.Music);
            ParseAudio(input.sceneState.activeAmbiance,AudioManager.AudioGroup.Ambiance);
            SetBackdrop(input.sceneState.background);
            SetSprites(input.sceneState.sprites);
            SetSprites(input.sceneState.sprites);
            Spd(input.sceneState.textSpeedMod);
        }
        #endregion DATA




        #region STORY ADVANCEMENT

        /* DEPRICATED, MOVED TO TEXTPRODUCER
        public IEnumerator DisplayContent(string newText) // Creates a textbox showing the the poaragraph of text
        {

            timeSinceAdvance = 0; // reset timer for skip button
            int i0 = inkData.textLog.Length; // set startpoint for forloop
            inkData.textLog += newText; // add the nex text
            textPanel.text = inkData.textLog; //set the textpanel to whatever the inkdata has.
                                              // (okay, voordeel van het zo doen: inkdata and textpanel are always the same, dat is fijner met playtesting for visibility
                                              // but, this seems like a possibly bad idea in actual build, since any issue caused is immediately on your stash. although, not yet on data actually saved to disk, so that might be alright.
                                              // we can later add a buffer between stash and disk and also have redundancy with autosaves etc 

            //Debug.Log("This is when the textpanel was set to the contents of the newly updated indata: " + textPanel.text);
            //scrollbar.value = 0;

            for (int i = i0; i < textPanel.text.Length + 1; i++)
            {
                textPanel.maxVisibleCharacters = i;
                yield return new WaitForSecondsRealtime(1 / TextSpeedActual);
                yield return new WaitWhile(() => halted);
                yield return new WaitUntil(() => isActiveAndEnabled);
                if (skip)
                {
                    textPanel.maxVisibleCharacters = textPanel.text.Length;
                    skip = false;
                    yield break;
                }
            }

        }
        */
        /// <summary>
        /// Main function driving changes in what is being shown on screen based on the story.
        /// </summary>
        private void AdvanceStory()
        {
            if (!story.canContinue) OnInteractionEnd();
            else
            {
                RemoveOptions(); /// Destroy old choices
                StartCoroutine(MarkWhenAdvanceable()); /// reset the waitmarkers, and prepare behaviour for at the end of the text.
                StartCoroutine(ProduceText()); /// Run text generator (until next stop or choice point.)
            }
        }
        /// <summary>
        /// Remove the "next line" marker.
        /// Prepares behaviour for at the end: isplay a "next line" icon if story is continueable, create buttons if not
        /// </summary>
        /// <returns></returns>
        private IEnumerator MarkWhenAdvanceable()
        {
            floatingMarker.gameObject.SetActive(false);
            ReceptiveForInput = false; /// prevent input while working

            yield return new WaitUntil(() => story.canContinue); /// when this is being called, the story technically canContinue because we haven't continued it.
            yield return new WaitUntil(() => !textProducer.DoneAndReady); /// first wait until the textproducer has actually started producing text
            yield return new WaitUntil(() => textProducer.DoneAndReady); /// wait until current line has finished production
            yield return new WaitForSecondsRealtime(advanceDialogueDelay); /// wait for a little bit of extra time to account for human reaction time

            if (!PresentButtons()) ///try to make buttons if any
            {
                floatingMarker.gameObject.SetActive(true); /// else set bouncing triangle at most recent line
            }
            ReceptiveForInput = true; /// allow input again
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
            if (story.canContinue) return false;
            else if (story.currentChoices.Count > 0) /// Display all the choices, if there are any!
            {
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

        /// <summary>
        /// Runs the per line steps required for parsing and showing ink story
        /// </summary>
        private IEnumerator ProduceText()
        {
            do
            {
                PutDataIntoStash(); /// stash current scene state
                textProducer.FeedText(story.Continue());   /// Parse the ink story for functions and text: run functions and display text
                yield return new WaitUntil(() => textProducer.DoneAndReady);
            } while (story.canContinue);
        }


       
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
        public string textLog = "";
    }

    [Serializable]
    public class SceneState
    {

        public string text = "null";

        public string background = "null";
        public string sprites = "null";


        public string activeMusic = "null";
        public string activeAmbiance = "null";

        public float textSpeedMod = 1;

    }


    #endregion
}