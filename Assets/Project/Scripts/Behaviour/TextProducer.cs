using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using Extensions;
using Ink.UnityIntegration;
using Ink.Runtime;
using NaughtyAttributes;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System.Linq;
using Bas.Utility;
using UnityEngine.UI;

namespace ForgottenTrails.InkFacilitation
{
    /// <summary>
    /// <para>Produces text onto ui elements for the player.</para>
    /// </summary>
    /// delay effect from https://github.com/Tioboon/LogWritter/blob/main/EventController.cs
    public class TextProducer : MonoSingleton<TextProducer>
    {
        // Inspector Properties
        #region Inspector Properties

        [field: SerializeField, BoxGroup("Scene References"), Required]
        [Tooltip("Panel to display current paragraph.")]
        private TextMeshProUGUI TextBox { get; set; }

        [field: SerializeField, BoxGroup("Scene References"), Required]
        [Tooltip("Panel to collect overflow text.")]
        private TextMeshProUGUI OverFlowTextBox { get; set; }

        [field: SerializeField, BoxGroup("Scene References"), Required]
        [Tooltip("Panel to display previous text.")]
        private TextMeshProUGUI HistoryTextBox { get; set; }

        [SerializeField, ReadOnly]
        private TextSpeed _textSpeedPreset;
        public TextSpeed TextSpeedPreset
        {
            get
            { return _textSpeedPreset; }
            set
            {
                Debug.Log(string.Format("Changed from {0} to {1} speed", TextSpeedPreset.ToString(), value.ToString()));
                _textSpeedPreset = value;
                PlayerPrefs.SetInt("textSpeed", (int)_textSpeedPreset);
            }
        }
        [field: SerializeField, BoxGroup("Settings")]
        [Tooltip("Define pause timings here.")]
        private PauseInfo _pauseInfo { get; set; } = new()
        {
            _dotPause = .2f,
            _commaPause = .1f,
            _spacePause = .02f,
            _normalPause = .01f
        };
        [field: SerializeField, BoxGroup("Settings")]
        [Tooltip("Define tiny timings here for when skipping text.")]
        private PauseInfo _pauseInfoForSkips { get; set; } = new()
        {
            _dotPause = 0.000000005f,
            _commaPause = 0.000000002f,
            _spacePause = 0.0000000005f,
            _normalPause = 0.0000000001f
        };

        [field: SerializeField, BoxGroup("Settings")]
        public bool AutoAdvance { get; private set; } = false;
        [field: SerializeField, BoxGroup("Settings")]
        public bool AlwaysPause { get; private set; } = true;
        [field: SerializeField, BoxGroup("Settings")] 
        public bool ClearWhenFull { get; private set; } = true;

        #endregion
        // Public Properties
        #region Public Properties
        #region State Machine
        #endregion

        public float TextSpeedMod { get; private set; }
        public float TextSpeedActual => ((float)TextSpeedPreset) * TextSpeedMod;


        private PState _state = PState.Booting;
        public PState State // TODO: Encorporate into state
        {
            get { return _state; }
            private set
            {
                if (_state == value)
                {
                    Debug.LogWarning("Redundant state transition");
                }
                _state = value;
            }
        }


        public bool Skipping { get; private set; } // TODO: Encorporate into state machine?
        public bool EncounteredStop { get; set; } = false; // TODO: Encorporate into statemachine?
        public bool peeking { get; set; } // TODO: Incorporate into statemachine
        

        public string CurrentText
        {
            get
            {
                return TextBox.text;
            }
            private set
            {
                TextBox.text = value;
            }
        }
        public int VisibleCharacters
        {
            get
            {
                return TextBox.maxVisibleCharacters;
            }
            private set
            {
                TextBox.maxVisibleCharacters = value;
            }
        }
        public string PreviousText
        {
            get
            {
                return HistoryTextBox.text;
            }
            private set
            {
                HistoryTextBox.text = value;
            }
        }

        #endregion
        // Private Properties
        #region Private Properties
        private StoryController StoryController { get; set; }

        private Story Story => StoryController.Story;

        private PauseInfo GetPauseInfo
        {
            get
            {
                return Skipping ? _pauseInfoForSkips : _pauseInfo;
            }
        }

        private string NewText
        {
            get; set;
        }

        #endregion
        // MonoBehaviour LifeCycle Methods
        #region MonoBehaviour LifeCycle Methods
        protected override void Awake()
        {
            //TODO: Encorporate into statemachine
            base.Awake();
            _textSpeedPreset = (TextSpeed)PlayerPrefs.GetInt("textSpeed", (int)_textSpeedPreset);
            CurrentText = ""; //clear lorum ipsum
            VisibleCharacters = 0;
            PreviousText = ""; //clear lorum ipsum
            StoryController = GetComponent<StoryController>();
            State = PState.Idle;
            Skipping = false;
        }

        #endregion
        // Public Methods
        #region Public Methods
        public IEnumerator FeedText(string newText)
        {
            if (State != PState.Idle)
            {
                Debug.LogError(string.Format("Text displayer is {0}", State.ToString()));

                yield return null;
            }
            else
            {
                string parsedText = ParseText(newText);

                NewText = parsedText; /// add new text to target
            }
        }

