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

            [SerializeField, Header("Scene References"), Required]
            public items.Inventory Inventory;

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
                    if(AssetManager.Instance.Sprites.TryGetValue(inkListItem, out sprite))
                    {

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
                        if (AssetManager.Instance.Sprites.TryGetValue(inkListItem, out Sprite sprite))
                        {
                            //sprites.Add(sprite);
                            Instantiate(portraitPrefab, portraits.transform).sprite = sprite;
                        }
                        else
                        {
                            Debug.LogErrorFormat("No background found for {0}.", inkListItem);
                        }
                    }
                }
            }            
            public void FindAndPlayAudio(InkListItem inkListItem, float relVol = .5f)
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
                PlayAudio(find, audioGroup, relVol);
            }
            public AudioClip FindAudio(InkListItem inkListItem)
            {
                AudioClip audioClip = null; // default to null

                if (inkListItem.itemName == "none")
                {

                }
                else // if any other item beside "none" 
                {
                    if (AssetManager.Instance.AudioClips.TryGetValue(inkListItem, out audioClip))
                    {

                    }
                    else
                    {
                        Debug.LogErrorFormat("No background found for {0}.", inkListItem);
                    }
                }
                return audioClip;
            }
            private void PlayAudio(AudioClip audioClip, AudioGroup audioGroup, float relVol = .5f)
            {

                AudioSource audioSource = AudioHandler.GetSource(audioGroup);
                if (audioClip == null)
                {
                    // TODO: if ambiance:
                    // - i need a special method for getting the source 
                    // - want fadout when removing clip
                    // - and i wanna remove the children if inactive for a while
                    audioSource.clip = null;
                    audioSource.Stop();
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
                            bool exists = false;
                            foreach (AudioSource source in AudioHandler.AudioSourcesAmbiance)
                            {
                                if(source.clip == audioClip)
                                {
                                    exists = true;
                                    audioSource = source;
                                    break;
                                }
                            }

                            if (exists)
                            {
                                // apply volume only
                                Controller.StartCoroutine(ShiftVolumeGradually(audioSource, audioClip, relVol));
                            }
                            else // if it's a different clip than before,
                            {
                                // get new audio source
                                audioSource = AudioHandler.FirstAvailableAmbianceLayer();

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

            IEnumerator RemoveClipWhenFinished(AudioSource audioSource)
            {
                if (audioSource.isPlaying) yield return new WaitWhile(() => audioSource.isPlaying);
                audioSource.clip = null;
            }

        }
    }    
}
