using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    [Tooltip("The slider to use as value")]
    public Slider slider;
    [Tooltip("The volume to change")]
    public AudioMixerGroup audioGroup;
    private string ParameterName => audioGroup.ToString()+"Volume";
    void Start()
    {
        Debug.Log("Getting volume level from playerprefs");
        slider.value = PlayerPrefs.GetFloat(ParameterName, 0.75f);
    }
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
}