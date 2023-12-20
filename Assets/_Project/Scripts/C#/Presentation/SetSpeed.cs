using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Bas.ForgottenTrails.InkConnections
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
            dropdown.ClearOptions();
            List<string> options = new();
            foreach (var item in Enum.GetNames(typeof(StoryController.TextProduction.TextSpeed)))
            {
                options.Add(item);
            }
            dropdown.AddOptions(options);
        }
        private void Start()
        {
            StoryController.TextProduction.TextSpeed storedValue = (StoryController.TextProduction.TextSpeed)PlayerPrefs.GetInt("textSpeed");
            var value = storedValue switch
            {
                StoryController.TextProduction.TextSpeed.sluggish => 0,
                StoryController.TextProduction.TextSpeed.extraSlow => 1,
                StoryController.TextProduction.TextSpeed.slow => 2,
                StoryController.TextProduction.TextSpeed.medium => 3,
                StoryController.TextProduction.TextSpeed.fast => 4,
                StoryController.TextProduction.TextSpeed.extraFast => 5,
                StoryController.TextProduction.TextSpeed.bonkers => 6,
                _ => 3,
            };
            dropdown.value = value;
        }
        #endregion
        #region OTHER_METHODS
        public void SetSpeedTo(int value)
        {
            var speed = value switch
            {
                0 => StoryController.TextProduction.TextSpeed.sluggish,
                1 => StoryController.TextProduction.TextSpeed.extraSlow,
                2 => StoryController.TextProduction.TextSpeed.slow,
                3 => StoryController.TextProduction.TextSpeed.medium,
                4 => StoryController.TextProduction.TextSpeed.fast,
                5 => StoryController.TextProduction.TextSpeed.extraFast,
                6 => StoryController.TextProduction.TextSpeed.bonkers,
                _ => StoryController.TextProduction.TextSpeed.medium,
            };
            PlayerPrefs.SetInt("textSpeed", (int)speed); // should be redundand
            if (StoryController.Instance != null)
            {
                StoryController.Instance.TextProducer.TextSpeedPreset = speed;
            }
            else
            {
                PlayerPrefs.SetInt("textSpeed", (int)speed); // set value in prefs to be picked up later
            }
        }
        #endregion
    }
}
