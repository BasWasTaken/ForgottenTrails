using NaughtyAttributes;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using VVGames.Common;
using Debug = UnityEngine.Debug;

namespace VVGames.ForgottenTrails.InkConnections
{
    public partial class StoryController : MonoSingleton<StoryController>
    {
        #region Classes

        [Serializable]
        public partial class TextProduction
        {
            // Inspector Properties

            #region Fields

            [SerializeField]
            internal TextSpeed _textSpeedPreset;

            internal int maxVis = 20;

            private StoryController Controller;

            #endregion Fields

            #region Enums

            /// <summary>
            /// base text speed in characters per second
            /// </summary>
            public enum TextSpeed
            {
                sluggish = 1,
                extraSlow = 12,
                slow = 24,
                medium = 48,
                fast = 96,
                extraFast = 480,
                bonkers = 12000000
            }

            #endregion Enums

            #region Properties

            public TextSpeed TextSpeedPreset
            {
                get
                { return _textSpeedPreset; }
                set
                {
                    //Debug.Log(string.Format("Changed from {0} to {1} speed", TextSpeedPreset.ToString(), value.ToString()));
                    _textSpeedPreset = value;
                    PlayerPrefs.SetInt("textSpeed", (int)_textSpeedPreset);
                }
            }

            [field: SerializeField, BoxGroup("Settings")]
            public bool AutoAdvance { get; internal set; } = false;

            [field: SerializeField, BoxGroup("Settings")]
            public bool StillPauseWhileSkipping { get; internal set; } = true;

            public float TextSpeedMod { get; private set; }

            // Public Properties
            public float TextSpeedActual => ((float)TextSpeedPreset) * TextSpeedMod;

            public string CurrentText
            {
                get
                {
                    return TextBox.text;
                }
                internal set
                {
                    if (value == null) value = "";
                    TextBox.text = value;
                }
            }

            public int VisibleCharacters
            {
                get
                {
                    return TextBox.maxVisibleCharacters;
                }
                internal set
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
                internal set
                {
                    HistoryTextBox.text = value;
                }
            }

            [field: SerializeField, BoxGroup("Scene References"), Required]
            [Tooltip("Panel to collect overflow text.")]
            internal TextMeshProUGUI OverFlowTextBox { get; set; }

            [field: SerializeField, BoxGroup("Scene References"), Required]
            [Tooltip("Panel to display previous text.")]
            internal TextMeshProUGUI HistoryTextBox { get; set; }

            [field: SerializeField, Header("Settings"), BoxGroup("Settings")]
            [Tooltip("Define pause timings here.")]
            internal PauseSettings Pauses { get; set; }

            [field: SerializeField, Header("Scene References"), BoxGroup("Scene References"), Required]
            [Tooltip("Panel to display current paragraph.")]
            private TextMeshProUGUI TextBox { get; set; }

            [field: SerializeField]
            private AudioSource typingSound { get; set; } = new();

            #endregion Properties

            // Private Properties
            // Constructor

            #region Public Methods

            public void ClearPage()
            {
                if (Controller.TextProducer.Peeking) return; // not if we're just peeking
                //Debug.Log("Clearing " +CurrentText);
                HistoryTextBox.text += CurrentText; // move all text to the history log
                CurrentText = ""; // clear current and prospective texts
                VisibleCharacters = 0;
            }

            #endregion Public Methods

            #region Internal Methods

            internal void Assign()
            {
                Controller = Instance;
            }

            // Public Methods
            // Private Methods

            internal void Init(string cur, string his)
            {
                CurrentText = cur;
                VisibleCharacters = cur.Length;
                PreviousText = his;
            }

            internal void Spd(float speed)
            {
                TextSpeedMod = speed;
            }

            internal int SetMaxLines()
            {
                string backup = Controller.TextProducer.CurrentText;
                Controller.TextProducer.CurrentText = "";
                maxVis = 0;
                while (!Controller.TextProducer.TextBox.isTextOverflowing)
                {
                    maxVis++;
                    Controller.TextProducer.CurrentText += '\n';
                    if (maxVis > 1000) break;
                }
                Controller.TextProducer.CurrentText = backup;
                Debug.Log(string.Format("fitted {0} lines in the text box!", maxVis));
                return maxVis;
            }

            #endregion Internal Methods

            #region Private Methods

            private string[] SplitIntoWords(string input)
            {
                string[] split = Regex.Split(input, @"(?<=[ \n])");

                return split.SkipLast(1).ToArray();
            }

            #endregion Private Methods
        }

        #endregion Classes
    }
}