using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Bas.ForgottenTrails.UI
{
    /// <summary>
    /// Controls game volume through slider objects and stores value in playerprefs.
    /// </summary>
    public class SetVolume : MonoBehaviour
    {
        #region Fields

        [Tooltip("The slider to use as value")]
        public Slider slider;

        [Tooltip("The volume to change")]
        public AudioMixerGroup audioGroup;

        #endregion Fields

        #region Properties

        private string ParameterName => audioGroup.ToString() + "Volume";

        #endregion Properties

        #region Public Methods

        public void SetLevel(float sliderValue)
        {
            float newValue;
            if (sliderValue == 0)
            {
                newValue = -80;
            }
            else
            {
                newValue = Mathf.Log10(sliderValue) * 20;
            }
            audioGroup.audioMixer.SetFloat(ParameterName, newValue);
            PlayerPrefs.SetFloat(ParameterName, newValue);
        }

        #endregion Public Methods

        #region Private Methods

        private void Awake()
        {
            //Debug.Log("Getting volume level from playerprefs: " + PlayerPrefs.GetFloat(ParameterName));
            float storedValue = PlayerPrefs.GetFloat(ParameterName, 0.75f);
            float sliderValue;
            if (storedValue <= -80)
            {
                sliderValue = 0; // Set minimum value on the slider
            }
            else
            {
                sliderValue = Mathf.Pow(10, storedValue / 20); // Reverse conversion to get slider value
            }
            slider.value = sliderValue;
        }

        #endregion Private Methods
    }
}