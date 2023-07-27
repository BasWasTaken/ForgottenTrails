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

namespace ForgottenTrails
{
    /// <summary>
    /// <para>Works in tandem with <see cref="InkParser"/> to populate scenes with assets and effects as dictated in <see cref="Ink.Runtime.Story"/> assets.</para>
    /// </summary>
    public class SetDresser : MonoBehaviour
    {
        // Inspector Properties
        #region Inspector Properties

        #endregion
        // Public Properties
        #region Public Properties

        #endregion
        // Private Properties
        #region Private Properties

        #endregion
        // MonoBehaviour Events
        #region MonoBehaviour Events

        #endregion
        // Public Methods
        #region Public Methods

        #endregion
        // Private Methods
        #region Private Methods

        #endregion
        // UNRESOLVED
        [SerializeField, BoxGroup("Prefabs"), Required]
        private Image portraitPrefab = null;
        [SerializeField, BoxGroup("Scene References"), Required]
        private HorizontalLayoutGroup portraits;

        [SerializeField, BoxGroup("Scene References"), Required]
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
                InkDataAsset.sceneState.sprites += ", " + fileName;
            }
        }

        public void SetColor(string color, float duration = 0)
        {
            StartCoroutine(bgImage.FadeTo(color, duration));
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
            InkDataAsset.SceneState.Background = fileName;
            if (duration > 0)
            {
                StartCoroutine(SetDresser.bgImage.FadeTo(sprite, duration));
            }
            else
            {
                SetDresser.bgImage.SnapTo(sprite);
            }
        }
    }
}
