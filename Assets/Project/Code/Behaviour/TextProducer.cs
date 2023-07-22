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

namespace ForgottenTrails 
{ 
    /// <summary>
    /// <para>Produces text onto ui elements for the player.</para>
    /// </summary>
    /// // from https://github.com/Tioboon/LogWritter/blob/main/EventController.cs
    public class TextProducer : MonoBehaviour
    {
        private InkParser inkParser;

        [BoxGroup("Scene References"), Required, SerializeField]
        [Tooltip("Panel to display current paragraph.")]
        private TextMeshProUGUI textBox;
        public string CurrentText
        { 
            get 
            { 
                return textBox.text; 
            }
            private set 
            {
                textBox.text = value;
            }
        }
        public int CurrentlyVisible
        {
            get
            {
                return textBox.maxVisibleCharacters;
            }
            private set
            {
                textBox.maxVisibleCharacters = value;
            }
        }

        /*
        public string CurrentlyDisplayed
        {
            get
            {
                return textBox.text.Substring(0,textBox.maxVisibleCharacters); /// show only characters that are visible
            }
            private set
            {
                textBox.text = value;
                textBox.maxVisibleCharacters = value.Length;
                //Debug.Log("TextBox value set manually!");
            }
        }*/
        public bool Skipping { get; private set; }
        [BoxGroup("Scene References"), Required, SerializeField]
        [Tooltip("Panel to collect overflow text.")]
        private TextMeshProUGUI overFlowTextBox;
        [BoxGroup("Scene References"), Required, SerializeField]
        [Tooltip("Panel to display previous text.")]
        private TextMeshProUGUI historyTextBox;
        public string PreviouslyDisplayed { get { return historyTextBox.text; } private set { historyTextBox.text = value; } }
        [BoxGroup("Settings"), SerializeField]
        [Tooltip("Define pause timings here.")]
        private PauseInfo _pauseInfo = new() 
        {
            _dotPause = .2f,
            _commaPause = .1f,
            _spacePause = .02f,
            _normalPause = .01f
        };
        [BoxGroup("Settings"), SerializeField]
        [Tooltip("Define tiny timings here for when skipping text.")]
        private PauseInfo _pauseInfoForSkips = new()
        {
            _dotPause = 0.000000005f,
            _commaPause = 0.000000002f,
            _spacePause = 0.0000000005f,
            _normalPause = 0.0000000001f
        };
        public PauseInfo PauseInfo 
        { 
            get
            {
                return Skipping ? _pauseInfoForSkips : _pauseInfo;
            } 
        }
        private Story story => inkParser.story;

        public void SkipLines() 
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

        //private string prospectedText = "";
        public string[] NewWords { get; private set; }


        //public string CurrentWord = "";

        //private int letterIndex;
        //private int wordIndex;
        public enum PState 
        { 
            Booting,
            Idle,
            Producing,
            Stuck
        }
        private PState _state = PState.Booting;
        public PState State
        {
            get { return _state; }
            private set
            {
                if(_state == value)
                {
                    Debug.LogWarning("Redundant state transition");
                }
                _state = value;
            }
        }
        public bool IsWorking => State == PState.Producing;

        #region Methods
        private void Awake()
        {
            CurrentText = ""; //clear lorum ipsum
            CurrentlyVisible = 0;
            PreviouslyDisplayed = ""; //clear lorum ipsum
            inkParser = GetComponent<InkParser>();
            State = PState.Idle;
            Skipping = false;
        }
        #region Text Placement
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
        public bool encounteredStop = false;

