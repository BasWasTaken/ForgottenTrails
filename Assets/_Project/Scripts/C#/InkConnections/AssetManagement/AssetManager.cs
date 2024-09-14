using Ink.Runtime;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using VVGames.Common;
using VVGames.ForgottenTrails.InkConnections.Items;
using VVGames.ForgottenTrails.InkConnections.Party;
using VVGames.ForgottenTrails.InkConnections.Travel;

namespace VVGames.ForgottenTrails.InkConnections
{
    /// <summary>
    /// AlwaysActive gameobject which can hold asset reference for easy access in any scene without use of addressables and recourcefolders.
    /// </summary>
    public class AssetManager : MonoSingleton<AssetManager>
    {
        ///___VARIABLES___///

        #region Fields

        [Tooltip("MANUAL LIST OF WHAT LISTS TO CHECK")]
        public string[] InkListNames;

        [Scene]
        [Tooltip("The main menu scene")]
        public string menuScene;

        [Scene]
        [Tooltip("The scene to load on new game")]
        public string newGameScene;

        public Dictionary<InkListItem, string> assets = new();

        private const string resourceFolder = "/_Project/Resources/";

        [SerializeField]
        private List<InventoryItem> possibleItems = new();

        [SerializeField]
        private List<MapLocationDefinition> possibleLocations = new();

        [SerializeField]
        private List<PartyMemberSO> possiblePartyMembers = new();

        #endregion Fields

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

        #region Properties

        //public Dictionary<InkListItem, MapLocationDefinition> LocationDictionary { get; } = new(); // NOTE:: when is this every added to?
        public Dictionary<InkListItem, InventoryItem> ItemDictionary { get; } = new();

        public Dictionary<InkListItem, PartyMemberSO> PartyMemberDictionary { get; } = new();

        [field: SerializeField]
        public TextAsset TextAsset { get; set; }

        private string pathToResources => Application.dataPath + resourceFolder;
        private string basePath => "Assets" + resourceFolder;

        #endregion Properties

        #region Public Methods

        // helper functions
        public bool IsResourcesDirectory(string relativePath)
        {
            return Directory.Exists(pathToResources + "/" + relativePath);
        }

