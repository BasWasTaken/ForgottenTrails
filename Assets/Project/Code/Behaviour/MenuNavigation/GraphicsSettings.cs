using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

namespace TEMPLATENAMESPACE 
{ 
    /// <summary>
    /// <para>SUMMARY GOES HERE.</para>
    /// </summary>
    public class GraphicsSettings : MonoBehaviour
    {
        ///___VARIABLES___///
        #region INSPECTOR
        public TMP_Dropdown resolutionDropdown;
        #endregion
        #region BACKEND_VARIABLES
        Resolution[] resolutions;

        #endregion
        ///___METHODS___///
        #region LIFESPAN
        private void Awake()
        {
            
        }
        private void Start()
        {
            resolutions = Screen.resolutions;
            resolutionDropdown.ClearOptions();
            List<string> options = new();

            int curIndex = 0;
            int i = 0;
            foreach (Resolution resolution in resolutions)
            {
                string option = resolution.width + " x " + resolution.height;
                options.Add(option);

                if(resolution.width==Screen.currentResolution.width && resolution.height == Screen.currentResolution.height)
                {
                    curIndex = i;
                }
                i++;
            }
            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = curIndex;
            resolutionDropdown.RefreshShownValue();
        }
        #endregion
        #region LOOP
        private void Update()
        {
            
        }
        #endregion
        #region OTHER_METHODS
        public void SetQuality(int value)
        {
            QualitySettings.SetQualityLevel(value);
        }
        public void SetResolution(int resolutionIndex)
        {
            Resolution res = resolutions[resolutionIndex];
            Screen.SetResolution(res.width, res.height, Screen.fullScreen);
        }
        public void SetFullscreen(bool value)
        {
            Screen.fullScreen = value;
        }
        #endregion
    }
}

