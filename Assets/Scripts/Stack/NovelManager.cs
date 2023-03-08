using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Ink.Runtime;
using UnityEngine.UI;
using Utility;
using NaughtyAttributes;
using TMPro;
using DataService;
using System.IO;

namespace Ink 
{
    /// <summary>
    /// <para>INK window and story interaction object canibalised from mso project.</para>
    /// </summary>
    public class NovelManager : MonoSingleton<NovelManager>
    {
#region INSPECTOR_VARIABLES

#region SCENE REFERENCES

        [SerializeField, BoxGroup("Scene References"), Required]
        [Tooltip("Here drag the main canvas used for GUI elements in this scene.")]
        protected Canvas canvas;
        [SerializeField, BoxGroup("Scene References"), Required]
        [Tooltip("Here drag the component used for sfx.")]
        protected AudioSource audioSourceSfx;
        [SerializeField, BoxGroup("Scene References"), Required]
        [Tooltip("Here drag the component used for music.")]
        protected AudioSource audioSourceMusic;
        [SerializeField, BoxGroup("Scene References"), Required]
        [Tooltip("Here drag the component used for ambiance.")]
        protected AudioSource audioSourceAmbiance;

        [SerializeField, BoxGroup("Scene References"), Required, ShowAssetPreview]
        [Tooltip("Here drag the object for the visual novel screen")]
        protected DialogueWindow dialogueWindowObject;

#endregion SCENE REFERENCES & PREFABS

#region INK_ASSET_INFO

        [SerializeField, BoxGroup("INK")]//, Required]
        [Tooltip("Here drag the INK object containing the dialogue behaviour")]
        public TextAsset inkJSONAsset = null;

        [SerializeField, BoxGroup("INK")]
        [Tooltip("Data object containing INK data.")]
        protected InkData inkData;

        [SerializeField, BoxGroup("INK/Assets")]//, ValidateInput("IsValidFolder", "Enter an existing asset folder.")]
        [Tooltip("The folder to check for AudioClips.")]

        protected string sfxDirectory = "";
        [SerializeField, BoxGroup("INK/Assets")]//, ValidateInput("IsValidFolder", "Enter an existing asset folder.")]
        [Tooltip("The folder to check for AudioClips.")]
        protected string musicDirectory = "Music/";
        [SerializeField, BoxGroup("INK/Assets")]//, ValidateInput("IsValidFolder", "Enter an existing asset folder.")]
        [Tooltip("The folder to check for Portraits.")]
        protected string portraitDirectory = "Portraits/Three Houses Temp Portraits/";
        [SerializeField, BoxGroup("INK/Assets")]//, ValidateInput("IsValidFolder", "Enter an existing asset folder.")]
        [Tooltip("The folder to check for backgrounds.")]
        protected string bgDirectory = "Backgrounds/";

#endregion INK_ASSET_INFO

        [SerializeField, BoxGroup("INK/Settings")]
        [Tooltip("Choose wether the story should be active or inactive on scene start.")]
        protected bool autoStart = true;

        [SerializeField, BoxGroup("INK/Settings")]
        [Tooltip("Delay after which space button advances dialogue.")]
        protected float advanceDialogueDelay = .1f;
        public float AdvanceDialogueDelay => advanceDialogueDelay;

        [SerializeField, BoxGroup("INK/Settings"), Foldout("TextSpeed")]
        [Tooltip("Choose a numeric value for this option.")]
        protected float slowText = 10f;
        [SerializeField, BoxGroup("INK/Settings"), Foldout("TextSpeed")]
        [Tooltip("Choose a numeric value for this option.")]
        protected float mediumText = 50f;
        [SerializeField, BoxGroup("INK/Settings"), Foldout("TextSpeed")]
        [Tooltip("Choose a numeric value for this option.")]
        protected float fastText = 200f;

        #endregion INSPECTOR_VARIABLES

#region INSPECTOR_HELPERS

        [SerializeField, Button("ResetInkData")]
        protected void ResetInkDataButton() { inkData = CreateBlankData(); }
        [SerializeField, Button("LoadData")]
        protected void LoadDataButton() { TryLoadData(); }

        protected bool IsValidFolder(string path)
        {
            return Directory.Exists(path);
        }

#endregion INSPECTOR_HELPERS

#region BACKEND_FIELDS
        private const string dataLabel = "NovelManager";
        
