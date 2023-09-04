
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
using items;

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
        [SerializeField]
        List<InventoryItem> possibleItems = new();
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
        public Dictionary<InkListItem, InventoryItem> ItemDictionary { get; } = new();
        public Dictionary<InkListItem, string> assets = new();

        #endregion
        ///___METHODS___///
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject); // todo: move to subclass persistentmonosignleto

            
        }

        
        [field:SerializeField]
        public TextAsset TextAsset { get; set; }

        [Button("CreateAssetLibraries",EButtonEnableMode.Editor)]
        public void CreateAssetLibraries()
        {
            string error = "";

            // NOTE wsl ipv inventory aanroepen, inventory state?
            assets.Clear();
            Story story = new(TextAsset.text);

            ListDefinition items=null;
            ListDefinition affordances = null;
            foreach (string inkListName in InkListNames)
            {
                //Debug.LogFormat("Searching for {0}", inkListName);
                if (story.listDefinitions.TryListGetDefinition(inkListName, out ListDefinition listDefinition))
                {
                    if(inkListName == "Items")
                    {   
                       // Debug.Log("Found items list.");
                        items = listDefinition;
                    }
                    else if(inkListName == "Affordances")
                    {
                      //  Debug.Log("Found affordances list.");
                        affordances = listDefinition;
                    }
                    else
                    {
                       // Debug.LogFormat("Found {0}", listDefinition);
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
                                    error += string.Format("\nitem {0} not found", item);
                                }

                            }
                        }
                    }                    
                }
                else
                {
                    error += string.Format("\nListDefinition {0} not found.", inkListName);
                }
            }
            if (error == "")
            {

            }
            else
            {
                Debug.LogAssertion("ERROR IN ATTEMPTING TO LIST ASSETS" + error);
            }

            ItemListsKnown(items, affordances);

        }

        private bool ItemListsKnown(ListDefinition items, ListDefinition affordances)
        {
            ItemDictionary.Clear();
            Dictionary<string, InventoryItem> TemporaryDictionary = new();

            foreach (InventoryItem item in possibleItems)
            {
                TemporaryDictionary.Add(item.CanonicalName, item);
            }

            string error = "";

            // assert all items from ink exist in unity
            foreach (InkListItem inkListItem in items.items.Keys)
            {
                if (TemporaryDictionary.TryGetValue(inkListItem.itemName, out InventoryItem item))
                {
                    item.InkListItem = inkListItem;
                    ItemDictionary.Add(inkListItem, item);
                }
                else
                {
                    error += string.Format("\nItem \"{0}\" not found in unity dictionary!", inkListItem.itemName);
                }
            }
            // assert all afforances are same

            // assert all affordances from ink exist in unity
            foreach (InkListItem affordance in affordances.items.Keys)
            {
                if (!Enum.IsDefined(typeof(Affordance), affordance.itemName))
                {
                    error += string.Format("\nAffordance \"{0}\" not present in unity enum definition!", affordance);
                }
            }

            // assert all affordances from unity exist in ink
            foreach (string affordance in Enum.GetNames(typeof(Affordance)))
            {
                if (!affordances.ContainsItemWithName(affordance))
                {
                    error += string.Format("\nAffordance \"{0}\" not present in ink list!", affordance);
                }
            }

            if (error == "")
            {
                return true;
            }
            else
            {
                Debug.LogAssertion("ITEMS OR AFFORDANCES NOT MATCHED UP" + error);
                return false;
            }
        }

    }
}