        [Button("CreateAssetLibraries", EButtonEnableMode.Editor)]
        public void CreateAssetLibraries()
        {
            string error = "";
            string noError = "";

            assets.Clear();
            Story story = new(TextAsset.text);

            ListDefinition items = null;
            ListDefinition locations = null;
            ListDefinition partyCandidates = null;
            foreach (string inkListName in InkListNames)
            {
                //Debug.LogFormat("Searching for {0}", inkListName);
                if (story.listDefinitions.TryListGetDefinition(inkListName, out ListDefinition listDefinition))
                {
                    if (inkListName == "Items")
                    {
                        items = listDefinition;
                        Debug.LogFormat("Found items list as {0}", listDefinition);
                    }
                    else if (inkListName == "Locations")
                    {
                        locations = listDefinition;
                        Debug.LogFormat("Found locations list as {0}", listDefinition);
                    }
                    else if (inkListName == "PartyCandidates")
                    {
                        partyCandidates = listDefinition;
                        Debug.LogFormat("Found partycandidates list as {0}", listDefinition);
                    }
                    else
                    {
                        // Debug.LogFormat("Found {0}", listDefinition);
                        foreach (InkListItem item in listDefinition.items.Keys)
                        {
                            string searchFor = item.itemName;
                            if (searchFor.StartsWith("BG_"))
                            {
                                searchFor = searchFor.Substring(3);
                            }
                            if (searchFor != "none" & searchFor != "NA")
                            {
                                // search for asset with that name in the database
                                string[] foundAssets = AssetDatabase.FindAssets(searchFor);
                                int limit = 9;
                                bool assetLocated = false;
                                foreach (string asset in foundAssets)
                                {
                                    string absolutePath = AssetDatabase.GUIDToAssetPath(asset);
                                    absolutePath = absolutePath.Substring(0, absolutePath.LastIndexOf('.'));// remove .extension because resources utility is super finicky
                                    string relativePath = GetRelativePath(absolutePath, basePath);
                                    if (relativePath.ToLower().Contains(searchFor.ToString().ToLower() + "_")) // check if whole name plus underscore present in asset //in searching, I think it IS okay if the case sensitivity is of, because this is non-exact anyway, and to be honest a bit hacky, and it's best to make it as frictionless as possible.
                                    {
                                        noError += string.Format("\nFound {1} for {0}", item, relativePath);
                                        if (assets.TryAdd(item, relativePath))
                                        {
                                            noError += " and succesfully added it.";
                                            assetLocated = true;
                                            break;
                                        }
                                        else
                                        {
                                            error += string.Format("\nFound {1} for {0} but could not add it.", item, relativePath);
                                        }
                                    }
                                    else
                                    {
                                        noError += String.Format("\nFound wrong asset for {0}: {1}. Trying next asset.", item, relativePath);
                                    }
                                    limit--;
                                }
                                if (!assetLocated)
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

            string message;
            if (error == "") message = "Succesfully fetched InkLists." + noError;
            else message = "ERROR IN ATTEMPTING TO LIST ASSETS" + error;

            if (items == null | locations == null | partyCandidates == null)
            {
                throw new NullReferenceException("one of the expected inklists was not set to be searched. \nLog:" + message);
            }
            else if (error != "")
            {
                throw new Exception(message);
            }
            else if (!ItemListsKnown(items))
            {
                throw new Exception("Could not match up items. \nLog:" + message);
            }
            else if (!LocationsRecognised(locations))
            {
                throw new Exception("Could not match up locations. \nLog:" + message);
            }
            else if (!PartyMembersRecognised(partyCandidates))
            {
                throw new Exception("Could not match up party members. \nLog:" + message);
            }
            else
            {
                Debug.Log("Checked all lists and assets. No discrepencies found. \nLog:" + message);
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        #endregion Protected Methods

        #region Private Methods

        private static string GetRelativePath(string fullPath, string basePath)
        {
            if (!fullPath.StartsWith(basePath))
            {
                // The fullPath is not within the basePath.
                // You should handle this case based on your requirements.
                Debug.LogError(String.Format("{0} is not in {1}", basePath, fullPath));
            }

            string relativePath = fullPath.Substring(basePath.Length);
            return relativePath;
        }

        private bool ItemListsKnown(ListDefinition items)
        {
            ItemDictionary.Clear();
            Dictionary<string, InventoryItem> TemporaryDictionary = new();

            foreach (InventoryItem item in possibleItems)
            {
                TemporaryDictionary.Add(item.CanonicalName/*.ToLower()*/, item);
            }

            string error = "";

            // assert all items from ink exist in unity
            foreach (InkListItem inkListItem in items.items.Keys)
            {
                if (TemporaryDictionary.TryGetValue(inkListItem.itemName/*.ToLower()*/, out InventoryItem item))
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

            if (error == "")
            {
                return true;
            }
            else
            {
                Debug.LogAssertion("ITEMS NOT MATCHED UP" + error);
                return false;
            }
        }

        private bool LocationsRecognised(ListDefinition locations)
        {
            //LocationDictionary.Clear();
            Dictionary<string, MapLocationDefinition> TemporaryDictionary = new();
            foreach (MapLocationDefinition loc in possibleLocations)
            {
                TemporaryDictionary.Add(loc.CanonicalName/*.ToLower()*/, loc);
            }
            string error = "";

            // assert all locations to travel to from unity exist in ink
            foreach (var name in TemporaryDictionary.Keys)
            {
                //Debug.Log(name);
                //Debug.Log(locations);
                if (!locations.ContainsItemWithName(name/*.ToLower()*/))
                {
                    error += string.Format("\nLocation \"{0}\" not found in inky list!", name);
                }
            }

            if (error == "")
            {
                return true;
            }
            else
            {
                Debug.LogAssertion("LOCATIONS NOT MATCHED UP" + error);
                return false;
            }
        }

        private bool PartyMembersRecognised(ListDefinition partyMembers)
        {
            PartyMemberDictionary.Clear();
            Dictionary<string, PartyMemberSO> TemporaryDictionary = new();
            foreach (PartyMemberSO mem in possiblePartyMembers)
            {
                TemporaryDictionary.Add(mem.CanonicalName/*.ToLower()*/, mem);
            }
            string error = "";

            // assert all party members from ink exist in unity
            foreach (InkListItem inkListItem in partyMembers.items.Keys)
            {
                if (TemporaryDictionary.TryGetValue(inkListItem.itemName/*.ToLower()*/, out PartyMemberSO item))
                {
                    item.InkListItem = inkListItem;
                    PartyMemberDictionary.Add(inkListItem, item);
                }
                else
                {
                    error += string.Format("\nMember \"{0}\" not found in unity dictionary!", inkListItem.itemName);
                }
            }
            // assert all party members from unity exist in ink
            foreach (var name in TemporaryDictionary.Keys)
            {
                //Debug.Log(name);
                //Debug.Log(locations);
                if (!partyMembers.ContainsItemWithName(name)) // THIS is where I can't force the code not to check for casing.
                {
                    error += string.Format("\nParty Member \"{0}\" not found in inky list!", name);
                }
            }

            if (error == "")
            {
                return true;
            }
            else
            {
                Debug.LogAssertion("PARTYMEMBER CHARACTERS NOT MATCHED UP" + error);
                return false;
            }
        }

        #endregion Private Methods
    }
}