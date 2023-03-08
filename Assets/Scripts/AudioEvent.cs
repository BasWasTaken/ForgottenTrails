using NaughtyAttributes;
using System.Collections;
using UnityEngine;

namespace Utility
{
    /// <summary>
    /// Class for creating <see cref="AudioEvent"/> Assets, which allow <see cref="MonoBehaviour"/>s to play <see cref="AudioClip"/>s in varying pitch and volume.
    /// </summary>
    [CreateAssetMenu(fileName = "Assets/4) Audio/AudioEvents/Sample_Action(New).asset", menuName = "Utility/AudioEvent", order = 190)]
    public class AudioEvent : ScriptableObject
    {
        ///___VARIABLES___///
        #region INSPECTOR
        [SerializeField]
        [Tooltip("List of possible soundbites to draw from on playback. It is completely optional to include more than one.")]
        private AudioClip[] clips;
        [SerializeField]
        [Tooltip("Acceptable range of pitch on playback."), MinMaxSlider(.1f, 10)]
        private Vector2 pitch = new(1, 1);
        [SerializeField]
        [Tooltip("Acceptable range of volume on playback."), MinMaxSlider(0, 1)]
        private Vector2 volume = new(.5f, .5f);
        [Button("Preset Pitch High")]
        [Tooltip("CLick to set values for a higher pitch range.")]
        protected void SetPitchHigh() { pitch = new(1.1f, 1.3f); }
        [Button("Preset Pitch Mid")]
        [Tooltip("CLick to set values for a neutral pitch range.")]
        protected void SetPitchMid() { pitch = new(.9f, 1.1f); }
        [Button("Preset Pitch Low")]
        [Tooltip("CLick to set values for a deeper pitch range.")]
        protected void SetPitchLow() { pitch = new(.7f, .9f); }
        [Button("Reset Pitch")]
        [Tooltip("CLick to set values for a neutral, unedited pitch.")]
        protected void ResetPitch() { pitch = new(1, 1); }
        [Button("Reset Volume")]
        [Tooltip("CLick to set values for a deeper sound range.")]
        protected void ResetVolume() { volume = new(.5f, .5f); }
        #endregion
        ///___METHODS___///
        #region METHODS
        public void Play(AudioSource audioSource, float delay = 0, bool interrupt = true)
        {
            if (clips.Length == 0) return;
            if (interrupt)
            {
                audioSource.Stop();
            }
            if (!audioSource.isPlaying & delay == 0)
            {
                PlayNow(audioSource);
            }
            else
            {
                audioSource.gameObject.GetComponent<MonoBehaviour>().StartCoroutine(DelayedPlay(audioSource, delay));
            }
        }
        private IEnumerator DelayedPlay(AudioSource audioSource, float delay = 0)
        {
            yield return new WaitForSeconds(delay);
            yield return new WaitWhile(() => audioSource.isPlaying);
            PlayNow(audioSource);
        }
        private void PlayNow(AudioSource audioSource)
        {
            AudioClip clip = clips[Random.Range(0, clips.Length)];
            audioSource.volume = Random.Range(volume.x, volume.y);
            audioSource.pitch = Random.Range(pitch.x, pitch.y);
            audioSource.PlayOneShot(clip);
        }
        #endregion
    }
}