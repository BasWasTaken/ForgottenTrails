using DataService;
using Ink.Runtime;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Bas.Utility;

namespace ForgottenTrails.InkFacilitation
{
    public partial class StoryController : MonoSingleton<StoryController>
    {
        [Serializable]
        /// <summary>
        /// <para>Summary not provided.</para>
        /// </summary>
        public partial class InterfaceBroking
        {
            // Inspector Properties
            #region Inspector Properties
            [field: SerializeField, BoxGroup("Prefabs"), Required]
            [Tooltip("Prefab used for ink choices.")]
            public Button ButtonPrefab { get; internal set; }

            [field: SerializeField, BoxGroup("Scene References"), Required]
            internal Transform ButtonAnchor { get; set; }

            [field: SerializeField, BoxGroup("Scene References"), Required]
            public Image FloatingMarker { get; internal set; }

            [field: SerializeField, BoxGroup("Settings")]
            [Tooltip("Delay after which space button advances dialogue.")]
            public float AdvanceDialogueDelay { get; internal set; } = .1f;

            [field: SerializeField, BoxGroup("Settings")]
            [Tooltip("Delay after which space button skips new dialogue.")]
            public float SkipDelay { get; internal set; } = .2f;
            #endregion
            // Public Properties
            #region Public Properties



            #endregion
            // Private Properties
            #region Private Properties
            private StoryController Controller;
            #endregion
            // Constructor
            #region Constructor
            internal InterfaceBroking(StoryController controller)
            {
                Controller = controller;
            }
            #endregion
            // Public Methods
            #region Public Methods
            public bool CanPresentChoices()
            {
                if (Controller.Story.canContinue)
                {
                    //Debug.Log("no choices detected at this point");
                    return false;
                }
                else if (Controller.Story.currentChoices.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false; // technically, if ending the dialogue is a choise
                }
            }
            #endregion
            // Private Methods
            #region Private Methods
            
            #endregion
        }
    }    
}
