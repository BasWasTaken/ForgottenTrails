using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using Extensions;

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
        public string CurrentWord => FinalWords[wordIndex];

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
                string bufferText = CurrentText; // make backup
                textBox.text += newText; // add the text
                if (TooMuchText) // if overflow detected
                {
                    textBox.text = bufferText; // restore backup
                    ClearPage();
                }
                FinalText += newText;
                ShowNextLetter();
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
                letterIndex = 0;
            }
        }

        private void ShowNextLetter()
        {
            ///if not readied all letters in this word
            if (letterIndex < CurrentWord.Length) 
            {
                /// get the letter
                char letter = CurrentWord[letterIndex];

                ///Actualize on screen
                textBox.maxVisibleCharacters++; //is this questionable?
                letterIndex++;
                float delay = letter switch
                {
                    '.' => pauseInfo.dotPause,
                    ',' => pauseInfo.commaPause,
                    ' ' => pauseInfo.spacePause,
                    _ => pauseInfo.normalPause,
                };
                this.DelayedAction(() =>
                {
                    ShowNextLetter();
                }, delay);
            }
            else
            {
                PlaceNextWord();
            }
        }
        private void PlaceNextWord()
        {
            //if not readied all words in this string
            if (wordIndex < FinalWords.Length)
            {
                string nextWord = FinalWords[wordIndex];
                textBox.text += nextWord;
                wordIndex++;
                letterIndex = 0;
            }
            else
            {
                Debug.Log("Done reproducing!");
            }
        }
        //make canreproduce{advance} loop?
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
