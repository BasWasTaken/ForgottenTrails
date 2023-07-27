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
    /// <summary>
    /// <para>Summary not provided.</para>
    /// </summary>
    public class InkInputHandler : MonoBehaviour
    {
        // Inspector Properties
        #region Inspector Properties
        [field: SerializeField, BoxGroup("Prefabs"), Required]
        [Tooltip("Prefab used for ink choices.")]
        public Button ButtonPrefab { get; private set; }

        [field:SerializeField, BoxGroup("Scene References"), Required]
        public Image FloatingMarker { get; private set; }

        [field:SerializeField, BoxGroup("Settings")]
        [Tooltip("Delay after which space button advances dialogue.")]
        public float AdvanceDialogueDelay { get; private set; } = .1f;

        [field: SerializeField, BoxGroup("Settings")]
        [Tooltip("Delay after which space button skips new dialogue.")]
        public float SkipDelay { get; private set; } = .2f;
        #endregion
        // Public Properties
        #region Public Properties
        private bool _handleAdvancement = false;
        public bool HandleAdvancement
        {
            get { return _handleAdvancement; }
            protected set
            {
                _handleAdvancement = value;
                if (_handleAdvancement)
                {
                    // Debug.Log("Turning input on.");
                    if (PresentButtons()) // try to make buttons if any
                    {

                    }
                    else
                    {
                        FloatingMarker.gameObject.SetActive(true); // else set bouncing triangle at most recent line
                    }
                }
                else
                {
                    // Debug.Log("Turning input off.");
                    RemoveOptions(); // Destroy old choices
                    floatingMarker.gameObject.SetActive(false); // remove marker 
                }
            }
        }

        public float TimeSinceAdvance { get; private set; } = 0;


        #endregion
        // Private Properties
        #region Private Properties

        #endregion
        // MonoBehaviour LifeCycle Methods
        #region MonoBehaviour LifeCycle Methods
        private void Update()
        {
            TimeSinceAdvance += Time.unscaledDeltaTime; // TODO: Incorporate into statemachine, only advanving when appropriate
        }

        #endregion
        // Public Methods
        #region Public Methods

        #endregion
        // Private Methods
        #region Private Methods

        #endregion
    }
}