        protected Story story; 
        public event Action<Story> OnCreateStory;
        /// <summary>
        /// <para>Gets <see cref="storyActive"/> or sets it after handling the activation code.</para>
        /// <returns>Returns the logical answer to "is story active?"</returns>
        /// </summary>
        public bool StoryActive // kan hier uiteindelijk een (static) functie van maken die verschillende stories kan handelen, met parameter
        {
            get { return storyActive; }

            protected set
            {
                storyActive = value;
                if (value == true)
                {
                    dialogueWindowObject.gameObject.SetActive(true);
                    if (!activatedBefore)
                    {
                        activatedBefore = true;
                        StartStory();
                    }
                }
                else
                {
                    dialogueWindowObject.gameObject.SetActive(false);
                }
            }
        }
        private bool storyActive = false;
        private bool activatedBefore = false;
        public bool CanContinue { get; protected set; }

        [Serializable]
        public class NovelState
        {
            public enum DisplayState
            {
                Dialogue,
                Narration
            }
            public DisplayState displayState = DisplayState.Dialogue; 
            public enum ControlState
            {
                Manual, 
                Auto
            }
            public ControlState controlState = ControlState.Manual;

            public void EnterState<T>(T state) where T : Enum
            {
                var newState = Enum.Parse(typeof(T), state.ToString());
                if (state.GetType() == typeof(DisplayState))
                {
                    EnterDisplayState((DisplayState)newState);
                }
                else if (state.GetType() == typeof(ControlState))
                {
                    EnterControlState((ControlState)newState);
                }
            }
            protected void EnterDisplayState(DisplayState state)
            {
                if (displayState != state)
                {
                    displayState = state;
                }
            } 
            protected void EnterControlState(ControlState state)
            {
                if (controlState != state)
                {
                    controlState = state;
                }
            }
        }
        public NovelState State { get; protected set; }

#endregion BACKEND_VARIABLES        

///___METHODS___///
#region MONOBEHAVIOUR
#region LIFESPAN
        override protected void Awake()
        {
            base.Awake();
            CanContinue = false;
            State = new();
            if (true)
            {
                inkData = CreateBlankData();
            }
            audioSourceMusic.loop = true;
            audioSourceSfx.loop = false;
            audioSourceAmbiance.loop = true;
            dialogueWindowObject.Init(); /// initiate dialogue window
            dialogueWindowObject.gameObject.SetActive(false);
        }
        private void Start()
        {
            if (inkJSONAsset != null)
            {
                StartUp(); // give story and/or data?
            }

        }
        private void StartUp()
        {

            if (autoStart)
            {
                ToggleStoryActive();
            }

        }
        public void StartNewStory(TextAsset textAsset)
        {
            if (inkJSONAsset != null)
            {
                Debug.LogWarning("Overriding while json asset stil lactive!");
            }
            else
            {
                
                activatedBefore = false;
                inkJSONAsset = textAsset;
                inkData = CreateBlankData();

                StartUp();
            }
        }
        protected void StartStory()
        {
            story = new Story(inkJSONAsset.text);

            story.BindExternalFunction("Spd", (string speed) => Spd(speed, false));
            story.BindExternalFunction("SpdAuto", (string speed) => Spd(speed, true));
            story.BindExternalFunction("Tag", (string name, string position) => Tag(name, position));
            story.BindExternalFunction("ClrTag", () => Tag("", "L"));
            
            story.BindExternalFunctionGeneral("SetPort", (object[] args) =>
            {
                if (int.TryParse(args[0].ToString(), out int parsed))
                {
                    SetPort(parsed-1, args[1].ToString(), args[2].ToString(), args[3].ToString());
                    return null;
/*                    SetPortDelegate setPortDelegate = delegate (string[] args)
                    {
                        return SetPort(parsed, args[1].ToString(), args[2].ToString(), args[3].ToString());
                    };
                    return setPortDelegate;
                    return nul;*/
                }
                else
                {
                    Console.WriteLine($"Int32.TryParse could not parse '{args[0]}' to an int.");
                    throw new InvalidCastException();
                }

            });

            story.BindExternalFunction("ClrPort", (int slot, string animation) => ClrPort(slot-1, animation));
            story.BindExternalFunction("ClrPorts", (string animation) => ClrPorts(animation));
            story.BindExternalFunction("Anim", (int slot, string animation) => Anim(slot, animation));
            story.BindExternalFunction("Bg", (string fileName) => Bg(fileName));
            story.BindExternalFunction("Msc", (string fileName) => Msc(fileName));
            story.BindExternalFunction("Ambi", (string fileName) => Ambi(fileName));
            story.BindExternalFunction("Sfx", (string fileName) => Sfx(fileName));
            story.BindExternalFunction("ConsoleMessage", (string text) => ConsoleLogInk(text, false));
            story.BindExternalFunction("ConsoleWarning", (string text) => ConsoleLogInk(text, true));
            story.BindExternalFunction("EnterMode", (int i) => EnterMode(i-1));


            

            /*
            if (inkData.saveStateCur != "") /// if were just in conversation...
            {
                story.state.LoadJson(inkData.saveStateCur); /// load convo back
            }
            else if (inkData.variableState != "") /// if a conversation has taken place, but is not currently saved
            {
                story.ResetState();
            }
            else /// if this is the first time
            {
                story.state.GoToStart();
            }*/
            OnCreateStory?.Invoke(story); /// fire oncreatestory event if it has any listeners
            // hier kan je vast wat mee doen

            TryLoadData();

            if (inkData.saveStateCur != "")
            {
                Debug.Log("continueing from savepoit!");
                story.state.LoadJson(inkData.saveStateCur);
            }
            else
            {
                Debug.Log("no save point detected, startiung from stratr");
                story.state.GoToStart();
            }
            if(story.canContinue)StartCoroutine(AdvanceDialogue()); /// show the first bit of dialogue
        }
#endregion LIFESPAN
        
#region LOOP
        private void Update()
        {
            if (State.controlState==NovelState.ControlState.Manual)
            {
                if (inkJSONAsset != null)
                {
                    if (Input.GetKeyDown(KeyCode.Tab))
                    {
                        ToggleStoryActive();
                    }
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.KeypadEnter))
                    {
                        SceneHandler.Instance.StartNextStory();
                    }
                }

