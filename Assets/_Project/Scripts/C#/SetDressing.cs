using DataService;
using Ink.Runtime;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Bas.Utility;
using UnityEditor;
using static Bas.Utility.AudioHandler;
using Extensions;

namespace ForgottenTrails.InkFacilitation
{
    
    [RequireComponent(typeof(AudioHandler))]
    public partial class StoryController : MonoSingleton<StoryController>
    {/// <summary>
     /// <para>Works in tandem with <see cref="StoryController"/> to populate scenes with assets and effects as dictated in <see cref="Ink.Runtime.Story"/> assets.</para>
     /// </summary>
        [Serializable]
        public class SetDressing
        {
            // Inspector Properties
            #region Inspector Properties

            #endregion
            // Public Properties
            #region Public Properties

            #endregion
            // Private Properties
            #region Private Properties
            private StoryController Controller;
            internal AudioHandler AudioHandler { get; set; }
            #endregion
            // Constructor
            #region Constructor
            internal void Assign()
            {
                Controller = Instance;
                AudioHandler = Controller.GetComponent<AudioHandler>();
            }
            #endregion
            // Public Methods
            #region Public Methods

            #endregion
            // Private Methods
            #region Private Methods

            #endregion
            // UNRESOLVED
            [SerializeField, Header("Prefabs"), Required]
            private Image portraitPrefab = null;
            [SerializeField, Header("Scene References"), Required]
            private HorizontalLayoutGroup portraits;

            [SerializeField, Header("Scene References"), Required]
            public BackGround bgImage;

            public void SetColor(string color, float duration = 0)
            {
                Controller.StartCoroutine(bgImage.FadeTo(color, duration));
            }

            public void SetBackground(InkListItem inkListItem, float duration = 0)
            {
                Sprite sprite = null; // default to no background

                if (inkListItem.itemName == "none")
                {   

                }
                else // if any other item beside "none" 
                {
                    if(AssetManager.Instance.assets.TryGetValue(inkListItem, out string path))
                    {
                        sprite = (Sprite)Resources.Load(path,typeof(Sprite));
                        if (sprite == null)
                        {
                            Debug.LogError("could not find resource at " + path);
                        }
                    }
                    else
                    {
                        Debug.LogErrorFormat("No background found for {0}.", inkListItem);
                    }
                }

                if (duration > 0)
                {
                    Controller.StartCoroutine(bgImage.FadeTo(sprite, duration));
                }
                else
                {
                    bgImage.SnapTo(sprite);
                }
            }

            public void SetSprites(InkList inkList)
            {
                //List<Sprite> sprites = null; // default to no sprites

                if (inkList.maxItem.Key.itemName == "none") // NOTE: this check even unnesesay, can just move into the foreach and ignore the "none", elghouth I guess that would mean having to go this each iteration
                {
                    // clear all portraits
                    foreach (Image item in portraits.GetComponentsInChildren<Image>())
                    {
                        Destroy(item.gameObject);
                    }
                }
                else // if any other items beside "none" 
                {
                    // TODO: first remove unneeded, then add missing, instead of just..:

                    // add all sprites
                    foreach (InkListItem inkListItem in inkList.Keys)
                    {
                        // first clear all portraits
                        foreach (Image item in portraits.GetComponentsInChildren<Image>())
                        {
                            Destroy(item.gameObject);
                        }
                        if (AssetManager.Instance.assets.TryGetValue(inkListItem, out string path))
                        {
                            //sprites.Add(sprite);
                            Instantiate(portraitPrefab, portraits.transform).sprite = (Sprite)Resources.Load(path);
                        }
                        else
                        {
                            Debug.LogErrorFormat("No background found for {0}.", inkListItem);
                        }
                    }
                }
            }            
            public void InkRequestAudio(InkListItem inkListItem, float relVol = .5f)
            {
                AudioClip find = FindAudio(inkListItem);
                var audioGroup = inkListItem.originName switch // determine audiogroup
                {
                    "Vox" => AudioGroup.Voice,
                    "Sfx" => AudioGroup.Sfx,
                    "Ambiance" => AudioGroup.Ambiance,
                    "Music" => AudioGroup.Music,
                    _ => AudioGroup.System,
                };
                PlayOrStopAudio(find, audioGroup, relVol);
            }
            public void StopMusic()
            {
                ProcessStopRequest(AudioGroup.Music);
            }
            public void RemoveAmbianceAll()
            {
                ProcessStopRequest(AudioGroup.Ambiance);
            }
            public void RemoveAmbiance(InkListItem inkListItem)
            {
                AudioClip clip = FindAudio(inkListItem);
                ProcessStopRequest(AudioGroup.Ambiance, clip);
            }
            public void RemoveAmbiance(AudioClip clip)
            {
                ProcessStopRequest(AudioGroup.Ambiance, clip);
            }
            public AudioClip FindAudio(InkListItem inkListItem)
            {
                AudioClip audioClip = null; // default to null

                if (inkListItem.itemName == "none")
                {

                }
                else // if any other item beside "none" 
                {
                    if (AssetManager.Instance.assets.TryGetValue(inkListItem, out string path))
                    {
                        audioClip = (AudioClip)Resources.Load(path);
                    }
                    else
                    {
                        Debug.LogErrorFormat("No background found for {0}.", inkListItem);
                    }
                }
                return audioClip;
            }

