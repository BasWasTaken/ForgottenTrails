using UnityEngine;
using NaughtyAttributes;
using ForgottenTrails.InkFacilitation;
using System.Collections.Generic;

namespace Bas.Utility
{

    /// <summary>
    /// <para>Simply a container to handle several <see cref="AudioSource"/>s (as child objects) from within one object.</para>
    /// </summary>
    public class AudioHandler : MonoBehaviour
    {
        // Inspector Properties
        #region Inspector Properties
        [field: SerializeField, BoxGroup("Scene References"), Required]
        [Tooltip("Here drag the component used for system sounds like ui.")]
        public AudioSource AudioSourceSystem { get; private set; }

        [field: SerializeField, BoxGroup("Scene References"), Required]
        [Tooltip("Here drag the component used for sfx.")]
        public AudioSource AudioSourceSfx { get; private set; }

        [field: SerializeField, BoxGroup("Scene References"), Required]
        [Tooltip("Here drag the component used for voice.")]
        public AudioSource AudioSourceVoice { get; private set; }

        [SerializeField, BoxGroup("Scene References"), Required]
        [Tooltip("Here drag the component used for ambiance.")]
        public AudioSource AudioSourceAmbiance; // NOTE should actually chagne this name so that not accidentalyl called

        private List<AudioSource> _AudioSourcesAmbiance = new();
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
        
        [field: SerializeField, BoxGroup("Scene References"), Required]
        [Tooltip("Here drag the component used for music.")] 
        public AudioSource AudioSourceMusic { get; private set; }

        #endregion
        // Public Methods
        #region Public Methods
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

        #endregion


        // Peripheral
        #region Peripheral
        public enum AudioGroup
        {
            Sfx,
            Ambiance,
            Music,
            Voice,
            System
        }

        #endregion
    }
}
