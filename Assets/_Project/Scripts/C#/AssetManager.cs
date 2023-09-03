
using UnityEngine;
using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using NaughtyAttributes;
using Bas.Utility;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace DataService
{
    /// <summary>
    /// AlwaysActive gameobject which can hold asset reference for easy access in any scene without use of addressables and recourcefolders.
    /// </summary>
    public class AssetManager : MonoSingleton<AssetManager>
    {
        ///___VARIABLES___///
        #region inspector
        [Scene]
        [Tooltip("The main menu scene")]
        public string menuScene;
        [Scene]
        [Tooltip("The scene to load on new game")]
        public string newGameScene;
        /* delete
        [field:SerializeField, ValidateInput("IsResourcesDirectory")]
        public string BackgroundsDirectory { get; private set; }

        [field: SerializeField, ValidateInput("IsResourcesDirectory")]
        public string PortraitsDirectory { get; private set; }
        [field: SerializeField, ValidateInput("IsResourcesDirectory")]
        public string VoxDirectory { get; private set; }
        [field: SerializeField, ValidateInput("IsResourcesDirectory")]
        public string SfxDirectory { get; private set; }
        [field: SerializeField, ValidateInput("IsResourcesDirectory")]
        public string AmbianceDirectory { get; private set; }
        */

        #endregion
        //private string pathToResources => Application.dataPath + "/_Project_/Resources"

        // helper functions
        public bool IsResourcesDirectory(string relativePath)
        {
            return Directory.Exists(pathToResources + "/" + relativePath);
        }

        #region backend
        public Dictionary<Ink.Runtime.InkListItem, Sprite> Sprites;

        public Dictionary<Ink.Runtime.InkListItem, AudioClip> AudioClips;

        #endregion
        ///___METHODS___///
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);

            /// init dictionaries
            /// 
            HashSet<Sprite> spritesHash = new(sprites);
            Sprites = new();
            foreach (Sprite sprite in spritesHash)
            {
                Sprites.Add(sprite.name.ToLower().Split('.')[0], sprite);
            }

            HashSet<AudioClip> audioHash = new(audioClips);
            AudioClips = new();
            foreach (AudioClip audioClip in audioHash)
            {
                AudioClips.Add(audioClip.name.ToLower().Split('.')[0], audioClip);
            }
        }

    }
}