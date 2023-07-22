using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ForgottenTrails;
using UnityEngine.UI;
using TMPro;

namespace TEMPLATENAMESPACE 
{ 
    /// <summary>
    /// <para>SUMMARY GOES HERE.</para>
    /// </summary>
    public class SetSpeed : MonoBehaviour
    {
        ///___VARIABLES___///
        #region INSPECTOR
        private TMP_Dropdown dropdown;
        #endregion
        #region BACKEND_VARIABLES

        #endregion
        ///___METHODS___///
        #region LIFESPAN
        void Awake()
        {
            dropdown = GetComponent<TMP_Dropdown>();
            InkParser.TextSpeed storedValue = (InkParser.TextSpeed)PlayerPrefs.GetInt("textSpeed");
            var value = storedValue switch
            {
                InkParser.TextSpeed.slow => 0,
                InkParser.TextSpeed.medium => 1,
                InkParser.TextSpeed.fast => 2,
                _ => 1,
            };
            dropdown.value = value;
        }
        private void Start()
        {
            
        }
        #endregion
        #region LOOP
        private void Update()
        {
            
        }
        #endregion
        #region OTHER_METHODS
        public void SetSpeedTo(int value)
        {
            var speed = value switch
            {
                0 => InkParser.TextSpeed.slow,
                1 => InkParser.TextSpeed.medium,
                2 => InkParser.TextSpeed.fast,
                _ => InkParser.TextSpeed.medium,
            };
            PlayerPrefs.SetInt("textSpeed", (int)speed);
        }
        #endregion
    }
}