                if (StoryActive)
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
  //                      Debug.Log("Space!");
                        if (story.canContinue & CanContinue & dialogueWindowObject.CompletedText)
                        {
//                            Debug.Log("Doe wat!");
                            StartCoroutine(AdvanceDialogue());
                        }
                        else
                        {
       //                     Debug.Log(story.canContinue.ToString() + CanContinue.ToString() + dialogueWindowObject.CompletedText.ToString());
                        }
                    }
                }
            }
        }
#endregion LOOP
#endregion MONOBEHAVIOUR

#region INK_INTEGRATION

        public bool ToggleStoryActive()
        {
            if (StoryActive)
            {
                Time.timeScale = 1;
                foreach (AudioSource source in FindObjectsOfType<AudioSource>())
                {
                    if (source.gameObject.CompareTag(gameObject.tag))
                    {
                        source.Pause();
                    }
                    else
                    {
                        source.UnPause();
                    }
                }
                StoryActive = false;
            }
            else
            {
                Time.timeScale = 0;
                foreach (AudioSource source in FindObjectsOfType<AudioSource>())
                {
                    if (source.gameObject.CompareTag(gameObject.tag))
                    {
                        source.UnPause();
                    }
                    else
                    {
                        source.Pause();
                    }
                }
                StoryActive = true;
            }
            return StoryActive;
        }