            private void AudioFadeOut(AudioSource audioSource, AudioClip audioClip)
            {
                ShiftVolumeGradually(audioSource, audioClip, 0);
                StopClipWhenVolume0(audioSource);
                RemoveClipWhenFinished(audioSource);
            }
            /// can be made a lot cleaner, I think. perhaps should be split up 
            public void ProcessStopRequest(AudioGroup audioGroup, AudioClip audioClip = null, bool sudden = false)
            {
                AudioSource audioSource;
                if (audioGroup == AudioGroup.Ambiance)
                {
                    if (audioClip != null) // stop specific ambiance
                    {
                        if (AudioHandler.TryGetAmbianceSource(audioClip, out audioSource))
                        {
                            AudioFadeOut(audioSource, audioClip);
                        }
                        else
                        {
                            Debug.LogErrorFormat("ambiance with clip not found");
                        }
                    }
                    else // stop all ambiance
                    {
                        foreach (AudioSource source in AudioHandler.AudioSourcesAmbiance)
                        {
                            if (source.clip != null)
                            {
                                AudioFadeOut(source, audioClip);
                            }
                            else
                            {
                                Destroy(source.gameObject);
                            }
                        }
                    }
                }
                else // stop other audio type
                {
                    audioSource = AudioHandler.GetSource(audioGroup);
                    if (sudden)
                    {
                        audioSource.Stop();
                        audioSource.clip = null;
                    }
                    else
                    {
                        AudioFadeOut(audioSource, audioClip);
                    }
                }

            }
            /// can be made a lot cleaner, I think. perhaps should be split up 
            private void PlayOrStopAudio(AudioClip audioClip, AudioGroup audioGroup, float relVol = .5f)
            {
                AudioSource audioSource = AudioHandler.GetSource(audioGroup);
                if (audioClip == null)
                {
                    ProcessStopRequest(audioGroup, audioClip);
                }
                else
                {

                    if (relVol > 1)
                    {
                        Debug.LogWarningFormat("Relative volume of {0} exceeds accepted cap of 1.", relVol);
                        relVol = 1;
                    }
                    else if (relVol < 0)
                    {
                        Debug.LogWarningFormat("Relative volume of {0} does not exceed minimum value of 0.", relVol);
                        relVol = 0;
                    }

                    switch (audioGroup)
                    {
                        case AudioGroup.Sfx:
                            audioSource.loop = false;
                            audioSource.PlayOneShot(audioClip, relVol);
                            break;
                        case AudioGroup.Voice:
                            audioSource.loop = false;
                            audioSource.PlayOneShot(audioClip, relVol);
                            break;
                        case AudioGroup.Ambiance:                        
                            if (AudioHandler.TryGetAmbianceSource(audioClip, out audioSource)) // if the clip is already playing
                            {
                                // apply volume only
                                Controller.StartCoroutine(ShiftVolumeGradually(audioSource, audioClip, relVol));
                            }
                            else // if it's a new clip,
                            {
                                // start playing at volume
                                audioSource.clip = audioClip;
                                audioSource.volume = relVol;
                                audioSource.loop = true;
                                audioSource.Play();
                            } 
                            break;
                        case AudioGroup.Music:
                            if (audioSource.clip != audioClip) /// if it's a different clip than before, start playing at volume
                            {
                                audioSource.clip = audioClip;
                                audioSource.volume = relVol;
                                audioSource.loop = true;
                                audioSource.Play();
                            }
                            else /// otherwise just apply the volume, but gradually
                            {
                                Controller.StartCoroutine(ShiftVolumeGradually(audioSource, audioClip, relVol));
                            }
                            break;
                        case AudioGroup.System:
                            audioSource.loop = false;
                            audioSource.PlayOneShot(audioClip, relVol);
                            break;
                        default:
                            break;
                    }
                    Controller.StartCoroutine(RemoveClipWhenFinished(audioSource));
                }
            }
            IEnumerator ShiftVolumeGradually(AudioSource audioSource, AudioClip audioClip, float relVol)
            {
                float t = .1f; ///how long to wait before each increment
                float d = .01f; ///size of an increment
                while (audioSource.clip == audioClip & audioSource.volume != relVol)
                {
                    yield return new WaitForSecondsRealtime(t);
                    audioSource.volume = Mathf.MoveTowards(audioSource.volume, relVol, d);
                }
            }

            IEnumerator StopClipWhenVolume0(AudioSource audioSource)
            {
                if (audioSource.volume > 0) yield return new WaitWhile(() => audioSource.volume > 0);
                audioSource.Stop();
            }
            IEnumerator RemoveClipWhenFinished(AudioSource audioSource)
            {
                if (audioSource.isPlaying) yield return new WaitWhile(() => audioSource.isPlaying);
                audioSource.clip = null;
            }

        }
    }    
}
