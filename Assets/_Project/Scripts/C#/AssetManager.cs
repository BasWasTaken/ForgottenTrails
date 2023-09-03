
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
using ForgottenTrails.InkFacilitation;
using Ink.Runtime;

namespace DataService
{
    /// <summary>
    /// AlwaysActive gameobject which can hold asset reference for easy access in any scene without use of addressables and recourcefolders.
    /// </summary>
    public class AssetManager : MonoSingleton<AssetManager>
    {
        ///___VARIABLES___///
        #region inspector
        public string[] InkListNames;
        [Scene]
        [Tooltip("The main menu scene")]
        public string menuScene;
        [Scene]
        [Tooltip("The scene to load on new game")]
        public string newGameScene;
        /* delete or ineed use list of folders to dssearch resources 
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
        private string pathToResources => Application.dataPath + "/_Project/Resources";

        // helper functions
        public bool IsResourcesDirectory(string relativePath)
        {
            return Directory.Exists(pathToResources + "/" + relativePath);
        }

        #region backend
        public Dictionary<Ink.Runtime.InkListItem, string> assets = new();

        #endregion
        ///___METHODS___///
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject); // todo: move to subclass persistentmonosignleto
        }

        [Button("CreateAssetLibraries",EButtonEnableMode.Editor)]
        private void CreateAssetLibraries()
        {
            assets.Clear();
            Story story = new Story(StoryController.Instance.InkStoryAsset.text);

            foreach (string inkListName in InkListNames)
            {
                if (story.listDefinitions.TryListGetDefinition(inkListName, out ListDefinition listDefinition))
                {
                    foreach (InkListItem item in listDefinition.items.Keys)
                    {
                        string searchFor = item.itemName;
                        if (searchFor != "none")
                        {
                            // try to find asset
                            try
                            {                
                                assets.Add(item, AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets(searchFor)[0]));
                            }
                            catch (Exception)
                            {

                                throw;
                            }

                        }
                    }
                }
                else
                {
                    Debug.LogAssertionFormat("ListDefinition {0} not found.", inkListName);
                }
            }
        }

    }
}