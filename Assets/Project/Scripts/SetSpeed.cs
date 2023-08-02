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
        private StoryController Controller;

        #region LIFESPAN
        private void Awake()
        {/*
            Controller = FindObjectOfType<StoryController>();
            dropdown = GetComponent<TMP_Dropdown>();
            Controller.TextSpeed storedValue = (Controller.TextSpeed)PlayerPrefs.GetInt("textSpeed");
            var value = storedValue switch
            {
                Controller.TextSpeed.slow => 0,
                Controller.TextSpeed.medium => 1,
                Controller.TextSpeed.fast => 2,
                _ => 1,
            };
            dropdown.value = value;
            */
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
        {/*
            var speed = value switch
            {
                0 => Controller.TextSpeed.slow,
                1 => Controller.TextSpeed.medium,
                2 => Controller.TextSpeed.fast,
                _ => Controller.TextSpeed.medium,
            };
            PlayerPrefs.SetInt("textSpeed", (int)speed);*/
        }
        #endregion
    }
}
