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
        #endregion
        // Private Properties
        #region Private Properties

        #endregion
        // MonoBehaviour Events
        #region MonoBehaviour Events

        #endregion
        // Public Methods
        #region Public Methods

        #endregion
        // Private Methods
        #region Private Methods

        #endregion
    }
}