        bool GlueNext = false;
        string catchAfterStop = "";
        private string ParseText(string input)
        {
            

            Regex tags = new(@"\{([^>]+)\}");

            foreach (Match match in tags.Matches(input))
            {
                //Debug.Log(string.Format("Encountered command {0} in {1}",match.Value,output));

                if (match.Value == "{stop}")
                {
                    //Debug.Log("recognised stop command");
                    encounteredStop = true;
                    //catchAfterStop = input.Substring(match.Index+match.Value.Length);

                    //input = input.Substring(0, match.Index); 
                }
                else if(match.Value == "{glue}")
                {
                    input = input.TrimEnd('\n'); /// remove linebreak from this
                }
                else if (match.Value == "{aglue}")
                {
                    textBox.text = textBox.text.TrimEnd('\n'); /// remove linebreak from previous
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

            #region depricated
            /* Depricated: no longer using tags
            /// check for tags:
            foreach (string tag in story.currentTags)
            {
                Debug.Log(tag);
                DoFunction(tag);
            }
            */




            /* from when this code was part of the wordsplitter
            /// break down the input into words
            Regex tags = new(@"\<([^>]+)\>");
            List<string> output = new();

            foreach (string word in split)
            {
                if (tags.IsMatch(word))
                {
                    Debug.Log("found tag in " + word);
                    output.Add(tags.Replace(word, ""));
  
                    //output.Add(tags.Match(word).Value);
                }
                else
                {
                    output.Add(word);
                }
            }
            */
            #endregion depricated

            //Debug.Log("Write line as: " + output);
            return output;

        }

        public bool peeking;

        public void ClearPage()
        {
            if (peeking) return;
            if (State == PState.Producing)
            {
                throw new("Text displayer is busy!");
            }
            else
            {
                historyTextBox.text = CurrentText; /// move all text to the history log
                CurrentText = "";//prospectedText = ""; /// clear current and prospective texts
                CurrentlyVisible = 0;
            }
        }
        public bool autoAdvance = false;
        public bool AlwaysPause = false;
        private string _newText;
        private string NewText
        {
            get { return _newText; }
            set
            {
                //NewWords = SplitIntoWords(value);
                _newText = value;//string.Concat(NewWords); 
                //prospectedText = CurrentlyWritten + _newText;
            }
        }
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
        #endregion
        bool withinTag = false;

        bool autoClear = true;
        #region Typing
        /// <summary>
        /// Runs the per line steps required for parsing and showing ink story
        /// </summary>
        public IEnumerator ProduceTextOuter()
        {

            #region TryFit
            Stopwatch stopwatch = new();
            if (autoClear)
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
                CurrentlyVisible = textBox.text.Length;

                CurrentText += toBeAdded + '\n'; /// test if the text will fit
                yield return 0;/// wait 1 frame
                bool overflow = overFlowTextBox.text.Length > 0; /// store result

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

                story.ContinueAsync(0); /// advance a bit 
                string newLine = story.currentText; /// get the next line up until there
                //string newLine = story.Continue();

                yield return FeedText(newLine);   /// Parse the ink story for functions and text: run functions and display text
                //Debug.Log("Write a line: " + newLine);
                State = PState.Producing; /// transition to state


                /// Hele andere aanpak: gooi alles gewoon meteen visible, dan over de karakters itereren.

                if(CurrentlyVisible < CurrentText.Length) 
                {
                    CurrentlyVisible = CurrentText.Length;
                    Debug.LogWarning("Not all characters were done.");
                }
                
                string backup = CurrentText; /// make backup
                CurrentText += NewText; ///add the new text, invisibly
                if(overFlowTextBox.text.Length > 0) /// check for overflow
                {
                    CurrentText = backup; /// restore backup
                    ClearPage(); /// save page and clear it
                    CurrentText = NewText; /// add new text anyway

                }


                #region RevealLetters
                string message = "Wrote:\nChar\t\tDelay\t\tIntended\t\tExtra"; /// prepare console message
                int tagLevel = 0; ///int to remember if we go down any nested tags
                char letter;///prepare marker             
                while (CurrentlyVisible < textBox.text.Length) /// while not all characters are visible
                {
                    letter = textBox.text[CurrentlyVisible]; /// store letter we're typing letter   
                    CurrentlyVisible++; /// show the character
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
                        if (inkParser.Halted) yield return new WaitWhile(() => inkParser.Halted); /// don't continue if halted
                        if (!isActiveAndEnabled) yield return new WaitUntil(() => isActiveAndEnabled); /// only continue if enabled
                        //Debug.Log("show me " + letter);
                        float delay = 0; /// initialize delay
                        if (!Skipping) /// in normal behaviour
                        {
                            /// get the delay from the delay info object
                            delay = _pauseInfo.GetPause(letter) / inkParser.TextSpeedActual;
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

                #region Depricated
                /* DEPRICATED FROM HERE

                //TEMP I think I can just pre write all the words?
                CurrentlyWritten += NewText;

                wordIndex = 0; /// reset letter index
                while (wordIndex < NewWords.Length) /// for each word in the list 
                {
                    #region ProduceWord
                    CurrentWord = NewWords[wordIndex];

                    //Debug.Log("Write " + CurrentWord);
                    //TEMP don't do this, i do it all at once before CurrentlyWritten += CurrentWord;   /// place word 

                    letterIndex = 0; /// reset letter index\
                    
                    string message = "Wrote:\nChar\t\tDelay\t\tIntended\t\tExtra";
                    while (letterIndex < CurrentWord.Length) /// for each letter in that word
                    {
                        char letter = CurrentWord[letterIndex]; /// get the letter
                        textBox.maxVisibleCharacters++; /// actualize on screen //is this questionable?
                        letterIndex++; /// increment
                        #region RevealLetter   
                        if (withinTag) /// if within a tag, just check for the end
                        {
                            if (letter == '>') withinTag = false;
                        }
                        else if (letter == '<') /// if we entered a tag, mark 
                        {
                            withinTag = true;
                        }
                        else /// else apply potential delays
                        {
                            if (!isActiveAndEnabled) yield return new WaitUntil(() => isActiveAndEnabled);
                            if (inkParser.Halted) yield return new WaitWhile(() => inkParser.Halted);
                            //Debug.Log("show me " + letter);
                            float delay = 0;
                            if (!Skipping)
                            {
                                delay = _pauseInfo.GetPause(letter) * 1f / inkParser.TextSpeedActual; ;
                            }
                            else if (AlwaysPause)
                            {
                                delay = _pauseInfoForSkips.GetPause(letter); //do(n't) add time even while skipping
                            }
                            if (delay > 0) yield return new WaitForSecondsRealtime(delay);
                            float delayMs = delay * 1000;
                            float delayActual = stopwatch.ElapsedMilliseconds;
                            message +=string.Format("\n\'{0}\'\t\t{1} ms\t\t{2} ms\t\t {3} ms", letter, delayActual, delayMs, delayActual- delayMs);
                            stopwatch.Restart();
                        }

                        #endregion RevealLetter

                    }
                    Debug.Log(message);
                    letterIndex = 0; /// reset letter index
                    wordIndex++; /// increment word
                    #endregion ProduceWord
                }
                wordIndex = 0; /// reset letter index

                if (CurrentlyWritten != prospectedText) /// check success
                {
                    Debug.Log("Is that right..?");
                    if (CurrentlyDisplayed != prospectedText)
                    {
                        string message = "Unable to accurately reproduce line.";
                        message += string.Format("\nExpected:\n{0}\nGot:\n{1}\nTrue:\n{2}", prospectedText, CurrentlyDisplayed, CurrentlyWritten);
                        State = PState.Stuck;
                        throw new Exception(message);
                    }
                }
                //Debug.Log("Done reproducing following text:\n" + string.Concat(NewWords));
                */
                #endregion Depricated
                
                State = PState.Idle;
                #endregion InnerLoop

                if (encounteredStop)  /// if we encounter a stop
                {
                    encounteredStop = false;
                    /// exit the loop or continue with a small delay
                    if (autoAdvance)
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
        #endregion Typing
        /* DEPRICATED
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
        #endregion



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
}
