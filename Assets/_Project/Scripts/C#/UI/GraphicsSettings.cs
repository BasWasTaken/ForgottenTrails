using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Bas.ForgottenTrails.UI
{
    /// <summary>
    /// <para>SUMMARY GOES HERE.</para>
    /// </summary>
    public class GraphicsSettings : MonoBehaviour
    {
        ///___VARIABLES___///

        #region Fields

        public TMP_Dropdown resolutionDropdown;

        private Resolution[] resolutions;

        #endregion Fields

        ///___METHODS___///

        #region Public Methods

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

        #endregion Public Methods

        #region Private Methods

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

                if (resolution.width == Screen.currentResolution.width && resolution.height == Screen.currentResolution.height)
                {
                    curIndex = i;
                }
                i++;
            }
            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = curIndex;
            resolutionDropdown.RefreshShownValue();
        }

        private void Update()
        {
        }

        #endregion Private Methods
    }
}