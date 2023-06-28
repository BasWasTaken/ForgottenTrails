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

namespace Core
{

    /// <summary>
    /// <para>Behaviour for displaying ink text and allowing the player to input choices.</para>
    /// </summary>
    /// This is a behaviour script based on an example script of how to play and display a ink story in Unity. Further functionality has been canibalised from mso project. 
    /// Taken from the Inky demo on 2023-03-08, 14:45 and adapted by Bas Vegt.
    public class BasicInkScript : MonoSingleton<BasicInkScript>
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
        [SerializeField, BoxGroup("Scene References"), Required]
        [Tooltip("The main canvas used for GUI elements in this scene.")]
        private Canvas canvas = null;

        [SerializeField, BoxGroup("Scene References"), Required]
        [Tooltip("The scrollbar used for the paper scroll.")]
        public Scrollbar scrollbar;
        [SerializeField, BoxGroup("Scene References"), Required]
        [Tooltip("The spacer used at bottom of the paper scroll.")]
        public LayoutElement spacer;


        [SerializeField, BoxGroup("Scene References"), Required]
        [Tooltip("Panel to display current paragraph.")]
        public TextMeshProUGUI textPanel = null;
        [SerializeField, BoxGroup("Scene References")]
        public Image bgImage;
        [SerializeField, BoxGroup("Scene References")]
        public HorizontalLayoutGroup portraits;
        [SerializeField, BoxGroup("Scene References"), Required]
        public Transform buttonAnchor;
        [SerializeField, BoxGroup("Scene References")]
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




        //[SerializeField, BoxGroup("Settings")]
        [Tooltip("Delay after which space button advances dialogue.")]
        protected float advanceDialogueDelay = .1f;
        public float AdvanceDialogueDelay => advanceDialogueDelay;

        private bool halted = false;

        private bool playing = false;

        [SerializeField, BoxGroup("Settings"), Foldout("TextSpeed")]
        [Tooltip("Choose a numeric value for this option.")]
        private float slowText = 10f;
        [SerializeField, BoxGroup("Settings"), Foldout("TextSpeed")]
        [Tooltip("Choose a numeric value for this option.")]
        private float mediumText = 50f;
        [SerializeField, BoxGroup("Settings"), Foldout("TextSpeed")]
        [Tooltip("Choose a numeric value for this option.")]
        private float fastText = 200f;
        [BoxGroup("Settings")]
        [Tooltip("Rate of typewriter effect in nr of characters per second.")]
        public float textSpeed = 50;

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

        public bool CanAdvance { get; protected set; }
        public bool CompletedText => textPanel.maxVisibleCharacters == textPanel.text.Length;
        private bool completeText = false;
        private float timeSinceAdvance = 0;

        #endregion BACKEND FIELDS

        #region LIFESPAN	

