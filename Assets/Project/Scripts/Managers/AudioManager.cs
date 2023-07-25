using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Utility;

namespace ForgottenTrails 
{ 
    /// <summary>
    /// <para>SUMMARY GOES HERE.</para>
    /// </summary>
    public class AudioManager : MonoSingleton<AudioManager>
    {
        public enum AudioGroup
        {
            Sfx,
            Ambiance,
            Music,
            Voice,
            System
        }
        [SerializeField]
        private AudioSource audioSourceSystem;
        [SerializeField]
        private AudioSource audioSourceSfx;
        [SerializeField]
        private AudioSource audioSourceVoice;
        [SerializeField]
        private AudioSource audioSourceAmbiance;
        [SerializeField]
        private AudioSource audioSourceMusic;
        public AudioSource GlobalAudio(AudioGroup audioGroup)
        {
            return audioGroup switch
            {
                AudioGroup.Sfx => audioSourceSfx,
                AudioGroup.Ambiance => audioSourceAmbiance,
                AudioGroup.Music => audioSourceMusic,
                AudioGroup.Voice => audioSourceVoice,
                AudioGroup.System => audioSourceSystem,
                _ => null,
            };
        }
    }
}
