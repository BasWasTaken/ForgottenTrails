using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FeedAudio : MonoBehaviour
{
    private AudioSource audioSource;
    private void Awake()
    {
        if(!TryGetComponent(out audioSource))
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
    public void Feed(AudioClip audioClip) {
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
    IEnumerator RemoveClipWhenFinished(AudioSource audioSource)
    {
        yield return new WaitWhile(() => audioSource.isPlaying);
        audioSource.clip = null;
    }
}