#region INKY_EXTERNALS

        private void Spd(string speed, bool auto = false)
        {
            if (auto)
            {
                State.EnterState(NovelState.ControlState.Auto);
            }
            else
            {
                State.EnterState(NovelState.ControlState.Manual);
            }

            char first = speed[0];
            if (first == 'S' | first == 's')
            {
                dialogueWindowObject.textSpeed = slowText;
            }
            else if (first == 'M' | first == 'm')
            {
                dialogueWindowObject.textSpeed = mediumText;
            }
            else if (first == 'F' | first == 'f')
            {
                dialogueWindowObject.textSpeed = fastText;
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
        
        private void Tag(string name, string position)
        {
            if (State.displayState == NovelState.DisplayState.Dialogue)
            {
                SetNameTagFor(name, dialogueWindowObject.nameTag, position);
            }
            else if(State.displayState == NovelState.DisplayState.Narration)
            {
                SetNameTagFor(name, dialogueWindowObject.nameTagAlt, position);
            }
        }
        protected void SetNameTagFor(string name, Image tag, string position)
        {
            char first = position[0];

            tag.GetComponentInChildren<TextMeshProUGUI>().text = name;
            inkData.sceneState.tag = name;

            if (name == "")
            {
                tag.gameObject.SetActive(false);
            }
            else if (!tag.isActiveAndEnabled)
            {
                tag.gameObject.SetActive(true);
            }
            if (position != "")
            {
                if (first == 'L' | first == 'l')
                {
                    tag.rectTransform.anchorMin = new(0, 1);
                    tag.rectTransform.anchorMax = new(0, 1);
                    tag.rectTransform.anchoredPosition = new(0, 50);
                    inkData.sceneState.tagPos = position;
                }
                else if (first == 'R' | first == 'r')
                {
                    tag.rectTransform.anchorMin = new(1, 1);
                    tag.rectTransform.anchorMax = new(1, 1);
                    tag.rectTransform.anchoredPosition = new(-250, 50);
                    inkData.sceneState.tagPos = position;
                }
                else
                {
                    Debug.LogError("INKY Error: position parameter \'" + position + "\' for tag function not recognised. Input either \'L\' or \'R\'.");
                    return;
                }

                if (position.Length > 1)
                {
                    Debug.LogWarning(string.Format("Note: You requested the position be changed to {0}, which was recognised as {1} and the applied accordingly. However, you only need to supply either the letter \'L\' for left or \'R\' for Right. Please address this in your INK Story.", position, first));
                }
            }
        }
        private void SetPort(int slot, string character, string sprite, string direction="R")
        {
            try
            {
                string filename = portraitDirectory + character + "-" + sprite;

                Sprite sprite1 = (Sprite)Resources.Load(filename, typeof(Sprite));
                if(sprite1==null)
                { 
                    throw new FileNotFoundException("File not found: " + filename);
                }


                if (slot>-1 & slot < 5)
                {
                    Image image = dialogueWindowObject.portraits[slot];

                    if (State.displayState == NovelState.DisplayState.Dialogue)
                    {
                        image.sprite = sprite1;


                        char first = direction[0];
                        if (first == 'L' | first == 'l')
                        {
                            image.rectTransform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                        }
                        else if (first == 'R' | first == 'r')
                        {
                            image.rectTransform.localRotation = Quaternion.Euler(0f, 180f, 0f);
                        }
                        else
                        {
                            Debug.LogError("INKY Error: Direction value \"" + direction + "\" not recognised. Please supply either the letter \'S\' for slow, \'M\' for medium, or \'F\' for fast. Speed was not changed.");
                            return;
                        }

                        if (direction.Length > 1)
                        {
                            Debug.LogWarning(string.Format("Note: You requested the direction be changed to {0}, which was recognised as {1} and the applied accordingly. However, you only need to supply either the letter \'L\' for left or \'R\' for right. Please address this in your INK Story.", direction, first));
                        }
                        PortraitInScene portrait = new(character, sprite, direction);
                        inkData.sceneState.portraitInScenes[slot] = portrait;

                    }
                    else
                    {
                        Debug.LogError("Wrong mode");
                    }


                }
                else
                {
                    Debug.LogError("INKY Error: Slot value \"" + slot + "\" not recognised. Please supply a number between and 5 including.");
                    return;
                }
                
                
            }
            catch (Exception e)
            {
                // Extract some information from this exception, and then
                // throw it to the parent method.
                if (e.Source != null)
                    Console.WriteLine("IOException source: {0}", e.Source);
                throw e;
            }

            

        }
        
        private void ClrPort(int slot, string animation="")
        {
            if(animation!="runRight" & animation != "runLeft" & animation != "fadeOut")
            {
                animation = "";
            }
            if (slot > -1 & slot < 5)
            {
                if (dialogueWindowObject.portraits[slot].color.a > 0) /// if not already absent
                {
                    if (animation == "") /// set generic absent
                    {
                        dialogueWindowObject.portraits[slot].GetComponent<Animator>().SetTrigger("absent");
                        dialogueWindowObject.portraits[slot].sprite = null;
                    }
                    else /// or set specific absent if available
                    {
                        dialogueWindowObject.portraits[slot].GetComponent<Animator>().SetTrigger(animation);
                        // set background process to clean up sprite if nothing else is put here in a second or so
                    }
                }
                PortraitInScene portrait = null;
                inkData.sceneState.portraitInScenes[slot] = portrait;


            }
            else
            {
                Debug.LogError("INKY Error: Slot value \"" + slot + "\" not recognised. Please supply a number between 1 and 5 including.");
                return;
            }
        }
        
        private void ClrPorts(string animation="")
        {
            for (int i = 0; i < 5; i++)
            {
                ClrPort(i, animation);
            }
        }
        
        private void Anim(int slot, string animation)
        {
            dialogueWindowObject.portraits[slot-1].GetComponent<Animator>().SetTrigger(animation);
        }

        private void Bg(string fileName)
        {
            if(fileName=="" | fileName == "null")
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

        }
        

        private void Msc(string fileName)
        {
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
        }
        private void Ambi(string fileName)
        {
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
        }

        private void Sfx(string fileName)
        {
            /*
            wsl beter om even met clips te werken voor nu, dan kan vugs het er makkelijker inslepen. op termijn kun je misschien ee nfunctie maken die bij drag automatisch audioevents maakt.
            try
            {
                AudioEvent var = (AudioEvent)AssetDatabase.LoadAssetAtPath("Assets/4) Audio/Raw Files/" + fileName, typeof(AudioEvent));
                var.Play(audioSourceSfx);
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

        void EnterMode(int i)
        {
            
            try
            {
                NovelState.DisplayState state = (NovelState.DisplayState)i;
                State.EnterState(state);
                if (state == NovelState.DisplayState.Narration)
                {
                    dialogueWindowObject.bgImageAlt.gameObject.SetActive(true);
                    dialogueWindowObject.textPanelAlt.rectTransform.parent.parent.gameObject.SetActive(true);
                }
                else
                {
                    dialogueWindowObject.bgImageAlt.gameObject.SetActive(false);
                    dialogueWindowObject.textPanelAlt.rectTransform.parent.parent.gameObject.SetActive(false);
                }

                Debug.Log("entered state " + state.ToString());
            }
            catch (Exception e)
            {
                // Extract some information from this exception, and then
                // throw it to the parent method.
                if (e.Source != null)
                    Console.WriteLine("IOException source: {0}", e.Source);
                throw;
            }

            /*
            //----
            // aantal opties:
            // objecten activeren en deactiveren
            // twee stories in scene hebben, en steeds 1 (de)activeren en state inladen (klinkt als veel werk)
            // in speciale mode wwn black bg eroverheen tekenen en tekst daarover in losse functie
            // alle sprekers enzo weghalen en daarna weer terugzetten
            // 
            // sws moet ik ten minste de tekstobjecten veranderen, if not vervangen/extra maken. so take that into consideration, dat er sws al IETS nodig is daar
            //
            // opzich denk ik dat het fine is als met gebruik van deze modus alle portretten weg zijn, evt met een bool retaincharacters
            int mode = 0;
            if (temp == "1")
            {
                mode = 1;
            }
            else if (temp == "2")
            {
                mode = 2;
            }
            else
            {
                Debug.LogError("invalid mode: " + temp);
                return;
            }

            if (Mode == mode)
            {
                Debug.LogError("already in mode " + mode);
            }
            else
            {
                if (mode == 1)
                {
                    dialogueWindowObject.bgImageAlt.gameObject.SetActive(false);
                    dialogueWindowObject.textPanelAlt.transform.parent.gameObject.SetActive(false);
                }
                else if (mode == 2)
                {
                    RemoveAllPortraits();
                    SetNameTag("");
                    dialogueWindowObject.bgImageAlt.gameObject.SetActive(true);
                    dialogueWindowObject.textPanelAlt.transform.parent.gameObject.SetActive(true);
                }
                Mode = mode;
            }
            throw new NotImplementedException();
            */
        }
        #endregion InkyExternals

#region INK_DATA

        
        public InkData CreateBlankData()
        {
            InkData data;
            if (SceneHandler.Instance != null)
            {
                data = new(dataLabel + SceneHandler.Instance.level);

            }
            else
            {
                data = new(dataLabel + "TestingScene");
            }

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

            inkData.sceneState.displayState = (int)State.displayState;
            inkData.saveStateCur = story.state.ToJson();
            inkData.StashData();
        }

        private InkData LoadData(InkData input)
        {
            //Debug.Log("gonna try to load data!");
            if (inkData != input)
            {
                //Debug.Log("confirmed data is not identical");
                if (activatedBefore)
                {
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
                    //Debug.Log("not yet activated so cannot load yet!");
                    return inkData;
                }
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

            foreach(var newVariable in newVariables)
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

        protected void LoadObjectsIntoScene(InkData newData) // use a var?
        {
            EnterMode(newData.sceneState.displayState); 
            Spd("M");

            Msc(newData.sceneState.activeMusic);
            Ambi(newData.sceneState.activeAmbiance);
            Tag(newData.sceneState.tag, newData.sceneState.tagPos);
            Bg(newData.sceneState.background);
            

            for (int i = 0; i < newData.sceneState.portraitInScenes.Length; i++)
            {
                PortraitInScene import = newData.sceneState.portraitInScenes[i];
                if (import == null)
                {
                    ClrPort(i);
                }
                else
                {
                    SetPort(i, import.character, import.sprite); // later still give the extra argument here
                }

            }
        }

        #endregion INK_DATA

        #region STORY_ADVANCEMENT

        protected IEnumerator MarkCanContinue()
        {
      //      Debug.Log("test");
            CanContinue = false;
            yield return new WaitUntil(() => story.canContinue);
            yield return new WaitForSecondsRealtime(advanceDialogueDelay);
            yield return new WaitUntil(() => dialogueWindowObject.CompletedText);
            if (State.controlState == NovelState.ControlState.Manual)
            {
                if (PresentButtons())
                {

                }
                else
                {
                    dialogueWindowObject.triangle.gameObject.SetActive(true);
                }
            }
//            Debug.Log("CanContinue!");
            CanContinue = true;
        }
        protected IEnumerator AdvanceDialogue()
        /// This is the main function called every time the story changes. It does a few things:
        /// Destroys all the old content and choices.
        /// Continues over all the lines of text, then displays all the choices. If there are no choices, the story is finished!
        /// NOTE Instead of going over all lines, I do it one bit at a time, so you advance with the space key.
        {
            /// save prev state
            inkData.sceneState.text = dialogueWindowObject.ActiveTextPanel.text;
            PutDataIntoStash();

            dialogueWindowObject.ClearUI();      /// Remove all the story UI on screen
            dialogueWindowObject.triangle.gameObject.SetActive(false);
            CanContinue = false;
            StartCoroutine(MarkCanContinue());
            string text = story.Continue().Trim();/// Continue gets the next line of the story
            yield return StartCoroutine(dialogueWindowObject.DisplayContent(text, State.displayState));/// Display the text on screen!

            if (State.controlState==NovelState.ControlState.Auto)
            {
                yield return new WaitUntil(() => CanContinue);
                StartCoroutine(AdvanceDialogue());
            }
            else
            {
            }
        }

        protected bool PresentButtons()
        {
            if (story.canContinue) // if we can continue
            {
                /*
                Button choice = dialogueWindowObject.PresentButtons("Next"); // make 'next' button
                choice.onClick.AddListener(delegate {
                    StartCoroutine(AdvanceDialogue());
                });
                */
                return false;
            }
            else if (story.currentChoices.Count > 0)     // Display all the choices, if there are any!
            {
                for (int i = 0; i < story.currentChoices.Count; i++)
                {
                    Choice choice = story.currentChoices[i];
                    Button button = dialogueWindowObject.PresentButtons(choice.text.Trim(), i, State.displayState);

                    button.onClick.AddListener(delegate {// Tell the button what to do when we press it
                        OnClickChoiceButton(choice);
                    });
                }
                return true;
            }
            else // If we've read all the content and there's no choices, the story is finished!
            {
                Button choice = dialogueWindowObject.PresentButtons("Close Dialogue", 0, State.displayState);
                choice.onClick.AddListener(delegate {
                    dialogueWindowObject.ClearUI(State.displayState);
                    OnInteractionEnd();

                });
                return true;
            }
        }

        public void OnClickChoiceButton(Choice choice) // When we click the choice button, tell the story to choose that choice!
        {
            story.ChooseChoiceIndex(choice.index); // feed the choice
            inkData.saveStateCur = story.state.ToJson(); // save the story state
            inkData.variableState = story.state.variablesState.ToString(); // save the variables
            StartCoroutine(AdvanceDialogue()); // next dialogue
        }

#endregion STORY_ADVANCEMENT

#region ENDING_INTERACTIONS
        protected void OnInteractionEnd()
        {
            story.RemoveVariableObserver();
            PutDataIntoStash();
            inkJSONAsset = null;
            story = null;
            SceneHandler.Instance.OnStoryFinished();
        }


#endregion ENDING_INTERACTIONS


#endregion INK_INTEGRATION

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
        public int displayState =0;

        public string tag = "null";
        public string tagPos= "null";
        public string text = "null";

        public string background = "null";
        public string backgroundAlt = "null";

        public string activeMusic = "null";
        public string activeAmbiance = "null";

        public PortraitInScene[] portraitInScenes = new PortraitInScene[5];
    }

    [Serializable]
    public class PortraitInScene
    {
        public string character = "null";
        public string sprite = "null";
        public string direction = "null";

        public PortraitInScene(string _character, string _sprite, string _direction)
        {
            character = _character;
            sprite = _sprite;
            direction = _direction;
        }
    }

    #endregion
}
