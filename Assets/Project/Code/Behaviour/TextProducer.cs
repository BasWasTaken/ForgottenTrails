using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using Extensions;
using Ink.UnityIntegration;
using Ink.Runtime;

namespace ForgottenTrails 
{ 
    /// <summary>
    /// <para>Produces text onto ui elements for the player.</para>
    /// </summary>
    /// // from https://github.com/Tioboon/LogWritter/blob/main/EventController.cs
    public class TextProducer : MonoBehaviour
    {

        [SerializeField] 
        private TextMeshProUGUI textBox;
        [SerializeField]
        private TextMeshProUGUI overFlowTextBox;
        [SerializeField]
        private TextMeshProUGUI historyTextBox;
        [SerializeField] 
        private PauseInfo pauseInfo;

        [Tooltip("Delay after which space button advances dialogue.")]
        protected float advanceDialogueDelay = .1f;
        public float AdvanceDialogueDelay => advanceDialogueDelay;

        private string _finalText = "";
        private string[] _finalWords;
        public string FinalText
        {
            get { return _finalText; }
            set { 
                _finalText = value;
                _finalWords = value.Split(' ');
            }
        }
        public string[] FinalWords => _finalWords;
        public string NextWord => FinalWords[wordIndex];

        private string CurrentText => textBox.text;

        private int letterIndex;
        private int wordIndex;

        public bool DoneAndReady => CurrentText.Length == FinalText.Length & textBox.text.Length == textBox.maxVisibleCharacters;

        private bool TooMuchText => overFlowTextBox.text.Length > 0;

        #region Methods
        public void FeedText(string newText)
        {
            if (!DoneAndReady)
            {
                Debug.LogError("Text displayer is busy!");
                return;
            }
            else
            {
                ParseText(ref newText);
                TryFitText(newText); /// check size and clear page if needed
                FinalText += newText; /// add new text to target
                StartProducing();
            }
        }

        private string ParseText(ref string input)
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

            return output;
        }
        private void TryFitText(string newText)
        {
            string bufferText = CurrentText; // make backup
            textBox.text += newText; // add the text
            if (TooMuchText) // if overflow detected
            {
                textBox.text = bufferText; // restore backup
                ClearPage();
            }
        }

        public void ClearPage()
        {
            if (!DoneAndReady)
            {
                Debug.LogError("Text displayer is busy!");
                return;
            }
            else
            {
                historyTextBox.text = textBox.text;
                textBox.text = "";
                FinalText = "";
                wordIndex = 0;
            }
        }

        private void StartProducing()
        {
            ShowNextLetter();
        }

        private void ShowNextLetter()
        {
            ///if not readied all letters in this word
            if (letterIndex < NextWord.Length) 
            {
                /// get the letter
                char letter = NextWord[letterIndex];

                ///Actualize on screen
                textBox.maxVisibleCharacters++; //is this questionable?
                letterIndex++; /// increment
                float delay = letter switch
                {
                    '.' => pauseInfo.dotPause,
                    ',' => pauseInfo.commaPause,
                    ' ' => pauseInfo.spacePause,
                    _ => pauseInfo.normalPause,
                };
                this.DelayedAction(() =>
                {
                    ShowNextLetter(); /// continue loop at letter,
                }, delay); /// after delay
            }
            else
            {
                PlaceNextWord(); /// continue loop at word
            }
        }
        private void PlaceNextWord()
        {
            //if not readied all words in this string
            if (wordIndex < FinalWords.Length)
            {
                string word = NextWord;



                if (word =="<stop>") /// now check for stop command, as it could appear here too
                {
                    if (FinalWords.Length > wordIndex+1) /// throw error if there are still words remaining
                    {
                        Debug.Log("Only use <stop> at end of line!");
                        FinalText = FinalText.Remove(FinalText.IndexOf("<stop>"));
                        /// This also ends the loop since readyanddone should now evalaute true.
                    }
                }
                else  /// in the normal case
                {
                    textBox.text += word;   /// place word 
                    wordIndex++; /// increment
                    letterIndex = 0; /// reset letter index
                    ShowNextLetter(); /// continue loop at letter
                }
            }
            else
            {
                Debug.Log("Done reproducing!");
            }
            
        }

        #endregion

    }
    [Serializable]
    public class PauseInfo
    {
        public float dotPause = .5f;
        public float commaPause = .2f;
        public float spacePause = .05f;
        public float normalPause = .01f;
    }
}
