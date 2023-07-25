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

namespace ForgottenTrails
{
    /// <summary>
    /// <para>Works in tandem with <see cref="InkParser"/> to populate scenes with assets and effects as dictated in <see cref="Ink.Runtime.Story"/> assets.</para>
    /// </summary>
    public class SetDresser : MonoBehaviour
    {
        // Inspector Properties
        #region Inspector Properties

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
        // UNRESOLVED
        // Variables
        [SerializeField, BoxGroup("Prefabs"), Required]
        private Image portraitPrefab = null;
        [SerializeField, BoxGroup("Scene References"), Required]
        private HorizontalLayoutGroup portraits;

        [SerializeField, BoxGroup("Scene References"), Required]
        private BackGround bgImage;

    }
}
