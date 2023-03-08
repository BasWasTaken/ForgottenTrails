using DataService;
using Ink.Runtime;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

		/// SCENE REFERENCES
		[SerializeField, BoxGroup("Scene References"), Required]
		[Tooltip("The main canvas used for GUI elements in this scene.")]
		private Canvas canvas = null;

        [SerializeField, BoxGroup("Scene References"), Required]
        public TextMeshProUGUI textPanel = null;
        [SerializeField, BoxGroup("Scene References")]
        public Image bgImage;
        [SerializeField, BoxGroup("Scene References")]
        public Transform buttonAnchor;
        [SerializeField, BoxGroup("Scene References")]
        public Vector2 buttonOffset = new(0, 1);
        [SerializeField, BoxGroup("Scene References")]
        public Animator triangle;

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

        [SerializeField, Button("ResetInkData")]
		private void ResetInkDataButton() { inkData = CreateBlankData(); }
		[SerializeField, Button("LoadData")]
		private void LoadDataButton() { TryLoadData(); }

		protected bool IsValidFolder(string path)
		{
			return Directory.Exists(path);
		}

		#endregion INSPECTOR_HELPERS


		#region BACKEND FIELDS
		private const string dataLabel = "BasicInkScript"; // but isn't this ridiculous? if you'll be using this object as ink interface all the time, it should't itelf store particular data, you should have objets etc store data... else all will be under here, won't it? or will it just be settings?
		public Story story;
		public static event Action<Story> OnCreateStory;

		public bool CanContinue { get; protected set; }
        public bool CompletedText => textPanel.maxVisibleCharacters == textPanel.text.Length;
        private bool completeText = false;
        private float timeSinceAdvance = 0;

        #endregion BACKEND FIELDS

        #region LIFESPAN	

        override protected void Awake()
		{
			base.Awake();
			CanContinue = false;

			if(true) // if no data present..?
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

			story.BindExternalFunction("Spd", (string speed) => Spd(speed));
			story.BindExternalFunction("Bg", (string fileName) => Bg(fileName));
			story.BindExternalFunction("Msc", (string fileName) => Msc(fileName));
			story.BindExternalFunction("Ambi", (string fileName) => Ambi(fileName));
			story.BindExternalFunction("Sfx", (string fileName) => Sfx(fileName));
			story.BindExternalFunction("ConsoleMessage", (string text) => ConsoleLogInk(text, false));
			story.BindExternalFunction("ConsoleWarning", (string text) => ConsoleLogInk(text, true));

			OnCreateStory?.Invoke(story);

			TryLoadData();

			if (inkData.saveStateCur != "")
			{
				Debug.Log("continueing from savepoint!");
				story.state.LoadJson(inkData.saveStateCur);
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
                else if (story.canContinue & CanContinue & CompletedText)
                {
                    StartCoroutine(AdvanceStory());
                }
            }
            timeSinceAdvance += Time.unscaledDeltaTime; // note don't let overflow
        }

        #endregion LOOP

        #region METHODS


        #region INKY_EXTERNALS

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

        private void Bg(string fileName)
        {
             Debug.LogError("Not yet implemented. Use addressables for this.");
             // old function:
             /*
            if (fileName == "" | fileName == "null")
            {
                //        Debug.Log("no bg loaded.");
            }
            else
            {
                try
                {
                   

                    string fileNameFull = bgDirectory + fileName;
                    Sprite bg = (Sprite)Resources.Load(fileNameFull, typeof(Sprite));
                    if (bg == null)
                    {
                        throw new FileNotFoundException("File not found: " + bgDirectory + fileName);
                    }
                    //if (State.displayState == NovelState.DisplayState.Dialogue)
                    //{
                    dialogueWindowObject.bgImage.sprite = bg;
                    inkData.sceneState.background = fileName;
                    //}
                    //else if (State.displayState == NovelState.DisplayState.Narration)
                    //{
                    dialogueWindowObject.bgImageAlt.sprite = bg;
                    inkData.sceneState.backgroundAlt = fileName;
                    //}
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
             */

        }


        private void Msc(string fileName)
        {
            Debug.LogError("Not yet implemented. Use addressables for this.");
            // old function:
            /*
           if (fileName == "")
           {
               audioSourceMusic.Stop();
           }
           else
           {
               try
               {
                   AudioClip var = (AudioClip)Resources.Load(musicDirectory + fileName, typeof(AudioClip));

                   if (audioSourceMusic.clip != var)
                   {
                       audioSourceMusic.clip = var;
                       inkData.sceneState.activeMusic = fileName;
                       audioSourceMusic.Play();
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
            */
       }
       private void Ambi(string fileName)
       {
            Debug.LogError("Not yet implemented. Use addressables for this.");
            // old function:
            /*
          if (fileName == "")
          {
              audioSourceAmbiance.Stop();
          }
          else
          {
              try
              {
                  AudioClip var = (AudioClip)Resources.Load(sfxDirectory + fileName, typeof(AudioClip));

                  if (audioSourceAmbiance.clip != var)
                  {
                      audioSourceAmbiance.clip = var;
                      inkData.sceneState.activeAmbiance = fileName;
                      audioSourceAmbiance.Play();
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
            */
      }

      private void Sfx(string fileName)
      {
            Debug.LogError("Not yet implemented. Use addressables for this.");
            // old function:
            /*
            try
            {
                AudioClip var = (AudioClip)Resources.Load(sfxDirectory + fileName, typeof(AudioClip));
                if (var == null)
                {
                    throw new FileNotFoundException("File not found: " + sfxDirectory + fileName);
                }
                if (audioSourceSfx.clip != var)
                {
                    audioSourceSfx.clip = var;
                    audioSourceSfx.PlayOneShot(var);
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
            */
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
        protected void PutDataIntoStash() // this should then be called every so often and whenever the save button is pressed
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

            Msc(newData.sceneState.activeMusic);
            Ambi(newData.sceneState.activeAmbiance);
            Bg(newData.sceneState.background);
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

            StartCoroutine(MarkWhenContinuable());

            /// Continue gets the next line of the story (removed .trim())
            string text = story.Continue();
            
            /// Read all the content until we can't continue any more and display the text on screen!          
            yield return StartCoroutine(DisplayContent(text));
            
        }

        public void RemoveOptions()/// Destroys all the buttons from choices
        {
            int childCount = buttonAnchor.childCount;
            foreach(Button child in transform.GetComponentsInChildren<Button>())
            {
                Destroy(child.gameObject);
            }
        }

        private IEnumerator MarkWhenContinuable()
        {
            // remove bouncing arrow 
            CanContinue = false;
            yield return new WaitUntil(() => story.canContinue);
            yield return new WaitForSecondsRealtime(advanceDialogueDelay);
            yield return new WaitUntil(() => CompletedText);
            if (PresentButtons())
            {

            }
            else
            {
                // set bouncing triangle at most recent line
            }
            CanContinue = true;
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
                Button choice = PresentButton("End of story.\nRestart?");
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
            choice.transform.SetParent(canvas.transform, false);

            /// Gets the text from the button prefab
            Text choiceText = choice.GetComponentInChildren<Text>();
            choiceText.text = text;

            /// Make the button expand to fit the text
            HorizontalLayoutGroup layoutGroup = choice.GetComponent<HorizontalLayoutGroup>();
            layoutGroup.childForceExpandHeight = false;

            return choice;
        }
        void OnClickChoiceButton(Choice choice)
        {
            story.ChooseChoiceIndex(choice.index); /// feed the choice
            inkData.saveStateCur = story.state.ToJson(); /// save the story state
            inkData.variableState = story.state.variablesState.ToString(); /// save the variables
            StartCoroutine(AdvanceStory()); /// next bit
		}


        public IEnumerator DisplayContent(string text) // Creates a textbox showing the the line of text
        {
            timeSinceAdvance = 0;
            textPanel.text = text;
            for (int i = 0; i < text.Length + 1; i++)
            {
                textPanel.maxVisibleCharacters = i;
                yield return new WaitForSecondsRealtime(1 / textSpeed);
                yield return new WaitUntil(() => isActiveAndEnabled);
                if (completeText)
                {
                    textPanel.maxVisibleCharacters = text.Length;
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

        public string activeMusic = "null";
        public string activeAmbiance = "null";
    }


    #endregion
}