        override protected void Awake()
        {
            base.Awake();
            textPanel.text = ""; //clear lorum ipsum
            //Debug.Log("This is when the textpanel was set to blank: " + textPanel.text);
            CanAdvance = false;
            //spacer.minHeight = Camera.main.scaledPixelHeight;
            scrollbar.value = 1;
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

            story.BindExternalFunction("Print", (string text) => ConsoleLogInk(text, false));
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
                    StartCoroutine(DisplayContent(story.currentText));
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
            completeText = true;
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
                    completeText = true;
                }
                else if (story.canContinue & CanAdvance & CompletedText)
                {
                    AdvanceStory();
                }
            }
            timeSinceAdvance += Time.unscaledDeltaTime; // note don't let overflow
        }

        #endregion LOOP

        #region METHODS


        #region INKY_EXTERNALS

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

        IEnumerator HaltText(int seconds)
        {
            halted = true;
            yield return new WaitForSecondsRealtime(seconds);
            halted = false;
        }

        void HaltTextFor(string time)
        {
            time = time.Trim('s');
            Int32.TryParse(time, out int seconds);
            StartCoroutine(HaltText(seconds));
        }


        private void Spd(string speed)
        {
            char first = speed[0];
            if (first == 'S' | first == 's')
            {
                textSpeed = slowText;
            }
            else if (first == 'M' | first == 'm')
            {
                textSpeed = mediumText;
            }
            else if (first == 'F' | first == 'f')
            {
                textSpeed = fastText;
            }
            else
            {
                Debug.LogError("INKY Error: Speed value \"" + speed + "\" not recognised. Please supply either the letter \'S\' for slow, \'M\' for medium, or \'F\' for fast. Speed was not changed.");
                return;
            }

            if (speed.Length > 1)
            {
                Debug.LogWarning(string.Format("Note: You requested the textSpeed be changed to {0}, which was recognised as {1} and the applied accordingly. However, you only need to supply either the letter \'S\' for slow, \'M\' for medium, or \'F\' for fast. Please address this in your INK Story.", speed, first));
            }
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
        private void SetMusic(string fileName)
        {
            AudioClip audioClip = null; /// clear music if no other value is given
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
            if (audioClip != null)
            {
                if (audioSourceMusic.clip != audioClip)
                {
                    audioSourceMusic.clip = audioClip;
                    inkData.sceneState.activeMusic = fileName;
                    audioSourceMusic.Play();
                }
            }
            else
            {
                audioSourceMusic.clip = audioClip;
                audioSourceMusic.Stop();
                inkData.sceneState.activeMusic = "";
            }
        }
        private void SetAmbiance(string fileName)
        {
            AudioClip audioClip = null; /// clear music if no other value is given
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
            if (audioClip != null)
            {
                if (audioSourceAmbiance.clip != audioClip)
                {
                    audioSourceAmbiance.clip = audioClip;
                    inkData.sceneState.activeAmbiance = fileName;
                    audioSourceAmbiance.Play();
                }
            }
            else
            {
                audioSourceAmbiance.Stop();
                inkData.sceneState.activeAmbiance = "";
            }
        }

        private void PlaySfx(string fileName)
        {
            AudioClip audioClip = null; /// clear music if no other value is given
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
            if (audioClip != null)
            {
                if (true)//audioSourceSfx.clip != audioClip)
                {
                    audioSourceSfx.clip = audioClip;
                    audioSourceSfx.PlayOneShot(audioClip);
                    StartCoroutine(RemoveClipWhenFinished(audioSourceSfx));
                }
            }
        }
        IEnumerator RemoveClipWhenFinished(AudioSource audioSource)
        {
            yield return new WaitWhile(() => audioSource.isPlaying);
            audioSource.clip = null;
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


            story.state.LoadJson(input.storyStateJson);
            output.storyStateJson = story.state.ToJson();

            foreach (string item in story.state.variablesState)
            {
                message += "\n" + item + ": " + story.state.variablesState[item].ToString();
            }

            Debug.Log(message);
        }

        protected void PopulateSceneFromData(InkData input, ref InkData output) 
        {
            Spd("M");

            textPanel.text = inkData.textLog;
            output.textLog = input.textLog;
            //Debug.Log("This is when the textpanel is set to the contents of inkdata: " + textPanel.text);
            SetMusic(input.sceneState.activeMusic);
            SetAmbiance(input.sceneState.activeAmbiance);
            SetBackdrop(input.sceneState.background);
            SetSprites(input.sceneState.sprites);
        }
        #endregion DATA




        #region STORY ADVANCEMENT
        /// This is the main function called every time the story changes. It does a few things:
        /// Destroys all the old content and choices.
        /// Continues over all the lines of text, then displays all the choices. If there are no choices, the story is finished!
        /// NOTE this adapted version goes over one or a few lines at a time, rather than displaying all at once. It also renders lines per letter. 
        private void AdvanceStory()
        {
            /// Remove all the UI options on screen
            RemoveOptions();

            /// reset the waitmarkers, and prepare behaviour for at the end of the text: 
            /// display a "next line" icon if story is continueable, create buttons if not
            StartCoroutine(MarkWhenAdvanceable());

            /// Assemble the next paragraph
            string text = AssembleParagraph();


            /// stash the new state
            PutDataIntoStash(); 

            ///  display the text on screen!          
            StartCoroutine(DisplayContent(text));

        }

        private string AssembleParagraph()
        {
            string text = "";
            while (story.canContinue) /// at most until the story hits a choice
            {
                string newLine = story.Continue(); ///Continue gets the next line of the story 
                if (newLine.StartsWith("..."))
                {
                    inkData.textLog = inkData.textLog.Trim('\n') + ' ';

                    text += newLine.TrimStart('.');
                }
                else
                {
                    /* removed speaker option
                    if (new Regex(".+:\\s\\w+").IsMatch(newLine)) /// (example) check if this line is being spoken my anybody specific
                    {
                        string[] split = newLine.Split(':');
                        if (split.Length > 2)
                        {
                            throw new ArgumentException("Cannot handle two ':'s in 1 line.");
                        }
                        newLine = "\"" + split[1] + "\"";
                        string speaker = split[0];
                    }
                    */

                    text += newLine; /// add the newline of the story
                    
                }

                /// check for tags:
                foreach (string tag in story.currentTags)
                {
                    DoFunction(tag);
                }



                /// stop if you hit a stop tag:
                if (text.EndsWith("\n<stop>\n"))
                {
                    //text = text.TrimEnd("<br>\n".ToCharArray()); 
                    text = "__________\n" + text;
                    break;
                }
            }
            return text;
        }

        public void RemoveOptions()/// Destroys all the buttons from choices
        {
            foreach (Button child in buttonAnchor.GetComponentsInChildren<Button>())
            {
                Destroy(child.gameObject);
            }
        }

        private IEnumerator MarkWhenAdvanceable()
        {
            floatingMarker.gameObject.SetActive(false);
            CanAdvance = false;
            yield return new WaitUntil(() => story.canContinue);
            yield return new WaitForSecondsRealtime(advanceDialogueDelay);
            yield return new WaitUntil(() => CompletedText);
            if (!PresentButtons()) ///try to make buttons if any
            {
                /// else set bouncing triangle at most recent line
                floatingMarker.gameObject.SetActive(true);
            }
            CanAdvance = true;
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
        void OnClickChoiceButton(Choice choice)
        {
            story.ChooseChoiceIndex(choice.index); /// feed the choice
            inkData.storyStateJson = story.state.ToJson(); /// record the story state
            AdvanceStory(); /// next bit
		}


        public IEnumerator DisplayContent(string newText) // Creates a textbox showing the the poaragraph of text
        {
            /* REMOVE
            if (glueLater.Length > 0)
            {
                Debug.Log("glued");
                inkData.textLog = inkData.textLog.TrimEnd('\n');
                glueLater = "";
            }
            Debug.Log("nothing to be glued");
            */

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
                yield return new WaitForSecondsRealtime(1 / textSpeed);
                yield return new WaitWhile(() => halted);
                yield return new WaitUntil(() => isActiveAndEnabled);
                if (completeText)
                {
                    textPanel.maxVisibleCharacters = textPanel.text.Length;
                    completeText = false;
                    yield break;
                }
            }

        }

        /// When we click the choice button, tell the story to choose that choice!


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

    }


    #endregion
}