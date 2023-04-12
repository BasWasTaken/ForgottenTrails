
using UnityEngine;
using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using NaughtyAttributes;
using Utility;
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
        [Tooltip("Sprites to be made accessible in scenes.")]
        [SerializeField]
        private Sprite[] sprites;
        public Dictionary<string, Sprite> Sprites;
        [Tooltip("Audiofiles to be made accessible in scenes.")]
        [SerializeField]
        private AudioClip[] audioClips;
        public Dictionary<string, AudioClip> AudioClips;
        #endregion
        #region backend
        #endregion
        ///___METHODS___///
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
            
            /// init dictionaries
            Sprites = new();
            foreach (Sprite sprite in sprites)
            {
                Sprites.Add(sprite.name.ToLower().Split('.')[0], sprite);
            }

            AudioClips = new();
            foreach (AudioClip audioClip in audioClips)
            {
                AudioClips.Add(audioClip.name.ToLower().Split('.')[0], audioClip);
            }
        }
    }
}