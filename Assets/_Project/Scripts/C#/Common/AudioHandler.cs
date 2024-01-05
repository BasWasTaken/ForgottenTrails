// ------------------------------------------------------------------------------
// Created on: Pre-2024.
// Author: Bas Vegt.
// Purpose: Handling several audio sources from within one object.
// ------------------------------------------------------------------------------
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace Bas.Common
{
    /// <summary>
    /// <para>Simply a container to handle several <see cref="AudioSource"/>s (as child objects) from within one object.</para>
    /// </summary>
    public class AudioHandler : MonoBehaviour
    {
        // Inspector Properties

        #region Fields

        [SerializeField, BoxGroup("Scene References"), Required]
        [Tooltip("Here drag the component used for ambiance.")]
        public AudioSource AudioSourceAmbiance;

        private List<AudioSource> _AudioSourcesAmbiance = new();

        #endregion Fields

        #region Enums

        public enum AudioGroup
        {
            Sfx,
            Ambiance,
            Music,
            Voice,
            System
        }

        #endregion Enums

        #region Properties

        [field: SerializeField, BoxGroup("Scene References"), Required]
        [Tooltip("Here drag the component used for system sounds like ui.")]
        public AudioSource AudioSourceSystem { get; private set; }

        [field: SerializeField, BoxGroup("Scene References"), Required]
        [Tooltip("Here drag the component used for sfx.")]
        public AudioSource AudioSourceSfx { get; private set; }

        [field: SerializeField, BoxGroup("Scene References"), Required]
        [Tooltip("Here drag the component used for voice.")]
        public AudioSource AudioSourceVoice { get; private set; }

        // NOTE should actually chagne this name so that not accidentalyl called
        public List<AudioSource> AudioSourcesAmbiance
        {
            get
            {
                if (_AudioSourcesAmbiance.Count == 0)
                {
                    NewAmbienceLayer();
                }
                return _AudioSourcesAmbiance;
            }
            private set
            {
                _AudioSourcesAmbiance = value;
            }
        }

        [field: SerializeField, BoxGroup("Scene References"), Required]
        [Tooltip("Here drag the component used for music.")]
        public AudioSource AudioSourceMusic { get; private set; }

        #endregion Properties

        #region Public Methods

        public AudioSource NewAmbienceLayer()
        {
            AudioSource source = Instantiate(AudioSourceAmbiance, transform);
            _AudioSourcesAmbiance.Add(source);
            return source;
        }

        public bool TryGetAmbianceSource(AudioClip clip, out AudioSource audioSource)
        {
            if (clip == null)
            {
                audioSource = FirstAvailableAmbianceLayer();
                return true;
            }
            else
            {
                foreach (AudioSource source in AudioSourcesAmbiance)
                {
                    if (source.clip == clip)
                    {
                        audioSource = source;
                        return true;
                    }
                }

                audioSource = FirstAvailableAmbianceLayer();
                return false;
            }
        }

        public AudioSource FirstAvailableAmbianceLayer()
        {
            foreach (AudioSource source in AudioSourcesAmbiance)
            {
                if (!source.isPlaying)
                {
                    source.clip = null;
                    source.transform.SetAsLastSibling();
                    return source;
                }
            }
            return NewAmbienceLayer();
        }

        // Public Methods

        public AudioSource GetSource(AudioGroup audioGroup)
        {
            return audioGroup switch
            {
                AudioGroup.Sfx => AudioSourceSfx,
                AudioGroup.Ambiance => AudioSourcesAmbiance[^1],
                AudioGroup.Music => AudioSourceMusic,
                AudioGroup.Voice => AudioSourceVoice,
                AudioGroup.System => AudioSourceSystem,
                _ => null,
            };
        }

        #endregion Public Methods

        // Peripheral
    }
}