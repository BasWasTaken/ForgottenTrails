using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ForgottenTrails;
using UnityEngine.UI;
using TMPro;
using Bas.Utility;

namespace ForgottenTrails.InkFacilitation
{ 
    /// <summary>
    /// <para>SUMMARY GOES HERE.</para>
    /// </summary>
    public class SetSpeed : MonoBehaviour
    {
        private TMP_Dropdown dropdown;

        #region LIFESPAN
        private void Awake()
        {
            dropdown = GetComponent<TMP_Dropdown>();            
        }
        private void Start()
        {
            StoryController.TextProduction.TextSpeed storedValue = (StoryController.TextProduction.TextSpeed)PlayerPrefs.GetInt("textSpeed");
            var value = storedValue switch
            {
                StoryController.TextProduction.TextSpeed.slow => 0,
                StoryController.TextProduction.TextSpeed.medium => 1,
                StoryController.TextProduction.TextSpeed.fast => 2,
                _ => 1,
            };
            dropdown.value = value;
        }
        #endregion
        #region OTHER_METHODS
        public void SetSpeedTo(int value)
        {
            var speed = value switch
            {
                0 => StoryController.TextProduction.TextSpeed.slow,
                1 => StoryController.TextProduction.TextSpeed.medium,
                2 => StoryController.TextProduction.TextSpeed.fast,
                _ => StoryController.TextProduction.TextSpeed.medium,
            };
            PlayerPrefs.SetInt("textSpeed", (int)speed);
        }
        #endregion
    }
}
