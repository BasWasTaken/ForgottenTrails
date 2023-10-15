
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
    /// Allows other gameobjects to save and load data through use of <see cref="IDataService"/>.
    /// </summary>
    public class DataManager : MonoSingleton<DataManager>
    {
        ///___VARIABLES___///
        #region INSPECTOR

        [SerializeField]
        protected MetaData metaData = new("Meta202305142002");
        public MetaData MetaData => metaData;
        #endregion
        #region backend

        #region Paths

        [Tooltip("Name of the folder to read from and write to")]
        [SerializeField, ReadOnly]
        private string nameOfMasterDataDirectory = "PlayerData";
        public string ActiveDataProfile
        {
            get { return _ActiveDataProfile; }
            private set
            {
                string path = DataProfileDirectory(value);
                if (!Directory.Exists(path)) // create path if it deosn't exist. could maybe be useful as a static utility function tood
                {
                    Directory.CreateDirectory(path);
                }
                _ActiveDataProfile = value;
            }
        }
        private string _ActiveDataProfile;
        [Tooltip("File extension to use.")]
        [SerializeField, ReadOnly]
        private string fileExtension = ".json";
        /// <summary>
        /// get folder containing all data (in subfolders)
        /// </summary>
        public string MasterDataDirectory
        {
            get
            {
                string masterFolderPath = Application.persistentDataPath + "/" + nameOfMasterDataDirectory + "/"; // note creates incompatability with macos thorugh slashes
                if (!Directory.Exists(masterFolderPath)) Directory.CreateDirectory(masterFolderPath);
                return masterFolderPath;
            }
        }

        /// <summary>
        /// get all existing data profiles
        /// </summary>
        public string[] DataProfileDirectories => Directory.GetDirectories(MasterDataDirectory);

        /// <summary>
        /// get a specific data profile
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        public string DataProfileDirectory(string profile) => MasterDataDirectory + profile + "/";

        /// <summary>
        /// get the active save profile
        /// </summary>
        public string ActiveDataProfileDirectory => DataProfileDirectory(ActiveDataProfile);

        private string GetPathForSaving(SaveMethod method)
        {
            return GetPathForSaving(ActiveDataProfile, method);
        }
        private string GetPathForSaving(string profile, SaveMethod method)
        {
            return DataProfileDirectory(profile) + method.ToString() + "save at " + Time.time + fileExtension;
        }

        /// <summary>
        /// Get savedata by profile
        /// </summary>
        /// <returns></returns>
        public List<string> GetSaveFiles(string profile) => new(Directory.GetFiles(DataProfileDirectory(profile)));

        /// <summary>
        /// Get savedata by profile and method
        /// </summary>
        /// <returns></returns>
        public List<string> GetSaveFiles(string profile, SaveMethod method) // should later do this with metadata (such as savemethod field in the data) but for now doing it from the name is fine)
        {
            // get all save files
            var allFiles = GetSaveFiles(profile);

            // then filter by method
            List<string> filtered = new();
            foreach (string file in allFiles)
            {
                if (file.Contains(method.ToString())) filtered.Add(file);
            }
            return filtered;
        }

        /// <summary>
        /// get all savedata across all profiles
        /// </summary>
        /// <returns></returns>
        public List<string> GetSaveFiles()
        {
            List<string> files = new();
            foreach (string profile in DataProfileDirectories)
            {
                files.AddRange(GetSaveFiles(profile));
            }
            return files;
        }
        #endregion 

        #endregion
        ///___METHODS___///
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        #region saving
        public bool TryStartNewGame(string profileName)
        {
            if (File.Exists(GetPathForSaving(profileName, SaveMethod.auto)))
            {
                Debug.LogWarning("Profile already exists. Choose different profile name or delete existing.");
                return false;
            }
            else
            {
                ActiveDataProfile = profileName;
                return true;
            }
        }

        public HashSet<DataClass> ActiveData 
        {
            get
            {
                HashSet<DataClass> output = new();
                foreach (var item in FindObjectsOfType(typeof(DataClass)))
                {
                    output.Add((DataClass)item);
                }
                return output;
            }
        }
        //this is our save data structure.
        [Serializable] //needs to be marked as serializable
        struct MetaDataStruct
        {
            public string playerName;
            public float totalPlayTime;
            public float timeOnSave;
            public DataClass[] DataClasses;
        }
        public enum SaveMethod
        {
            manual,
            auto,
            quick
        }
        public event Action OnDataSaved;
        public void SaveDataToFile(SaveMethod method)
        {
            // exit if manual since i haven't done that yet
            if (method == SaveMethod.manual)
            {
                throw new NotImplementedException();
            }

            //this is the formatter, you can also use an System.Xml.Serialization.XmlSerializer;
            var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

            // determine path
            string path = GetPathForSaving(method);
            // prepare debug message
            string message = string.Format("{0} sets of data to {1}:", ActiveData.Count, path);

            List<DataClass> datas = new();
            // collect all the dataclasses in queue
            foreach (DataClass dataClass in ActiveData)
            {
                message += "\n" + dataClass.Key;
                datas.Add(dataClass);
            }

            // form file content
            var data = new MetaDataStruct()
            {
                playerName = metaData.playerName,
                totalPlayTime = metaData.totalPlayTime,
                timeOnSave = Time.time,
                DataClasses = datas.ToArray()
            };

            //open a filestream to save on
            //notice there is no need to close or flush the stream as it will do it before disposing at the end of the using block.
            using (Stream filestream = File.Open(path, FileMode.Create))
            {
                //serialize directly into that stream.
                formatter.Serialize(filestream, data);
            }

            metaData.timeSinceLastSave = 0;
            // Call the OnDataSaved event/callback, if there are subscribers
            OnDataSaved?.Invoke();
            if (datas.Count == 0)
            {
                Debug.LogError("Failed to save " + message);
            }
            else
            {
                Debug.Log("Sucessfully saved " + message);
            }
        }


        [Button("QuickSave", EButtonEnableMode.Playmode)]
        public void QuickSave() => SaveDataToFile(SaveMethod.quick);

        #endregion
        #region loading 

        public Dictionary<string, DataClass> DataDictionary = new();

        public void LoadMostRecent()
        {
            LoadDataFromFile(GetMostRecentFile());
        }

        [Button("QuickLoad")]
        public void QuickLoad()
        {
            LoadDataFromFile(GetMostRecentFile(SaveMethod.quick));
        }

        public string GetMostRecentFile(string profile)
        {
            string mostRecent = "";
            DateTime record = DateTime.MinValue;
            foreach (string file in GetSaveFiles(profile))
            {
                DateTime contender = File.GetLastWriteTime(file);
                if (contender > record)
                {
                    record = contender;
                    mostRecent = file;
                }
            }
            return mostRecent;
        }
        public string GetMostRecentFile()
        {
            return GetMostRecentFile(ActiveDataProfile);
        }
        public string GetMostRecentFile(string profile, SaveMethod method)
        {
            string mostRecent = "";
            DateTime record = DateTime.MinValue;
            foreach (string file in GetSaveFiles(profile, method))
            {
                DateTime contender = File.GetLastWriteTime(file);
                if (contender > record)
                {
                    record = contender;
                    mostRecent = file;
                }
            }
            return mostRecent;
        }
        public string GetMostRecentFile(SaveMethod method)
        {
            return GetMostRecentFile(ActiveDataProfile, method);
        }

        /// <summary>
        /// used when loading save file (during reload or startup)
        /// </summary>
        public void LoadDataFromFile(string path)
        {
            //check if file available
            if (!File.Exists(path)) throw new Exception();

            //this is the formatter, you can also use an System.Xml.Serialization.XmlSerializer;
            var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

            // json formatting and deformatting apperantly unused
            //data = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));

            // declare data var
            MetaDataStruct loaded_data;       

            //again we open a filestream but now with fileMode.Open
            using (Stream filestream = File.Open("filename.dat", FileMode.Open))
            {
                //deserialize directly from that stream.
                loaded_data = (MetaDataStruct)formatter.Deserialize(filestream);
            }

            // NOTE een deel hiervan moet denk ik in de machine of controller gebeuren, ipv hier
            DataDictionary.Clear();
            foreach (DataClass dataClass in loaded_data.DataClasses)
            {
                DataDictionary.Add(dataClass.ToString(), dataClass);
            }
            metaData.timeSinceLastSave = 0;

            // after this is done the scene ashould be (re)launched.
        }


        #endregion
        #region resetting data


        public void WipeDataFromSlot(string profile)
        {
            DirectoryInfo dir = new(DataProfileDirectory(profile));
            dir.Delete(true);
            Debug.Log("Deleted data for profile " + profile);
        }

        [Button("Clear Data from this Profile", EButtonEnableMode.Editor)]
        public void WipeDataFromSlot()
        {
            WipeDataFromSlot(ActiveDataProfile);
        }

        [Button("Clear data from all profiles", EButtonEnableMode.Editor)]
        public void WipeDataFromAllSlots()
        {
            DirectoryInfo dir = new(MasterDataDirectory);
            dir.Delete(true);
            Debug.Log("Deleted data in all saveslots");
        }

        #endregion

        #region loop
        private void FixedUpdate()
        {
            metaData.totalPlayTime += Time.fixedDeltaTime;
            metaData.timeSinceLastSave += Time.fixedDeltaTime;
        }
        #endregion
    }
    [Serializable]
    public class MetaData : DataClass
    {
        public float currentPlayTime = Time.realtimeSinceStartup;
        public float totalPlayTime = 0;
        public float timeSinceLastSave = 0;

        public string testText = "nulled";
        public const string textConst = "const";
        public string playerName = "Sam";
        public MetaData(string label) : base(label) {}


    }




}