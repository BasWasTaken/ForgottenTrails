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
    public partial class StoryController : MonoSingleton<StoryController>
    {
        [Serializable]
        /// <summary>
        /// <para>Produces text onto ui elements for the player.</para>
        /// </summary>
        /// delay effect from https://github.com/Tioboon/LogWritter/blob/main/EventController.cs
        public partial class TextProduction
        {
            // Inspector Properties
            #region Inspector Properties
            [field: SerializeField, Header("Scene References"), BoxGroup("Scene References"), Required]
            [Tooltip("Panel to display current paragraph.")]
            private TextMeshProUGUI TextBox { get; set; }

            [field: SerializeField, BoxGroup("Scene References"), Required]
            [Tooltip("Panel to collect overflow text.")]
            internal TextMeshProUGUI OverFlowTextBox { get; set; }

            [field: SerializeField, BoxGroup("Scene References"), Required]
            [Tooltip("Panel to display previous text.")]
            internal TextMeshProUGUI HistoryTextBox { get; set; }
            [field: SerializeField]
            AudioSource typingSound { get; set; } =            new();
            [SerializeField]
            internal TextSpeed _textSpeedPreset;
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
            [field: SerializeField, Header("Settings"), BoxGroup("Settings")]
            [Tooltip("Define pause timings here.")]
            internal PauseInfo Pauses { get; set; } = new();
            [field: SerializeField, BoxGroup("Settings")]
            public bool AutoAdvance { get; internal set; } = false;
            [field: SerializeField, BoxGroup("Settings")]
            public bool StillPauseWhileSkipping { get; internal set; } = true;
            #endregion
            // Public Properties
            #region Public Properties
            public float TextSpeedMod { get; private set; }
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

            #endregion
            // Private Properties
            #region Private Properties
            StoryController Controller;
            #endregion
            // Constructor
            #region Constructor
            internal void Assign()
            {
                Controller = Instance;
            }
            #endregion
            // Public Methods
            #region Public Methods
            public void ClearPage()
            {
                if (Controller.TextProducer.Peeking) return;
                Debug.Log("Clearing " +CurrentText);
                HistoryTextBox.text += CurrentText; // move all text to the history log
                CurrentText = ""; // clear current and prospective texts
                VisibleCharacters = 0;
            }
            
            #endregion
            // Private Methods
            #region Private Methods
            internal void Init(string cur, string his)
            {
                CurrentText = cur;
                VisibleCharacters = 0;
                PreviousText = his;
            }
            internal void Spd(float speed)
            {
                TextSpeedMod = speed;
            }


            #endregion
            // Peripheral
            #region Peripheral
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


            [Serializable]
            public class PauseInfo
            {
                // relatie delays
                public float _dotPause;// = 2.5f;
                public float _commaPause;// = 2f;
                public float _spacePause;// = 1.5f;
                [ReadOnly,MinValue(1), MaxValue(1)]
                public float _normalPause = 1f; // should stay at 1

                public float GetPause(char letter) // TODO: make this a scriptable object so that chagnes persist? and the nyou can also make the settings hotswapapble
                {
                    float delay = letter switch
                    {
                        '.' => _dotPause,
                        ':' => _dotPause,
                        ',' => _commaPause,
                        ';' => _commaPause,
                        ' ' => _spacePause,
                        '\t' => _spacePause,
                        '\n' => _spacePause,
                        _ => _normalPause,
                    };
                    return delay;
                }
            }

            #endregion
            // DEPRICATED
            
            private string[] SplitIntoWords(string input)
            {
                string[] split = Regex.Split(input, @"(?<=[ \n])");

                return split.SkipLast(1).ToArray();
            }

        }
    }
    
}
