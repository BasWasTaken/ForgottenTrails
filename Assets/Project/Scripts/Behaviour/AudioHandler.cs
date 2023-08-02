using UnityEngine;
using NaughtyAttributes;
using ForgottenTrails.InkFacilitation;

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
        public AudioSource AudioSourceSystem { get; }

        [field: SerializeField, BoxGroup("Scene References"), Required]
        [Tooltip("Here drag the component used for sfx.")]
        public AudioSource AudioSourceSfx { get; }

        [field: SerializeField, BoxGroup("Scene References"), Required]
        [Tooltip("Here drag the component used for voice.")]
        public AudioSource AudioSourceVoice { get; }
        
        [field: SerializeField, BoxGroup("Scene References"), Required]
        [Tooltip("Here drag the component used for ambiance.")]
        public AudioSource AudioSourceAmbiance { get; }
        
        [field: SerializeField, BoxGroup("Scene References"), Required]
        [Tooltip("Here drag the component used for music.")] 
        public AudioSource AudioSourceMusic { get; }

        #endregion
        // Public Methods
        #region Public Methods
        public AudioSource GetSource(AudioGroup audioGroup)
        {
            return audioGroup switch
            {
                AudioGroup.Sfx => AudioSourceSfx,
                AudioGroup.Ambiance => AudioSourceAmbiance,
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
