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




        [SerializeField, BoxGroup("Settings")]
        [Tooltip("Delay after which space button advances dialogue.")]
        protected float advanceDialogueDelay = .1f;
        public float AdvanceDialogueDelay => advanceDialogueDelay;

        private bool halted = false;

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

        [SerializeField, Button("ResetInkData",EButtonEnableMode.Editor)]
        public void ResetInkDataButton() { inkData = CreateBlankData(); }
        [SerializeField, Button("LoadData")]
        public void LoadDataButton() { 
            TryLoadData();
#if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlaying == true)
            {
                StartLoadedStory();
                Debug.Log(new NotImplementedException("You should reset the scene here."));
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
            CanAdvance = false;
            
            if (true) // if no data present..?
            {
                inkData = CreateBlankData();
            }
        }
        private void Start()
        {
            transform.localPosition = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);
            if (inkJSONAsset != null)
            {
                StartStory();
            }
        }

        /// Creates a new Story object with the compiled story which we can then play!
        void StartStory()
        {
            story = new Story(inkJSONAsset.text);

            story.BindExternalFunction("Print", (string text) => ConsoleLogInk(text, false));
            OnCreateStory?.Invoke(story);

            TryLoadData();

            StartLoadedStory();
		}
        void StartLoadedStory()
        {
            if (inkData.saveStateCur != "")
            {
                Debug.Log("continueing from savepoint!");
                story.state.LoadJson(inkData.saveStateCur);

                    Debug.Log(new NotImplementedException("You activate the ink interface here"));
            }
            else
            {
                Debug.Log("no save point detected, starting from start");
                story.state.GoToStart();
            }


            if (story.canContinue) StartCoroutine(AdvanceStory()); /// show the first bit of story
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
                    StartCoroutine(AdvanceStory());
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

            foreach (string fileName in fileNamesSplit)
            {
                Sprite sprite = null; /// clear bg if no other value is given

                if (!(fileName == "" | fileName == "null"))
                {
                    try
                    {
                        if (!AssetManager.Instance.Sprites.TryGetValue(fileName.ToLower().Trim(' '), out Sprite sprite1))
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
        public InkData CreateBlankData()
        {
            InkData data;

            data = new(dataLabel + "DemoScene");
            // some sort of init here
            return data;
        }

        /// <summary>
        /// preps data for saving. happens at the end of every bit of dialogue.
        /// </summary>
        public void PutDataIntoStash() // this should then be called every so often and whenever the save button is pressed
        {
            ObserveNewVariables(); // adds any new variables that exist in the story to our list and start keeping track.
            /// save all the things

            inkData.saveStateCur = story.state.ToJson();
            inkData.StashData();
        }
        private bool TryLoadData()
        {
            if (DataManager.Instance.DataAvailable(inkData.Key))
            {
                inkData = LoadData(DataManager.Instance.FetchData<InkData>(inkData.Key));
                return true;
            }
            else { Debug.Log("No data found."); return false; }

        }
        private InkData LoadData(InkData input)
        {
            //Debug.Log("gonna try to load data!");
            if (inkData != input)
            {
                //Debug.Log("confirmed data is not identical");
                //Debug.Log("now deleting own data to avoid confusion");
                inkData = CreateBlankData();
                //    Debug.Log("wil now load the variables into ink");
                LoadVarsIntoInk(ref input);
                //    Debug.Log("will now load all the objects!");
                LoadObjectsIntoScene(input);

                //      Debug.Log("will now assign inkdata");

                return input;
            }
            else
            {
                Debug.Log("ah, no, that's just this data. gonna remove it from cache for testing");

                DataManager.Instance.RemoveFromCache<InkData>(inkData.Key);
                return inkData;
            }
        }

        protected void LoadVarsIntoInk(ref InkData newData)
        {
            string message = "InkVars:";
            foreach (var item in newData.inkVars)
            {
                story.variablesState[item.Key] = item.Value;
                message += "\n" + item.Key + ": " + item.Value.ToString();
            }
            Debug.Log(message);
            newData.inkVars.Clear();
            newData.inkVarsKeys.Clear();
            newData.inkVarsValues.Clear();
        }

        /// <summary>
        /// Purpose: to synch variables between dataclass and ink. get all of the data that is on ink's side in my datafile.
        /// Any data that may be on my file, but not on ink's  side, that's useless. it hasn't gotten there yet apperantly and when it will it will be overridden.
        ///  So this should just happen unilaterally: clear inkdata's variables and observe all of inky's.
        /// Should be started whenever data is attempted to be added to cash, so that that data is always up to date.
        /// also probably when loading data in.
        /// </summary>
        protected void ObserveNewVariables()
        {
            /// make an empty list
            List<string> newVariables = new();

            /// fill it with all the variables that we have not yet collected:
            foreach (var variable in story.variablesState)
            {/// get all variables
                if (!inkData.inkVars.ContainsKey(variable))
                {/// check all new ones
                    newVariables.Add(variable); /// add it to our list
                }
            }

            foreach (var newVariable in newVariables)
            {/// for each of our new variables:
                inkData.inkVars.Add(newVariable, story.variablesState[newVariable]); /// add it to our dataclass
                inkData.inkVarsKeys.Add(newVariable); /// and at it to the cheatsheets
                inkData.inkVarsValues.Add((string)story.variablesState[newVariable]);

                story.ObserveVariable(newVariable, (string key, object value) =>
                {/// finally ask ink to keep the dataclass updated in the event of any changes
                    inkData.inkVars[key] = value;
                    inkData.inkVarsValues[inkData.inkVarsKeys.IndexOf(newVariable)] = (string)value;
                });
            }
        }

        protected void LoadObjectsIntoScene(InkData newData)
        {
            Spd("M");

            SetMusic(newData.sceneState.activeMusic);
            SetAmbiance(newData.sceneState.activeAmbiance);
            SetBackdrop(newData.sceneState.background);
            SetSprites(newData.sceneState.sprites);
        }
        #endregion DATA




        #region STORY ADVANCEMENT
        /// This is the main function called every time the story changes. It does a few things:
        /// Destroys all the old content and choices.
        /// Continues over all the lines of text, then displays all the choices. If there are no choices, the story is finished!
        /// NOTE this adapted version goes over one or a few lines at a time, rather than displaying all at once. It also renders lines per letter. 
        private IEnumerator AdvanceStory()
        {
            /// save prev state
            PutDataIntoStash();

            /// Remove all the UI options on screen
            RemoveOptions();

            /// reset the waitmarkers, and prepare behaviour for at the end of the text: 
            /// display a "next line" icon if story is continueable, create buttons if not
            StartCoroutine(MarkWhenAdvanceable());

            /// Assemble the next paragraph
            string text = AssembleParagraph();

            ///  display the text on screen!          
            yield return StartCoroutine(DisplayContent(text));

        }

        private string AssembleParagraph()
        {
            string text = "";
            while (story.canContinue) /// at most until the story hits a choice
            {
                string newLine = story.Continue(); ///Continue gets the next line of the story 
                if (newLine.StartsWith(">>>")) /// (example) check if this line is being spoken my anybody specific
                {
                    PlaySfx(newLine.Split(">>>")[1].TrimEnd('\n').TrimEnd(' ').ToLower());
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



                /// stop if you hit a paragraph break:
                if (text.EndsWith("\n<br>\n"))
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
            inkData.saveStateCur = story.state.ToJson(); /// save the story state
            inkData.variableState = story.state.variablesState.ToString(); /// save the variables
            StartCoroutine(AdvanceStory()); /// next bit
		}


        public IEnumerator DisplayContent(string text) // Creates a textbox showing the the poaragraph of text
        {
            timeSinceAdvance = 0;
            int i0 = textPanel.text.Length;
            textPanel.text += text;
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
        /*UNUSED DEFAULT METHOD FOR CREATING CONTENT:
        /// Creates a textbox showing the the line of text
        void CreateContentView(string text)
        {
            Text storyText = Instantiate(textPrefab) as Text;
            storyText.text = text;
            storyText.transform.SetParent(canvas.transform, false);
        }
        */
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
        [SerializeField, BoxGroup("INK"), ReadOnly]
        [Tooltip("view which variables have been saved on the ink object")]
        public List<string> inkVarsKeys = new();
        [SerializeField, BoxGroup("INK"), ReadOnly]
        [Tooltip("view which variables have been saved on the ink object")]
        public List<string> inkVarsValues = new();
        public Dictionary<string, object> inkVars = new();
        public InkData(string label) : base(label) { }

        public string variableState = ""; /// class containing states of all variables in story
        public string saveStateCur = ""; /// string indicating most recently saved state of the ink object.

        public StorySceneState sceneState = new();
    }

    [Serializable]
    public class StorySceneState
    {

        public string text = "null";

        public string background = "null";
        public string sprites = "null";


        public string activeMusic = "null";
        public string activeAmbiance = "null";
    }


    #endregion
}