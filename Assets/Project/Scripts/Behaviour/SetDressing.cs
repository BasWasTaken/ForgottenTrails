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
            internal SetDressing()
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



            public void SetSprites(string fileNames)
            {
                /// first clear all portraits
                foreach (Image item in portraits.GetComponentsInChildren<Image>())
                {
                    Destroy(item.gameObject);
                }

                string[] fileNamesSplit = fileNames.Split(',');

                foreach (string fileName0 in fileNamesSplit)
                {
                    Sprite sprite = null; /// clear if no other value is given

                    string fileName = fileName0.ToLower().Trim(' '); // trim spaces
                    if (!(fileName == "" | fileName == "null"))
                    {
                        try
                        {
                            if (!AssetManager.Instance.Sprites.TryGetValue(fileName.ToLower(), out Sprite sprite1))
                            {
                                Debug.LogError(new FileNotFoundException("File not found: " + fileName));
                            }
                            else
                            {
                                sprite = sprite1;
                            }
                        }
                        catch (Exception e)
                        {
                            // Extract some information from this exception, and then
                            // throw it to the parent method.
                            if (e.Source != null)
                                Console.WriteLine("IOException source: {0}", e.Source);
                            Debug.LogException(e);
                        }
                    }

                    Instantiate(portraitPrefab, portraits.transform).sprite = sprite;
                    Controller.InkDataAsset.SceneState.Sprites += ", " + fileName;
                }
            }

            public void SetColor(string color, float duration = 0)
            {
                Controller.StartCoroutine(bgImage.FadeTo(color, duration));
            }

            public void SetBackdrop(string fileName, float duration = 0)
            {
                Sprite sprite = null; // clear bg if no other value is given
                if (!(fileName == "" | fileName == "null"))
                {
                    try
                    {
                        if (!AssetManager.Instance.Sprites.TryGetValue(fileName.ToLower(), out Sprite sprite1))
                        {
                            throw new FileNotFoundException("File not found: " + fileName);
                        }
                        else
                        {
                            sprite = sprite1;
                        }
                    }
                    catch (Exception e)
                    {
                        // Extract some information from this exception, and then
                        // throw it to the parent method.
                        if (e.Source != null)
                            Console.WriteLine("IOException source: {0}", e.Source);
                        Debug.LogException(e);
                    }
                }
                Controller.InkDataAsset.SceneState.Background = fileName;
                if (duration > 0)
                {
                    Controller.StartCoroutine(bgImage.FadeTo(sprite, duration));
                }
                else
                {
                    bgImage.SnapTo(sprite);
                }
            }

            public void ParseAudio(string fileName, AudioHandler.AudioGroup audioGroup, float relVol = .5f)
            {
                AudioClip audioClip = null; /// clear audio if no other value is given
                if (!(fileName == "" | fileName == "null"))
                {
                    try
                    {
                        if (!AssetManager.Instance.AudioClips.TryGetValue(fileName.ToLower(), out AudioClip audioClip1))
                        {
                            throw new FileNotFoundException("File not found: " + fileName);
                        }
                        else
                        {
                            audioClip = audioClip1;
                        }
                    }
                    catch (Exception e)
                    {
                        // Extract some information from this exception, and then
                        // throw it to the parent method.
                        if (e.Source != null)
                            Console.WriteLine("IOException source: {0}", e.Source);
                        Debug.LogException(e);
                    }
                }
                if (relVol > 1)
                {
                    Debug.LogWarning(string.Format("Relative volume of {0} exceeds accepted cap of 1.", relVol));
                    relVol = 1;
                }
                else if (relVol < 0)
                {
                    Debug.LogWarning(string.Format("Relative volume of {0} does not exceed minimum value of 0.", relVol));
                    relVol = 0;
                }
                bool oneShot = false;
                bool loop = false;
                if (audioGroup == AudioHandler.AudioGroup.Music)
                {
                    //oneShot = false;
                    loop = true;
                    Controller.InkDataAsset.SceneState.ActiveMusic = fileName;
                }
                else if (audioGroup == AudioHandler.AudioGroup.Ambiance)
                {
                    //oneShot = false;
                    loop = true;
                    Controller.InkDataAsset.SceneState.ActiveAmbiance = fileName;
                }
                PlayAudio(audioClip, audioGroup, relVol, oneShot: oneShot, loop: loop);
                Debug.Log("Test");
            }
            private void PlayAudio(AudioClip audioClip, AudioHandler.AudioGroup audioGroup, float relVol = .5f, bool oneShot = false, bool loop = false)
            {
                Debug.Log("Test");
                AudioSource audioSource = AudioHandler.GetSource(audioGroup);
                Debug.Log(AudioHandler);
                Debug.Log(audioGroup);
                Debug.Log(audioSource);
                if (audioClip == null)
                {
                    Debug.Log("Test");
                    audioSource.clip = null;
                    audioSource.Stop();
                }
                else
                {
                    Debug.Log("Test");
                    if (oneShot)
                    {
                        Debug.Log("Test");
                        audioSource.PlayOneShot(audioClip, relVol);
                    }
                    else
                    {
                        Debug.Log("Test");
                        if (audioSource.clip != audioClip) /// if it's a different clip than before, start playing at volume
                        {
                            Debug.Log("Test");
                            audioSource.clip = audioClip;
                            audioSource.volume = relVol;
                            audioSource.Play();
                        }
                        else /// otherwise just apply the volume, but gradually
                        {
                            Debug.Log("Test");
                            Controller.StartCoroutine(ShiftVolumeGradually(audioSource, audioClip, relVol));
                        }

                        if (loop)
                        {
                            Debug.Log("Test");
                            audioSource.loop = true;
                        }
                        else
                        {
                            Debug.Log("Test");
                            audioSource.loop = false;
                            Controller.StartCoroutine(RemoveClipWhenFinished(audioSource));
                        }
                    }
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