        public void FastForward()
        {
            if (State == PState.Producing & /*CanSkip*/true)
            {
                if (!Skipping)
                {
                    //Debug.Log("Skip this line!");
                    Skipping = true;
                }
                else
                {
                    Debug.LogWarning("Already skipping!");
                }
            }
            else
            {
                Debug.LogWarning("Cannot skip right now.");
            }
        }
        public void ResetSkipping()
        {
            if (Skipping)
            {
                //Debug.Log("Done Skipping");
                Skipping = false;
            }
        }

        public void ClearPage()
        {
            if (peeking) return;
            if (State == PState.Producing)
            {
                throw new("Text displayer is busy!");
            }
            else
            {
                HistoryTextBox.text = CurrentText; // move all text to the history log
                CurrentText = ""; // clear current and prospective texts
                VisibleCharacters = 0;
            }
        }

        public void Spd(float speed)
        {
            StoryController.InkDataAsset.SceneState.TextSpeedMod = speed;
            TextSpeedMod = speed;
        }

        #endregion
        // Private Methods
        #region Private Methods
        private string ParseText(string input)
        {
            Regex tags = new(@"\{([^>]+)\}");

            foreach (Match match in tags.Matches(input))
            {
                //Debug.Log(string.Format("Encountered command {0} in {1}",match.Value,output));

                if (match.Value == "{stop}")
                {
                    //Debug.Log("recognised stop command");
                    EncounteredStop = true;
                    //catchAfterStop = input.Substring(match.Index+match.Value.Length);

                    //input = input.Substring(0, match.Index); 
                }
                else if (match.Value == "{glue}")
                {
                    input = input.TrimEnd('\n'); /// remove linebreak from this
                }
                else if (match.Value == "{aglue}")
                {
                    TextBox.text = TextBox.text.TrimEnd('\n'); /// remove linebreak from previous
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

        /// <summary>
        /// Runs the per line steps required for parsing and showing ink story
        /// </summary>
        public IEnumerator ProduceTextOuter()
        {

            #region TryFit
            Stopwatch stopwatch = new();
            if (ClearWhenFull)
            {
                // construct future chcker for all upcoming lines here
                var storedState = story.state.ToJson(); /// set return point
                peeking = true; /// begin traversal
                string toBeAdded = "";
                while (story.canContinue) /// continue maximally or to stop
                {
                    toBeAdded += story.Continue();
                    if (toBeAdded.Contains("{stop}")) break;
                }

                /// check size and clear page if needed

                string bufferText = CurrentText; /// make backup
                VisibleCharacters = TextBox.text.Length;

                CurrentText += toBeAdded + '\n'; /// test if the text will fit
                yield return 0;/// wait 1 frame
                bool overflow = OverFlowTextBox.text.Length > 0; /// store result

                CurrentText = bufferText; /// restore backup
                story.state.LoadJson(storedState); /// return to original state, reverting from peaking
                peeking = false;

                if (overflow) { ClearPage(); } /// clear page if needed
            }

            #endregion TryFit
            #region continueloop
            do
            {
                // or add ifspace remaining check here per line?

                #region InnerLoop
                if (State == PState.Booting) yield return new WaitWhile(() => State == PState.Booting);

                Story.ContinueAsync(0); /// advance a bit 
                string newLine = story.currentText; /// get the next line up until there
                //string newLine = story.Continue();

                yield return FeedText(newLine);   /// Parse the ink story for functions and text: run functions and display text
                //Debug.Log("Write a line: " + newLine);
                State = PState.Producing; /// transition to state


                /// Hele andere aanpak: gooi alles gewoon meteen visible, dan over de karakters itereren.

                if (VisibleCharacters < CurrentText.Length)
                {
                    VisibleCharacters = CurrentText.Length;
                    Debug.LogWarning("Not all characters were done.");
                }

                string backup = CurrentText; /// make backup
                CurrentText += NewText; ///add the new text, invisibly
                if (OverFlowTextBox.text.Length > 0) /// check for overflow
                {
                    CurrentText = backup; /// restore backup
                    ClearPage(); /// save page and clear it
                    CurrentText = NewText; /// add new text anyway

                }


                #region RevealLetters
                string message = string.Format("On speed {0} (base {1}) wrote:\nChar\t\tDelay\t\tIntended\t\tExtra", TextSpeedActual, TextSpeedPreset); // prepare console message
                int tagLevel = 0; ///int to remember if we go down any nested tags
                char letter;///prepare marker             
                while (VisibleCharacters < TextBox.text.Length) /// while not all characters are visible
                {
                    letter = TextBox.text[VisibleCharacters]; /// store letter we're typing letter   
                    VisibleCharacters++; /// show the character
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
                        if (StoryController.StateMachine.CurrentState!=StoryController.writingState) yield return new WaitUntil(() => StoryController.StateMachine.CurrentState != StoryController.writingState); /// don't continue if halted
                        if (!isActiveAndEnabled) yield return new WaitUntil(() => isActiveAndEnabled); /// only continue if enabled
                        //Debug.Log("show me " + letter);
                        float delay = 0; /// initialize delay
                        if (!Skipping) /// in normal behaviour
                        {
                            /// get the delay from the delay info object
                            delay = _pauseInfo.GetPause(letter) / TextSpeedActual;
                        }
                        else if (AlwaysPause) /// if this setting is enabled, 
                        {
                            ///get pause info while skipping text too
                            delay = _pauseInfoForSkips.GetPause(letter);
                        }
                        if (delay > 0) yield return new WaitForSecondsRealtime(delay); /// apply the delay if any
                        float delayMs = delay * 1000;
                        float delayActual = stopwatch.ElapsedMilliseconds;
                        message += string.Format("\n\'{0}\'\t\t{1} ms\t\t{2} ms\t\t {3} ms", letter, delayActual, delayMs, delayActual - delayMs);
                        stopwatch.Restart();
                    }
                }

                Debug.Log(message);
                #endregion RevealLetters

                State = PState.Idle;
                #endregion InnerLoop

                if (EncounteredStop)  /// if we encounter a stop
                {
                    EncounteredStop = false;
                    /// exit the loop or continue with a small delay
                    if (AutoAdvance)
                    {
                        yield return new WaitForFixedUpdate();
                    }
                    else
                    {
                        yield break;
                    }
                }
            } while (story.canContinue); /// while the story can continue without input on choices
            #endregion continueloop

            ResetSkipping();

        }

        #endregion
        // Peripheral
        #region Peripheral
        public enum TextSpeed
        {
            slow = 6,
            medium = 12,
            fast = 24
        }

        public enum PState //TODO: Encorporate into statemachine
        {
            Booting,
            Idle,
            Producing,
            Stuck
        }

        [Serializable]
        public class PauseInfo
        {
            public float _dotPause = .5f;
            public float _commaPause = .2f;
            public float _spacePause = .05f;
            public float _normalPause = .01f;

            public float GetPause(char letter)
            {
                float delay = letter switch
                {
                    '.' => _dotPause,
                    ',' => _commaPause,
                    ' ' => _spacePause,
                    _ => _normalPause,
                };
                return delay;
            }
        }

        #endregion
        // DEPRICATED
        bool GlueNext { get; set; } = false; // TODO: Incorporate into statemachine, if it is needed at all
        string catchAfterStop { get; set; } = ""; // TODO: Incorporate into statemachine, if it is still needed

        private string[] SplitIntoWords(string input)
        {
            string[] split = Regex.Split(input, @"(?<=[ \n])");
            /* werkt ook niet
            string pattern = "(/s)";

            string[] split = Regex.Split(input, pattern);    // Split on hyphens
            // The method writes the following to the console:
            //    'plum'
            //    '-'
            //    'pear'
            */
            /*werk niet
            Regex regex = new("/s"); // split on whitespace cahracters
            string[] split = regex.Split(input);
            */

            //string[] split = input.Split(' ');

            /*
            string message = "Following words:";
            foreach (string word in split)
            {
                message += "\n\"" + word + "\",";
            }
            Debug.Log(message);
            */

            return split.SkipLast(1).ToArray();
        }

        /* 
        
        public string[] NewWords { get; private set; }


        private void _ShowNextLetter()
        {
            if(State==PState.Producing | State == PState.Skipping)
            {
                ///if not readied all letters in this word
                if (letterIndex < CurrentWord.Length)
                {
                    /// get the letter
                    char letter = CurrentWord[letterIndex];
                    Debug.Log("show me " + letter);
                    ///Actualize on screen
                    textBox.maxVisibleCharacters++; //is this questionable?
                    letterIndex++; /// increment
                    float delay = PauseInfo.Pause(letter) * 1 / inkParser.TextSpeedActual;
                    this.DelayedAction(() =>
                    {
                        _ShowNextLetter(); /// continue loop at letter,
                    }, delay /// after delay
                , isActiveAndEnabled /// when conditions are met
                , !inkParser.Halted
                    );


                }
                else
                {
                    letterIndex = 0; /// reset letter index
                    _PlaceNextWord(); /// continue loop at word
                }
            }
            else
            {
                throw new Exception("Wrong state");
            }
        }
        private void _PlaceNextWord()
        {
            if (State == PState.Producing | State == PState.Skipping)
            {

                //if not readied all words in this string
                if (wordIndex < NewWords.Length)
                {
                    CurrentWord = NewWords[wordIndex];
                    Debug.Log("Write " + CurrentWord);
                    wordIndex++; /// increment
                    textBox.text += CurrentWord;   /// place word 
                    _ShowNextLetter(); /// continue loop at letter
                }
                else
                {
                    wordIndex = 0;
                    if(CurrentText == prospectedText)
                    {
                        Debug.Log("Done reproducing following text:\n" + string.Concat(NewWords));

                        State = PState.Idle;
                    }
                    else
                    {
                        State = PState.Stuck;
                        throw new Exception("Unable to accurately reproduce line.");
                    }
                }
            }
            else
            {
                throw new Exception("Wrong state");
            }
        }
        
        */

        // UNRESOLVED




    }
    
}
