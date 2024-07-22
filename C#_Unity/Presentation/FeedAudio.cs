using System.Collections;
using UnityEngine;

namespace VVGames.ForgottenTrails.Presentation
{
    public class FeedAudio : MonoBehaviour
    {
        #region Fields

        private AudioSource audioSource;

        #endregion Fields

        #region Public Methods

        public void Feed(AudioClip audioClip)
        {
            if (audioClip != null)
            {
                if (true)//audioSourceSfx.clip != audioClip)
                {
                    audioSource.clip = audioClip;
                    audioSource.PlayOneShot(audioClip);
                    StartCoroutine(RemoveClipWhenFinished(audioSource));
                }
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void Awake()
        {
            if (!TryGetComponent(out audioSource))
            {
                foreach (AudioSource item in FindObjectsOfType<AudioSource>())
                {
                    if (item.gameObject.name.Contains("System"))
                    {
                        audioSource = item;
                        break;
                    }
                }
            }
        }

        private IEnumerator RemoveClipWhenFinished(AudioSource audioSource)
        {
            yield return new WaitWhile(() => audioSource.isPlaying);
            audioSource.clip = null;
        }

        #endregion Private Methods
    }
}