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
        public string CurrentlyDisplayed => textBox.text;
        [BoxGroup("Scene References"), Required, SerializeField]
        [Tooltip("Panel to collect overflow text.")]
        private TextMeshProUGUI overFlowTextBox;
        [BoxGroup("Scene References"), Required, SerializeField]
        [Tooltip("Panel to display previous text.")]
        private TextMeshProUGUI historyTextBox;
        public string PreviouslyDisplayed => historyTextBox.text;
        [BoxGroup("Settings"), SerializeField]
        [Tooltip("Define pause timings here.")]
        private PauseInfo _pauseInfo = new() 
        {
            _dotPause = .5f,
            _commaPause = .2f,
            _spacePause = .05f,
            _normalPause = .01f
        };
        private PauseInfo _noPause = new()
        {
            _dotPause = 0.001f,
            _commaPause = 0.001f,
            _spacePause = 0.001f,
            _normalPause = 0.001f
        };
        public PauseInfo PauseInfo 
        { 
            get
            {
                return state == State.Skipping ? _noPause : _pauseInfo;
            } 
        }
        public void SkipLine() 
        {
            if (state == State.Producing)
            {
                Debug.Log("Skip this line!");
                state = State.Skipping;
            }
            else
            {
                Debug.LogWarning("Cannot skip right now.");
            }
        }

        private string prospectedText = "";
        public string[] NewWords { get; private set; }

        private string[] SplitIntoWords(string input)
        {
            Regex regex = new("/s+"); // split on whitespace cahracters
            string[] split = regex.Split(input);
            return split;
        }
        public string CurrentWord = "";

        private string CurrentText => textBox.text;

        private int letterIndex;
        private int wordIndex;
        public enum State 
        { 
            Booting,
            Idle,
            Producing,
            Skipping,
            Stuck
        }
        private State state = State.Booting;
        public State GetState => state;
        public bool IsWorking => state == State.Producing | state == State.Skipping;

        #region Methods
        private void Awake()
        {
            textBox.text = ""; //clear lorum ipsum
            textBox.maxVisibleCharacters = 0;
            historyTextBox.text = ""; //clear lorum ipsum
            inkParser = GetComponent<InkParser>();
            state = State.Idle;
        }
        public void FeedText(string newText)
        {
            if (state != State.Idle)
            {
                Debug.LogError(string.Format("Text displayer is {0}", state.ToString()));

                return;
            }
            else
            {
                string parsedText = ParseText(newText);
                TryFitText(parsedText); /// check size and clear page if needed

                NewText = parsedText; /// add new text to target
            }
        }
        private string ParseText(string input)
        {
            string output = "";
            if (input == "<br>\n" | input == "<br>") /// when hitting explicit linebreak //which is it?
            {
                output += "\n"; //add a newline
            }
            else if (input.StartsWith("...")) /// when hitting agreed upon syntax for advanced glueing
            {
                textBox.text = textBox.text.TrimEnd('\n') + ' '; /// replace linebreak by space

                output += input.TrimStart('.'); /// and remove the syntax
            }
            /*
            else if (newLine.StartsWith(">>"))
            {
                PlaySfx(newLine.Split(">>")[1].TrimEnd('\n').TrimEnd(' ').ToLower());
            }
            */
            else
            {
                output += input; /// leave the input unaltered
            }
            /* Depricated: no longer using tags
            /// check for tags:
            foreach (string tag in story.currentTags)
            {
                Debug.Log(tag);
                DoFunction(tag);
            }
            */


            Regex tags = new(@"\<([^>]+)\>");

            foreach (Match match in tags.Matches(output))
            {
                Debug.Log(string.Format("Encountered tag {0} in {1}",match.Value,output));

                if (match.Value=="<stop>")
                {
                    Debug.Log("recognised stop tag");
                    inkParser.encounteredStop = true;
                }
                output = output.Replace(match.Value, "");
            } 

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

            Debug.Log("Write line as: " + output);
            return output;

        }
        private string NewText
        {
            set
            {
                NewWords = SplitIntoWords(value);
                prospectedText = CurrentText + string.Concat(NewWords);
                StartProducing();
            }
        }

        private void TryFitText(string newText)
        {
            string bufferText = CurrentText; /// make backup
            textBox.text += newText; /// test if the text will fit
            bool overflow = overFlowTextBox.text.Length > 0; /// store result

            textBox.text = bufferText; /// restore backup

            if (overflow) ClearPage(); /// clear page if needed

        }

        public void ClearPage()
        {
            if (state != State.Idle)
            {
                Debug.LogError("Text displayer is busy!");
                return;
            }
            else
            {
                historyTextBox.text = textBox.text; /// move all text to the history log
                textBox.text = NewText = ""; /// clear current and prospective texts
                wordIndex = letterIndex = textBox.maxVisibleCharacters = 0; /// reset indexes of word and letter iteration
            }
        }

        private void StartProducing()
        {
            if(state != State.Producing)
            {
                state = State.Producing;
                ShowNextLetter();
            }
            else
            {
                throw new Exception("Wrong state");
            }
        }

        private void ShowNextLetter()
        {
            if(state==State.Producing | state == State.Skipping)
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
                        ShowNextLetter(); /// continue loop at letter,
                    }, delay /// after delay
                , isActiveAndEnabled /// when conditions are met
                , !inkParser.Halted
                    );


                }
                else
                {
                    letterIndex = 0; /// reset letter index
                    PlaceNextWord(); /// continue loop at word
                }
            }
            else
            {
                throw new Exception("Wrong state");
            }
        }
        private void PlaceNextWord()
        {
            if (state == State.Producing | state == State.Skipping)
            {

                //if not readied all words in this string
                if (wordIndex < NewWords.Length)
                {
                    CurrentWord = NewWords[wordIndex];
                    Debug.Log("Write " + CurrentWord);
                    wordIndex++; /// increment
                    textBox.text += CurrentWord;   /// place word 
                    ShowNextLetter(); /// continue loop at letter
                }
                else
                {
                    wordIndex = 0;
                    if(CurrentText == prospectedText)
                    {
                        Debug.Log("Done reproducing following text:\n" + string.Concat(NewWords));

                        state = State.Idle;
                    }
                    else
                    {
                        state = State.Stuck;
                        throw new Exception("Unable to accurately reproduce line.");
                    }
                }
            }
            else
            {
                throw new Exception("Wrong state");
            }
        }
        #endregion



    }
    [Serializable]
    public class PauseInfo
    {
        public float _dotPause = .5f;
        public float _commaPause = .2f;
        public float _spacePause = .05f;
        public float _normalPause = .01f;

        public float Pause(char letter)
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
