
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
        protected MetaData metaData;
        public MetaData MetaData => metaData;
        #endregion
        #region backend

        #region Paths

        [Tooltip("Name of the folder to read from and write to")]
        [SerializeField]
        private string nameOfMasterDataDirectory = "PlayerData";
        public static string ActiveDataProfile
        {
            get { return Instance?_ActiveDataProfile:null; }
            private set
            {
                if (Instance != null)
                {
                    string path = Instance.DataProfileDirectory(value);
                    if (!Directory.Exists(path)) // create path if it doesn't exist. could maybe be useful as a static utility function tood
                    {
                        Directory.CreateDirectory(path);
                        //metaData = new();
                    }
                    _ActiveDataProfile = value;
                }
                else throw new Exception();
            }
        }
        private static string _ActiveDataProfile;
        [Tooltip("File extension to use.")]
        private string fileExtension = ".bin";
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

        public string DataProfile(string directory) => directory.Replace(MasterDataDirectory, "").Split('/')[0];
        public List<string> DataProfiles
        {
            get
            {
                List<string> output = new();
                foreach (string path in DataProfileDirectories)
                {
                    output.Add(DataProfile(path));
                }
                return output;
            }
        }

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
        public List<string> GetFilePaths(string profile) => new(Directory.GetFiles(DataProfileDirectory(profile)));

        /// <summary>
        /// Get savedata by profile and method
        /// </summary>
        /// <returns></returns>
        public List<string> GetFilePaths(string profile, SaveMethod method) // should later do this with metadata (such as savemethod field in the data) but for now doing it from the name is fine)
        {
            // get all save files
            var allFiles = GetFilePaths(profile);

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
        public List<string> GetFilePaths()
        {
            List<string> files = new();
            foreach (string profile in DataProfiles)
            {
                files.AddRange(GetFilePaths(profile));
            }
            return files;
        }
        #endregion 

        #endregion
        ///___METHODS___///
        protected override void Awake()
        {
            base.Awake();
            //DataMatrix = new();
            ActiveDataDictionary.Clear();
            reportedData = new();
            DontDestroyOnLoad(gameObject); // move this to parent persistantmonosingleton? or 
        }

        static List<DataClass> reportedData = new();

        public static void ReportDataExists<T>(T data) where T : DataClass
        {
            try
            {
                reportedData.Add(data);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #region saving
        public bool TryStartNewGame(string profileName)
        {
            if (DataProfiles.Contains(profileName))
            {
                Debug.LogWarning("Profile " +profileName+" already exists. Choose different profile name or delete existing.");
                return false;
            }
            else
            {
                ActiveDataDictionary.Clear();
                ActiveDataProfile = profileName;
                metaData = new();
                return true;
            }
        }
        //this is our save data structure.
        [Serializable] //needs to be marked as serializable
        struct MetaDataStruct
        {
            public string playerName;
            public float totalPlayTime;
            public float timeOnSave;
            public HashSet<Type> dataTypes;
            public HashSet<DataClass> DataClasses;
        }
        public enum SaveMethod
        {
            manual,
            auto,
            quick
        }
        public event Action OnDataSaved;
        public void SaveDataToFile(SaveMethod method) // for manuals hier ergens een override van profile naam? hm nee niet echt i guess
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
            string message = string.Format("{0} sets of data to {1}:", ActiveDataDictionary.Keys.Count, path);

            HashSet<DataClass> datas = new();
            // collect all the dataclasses in queue
            foreach (KeyValuePair<string, DataClass> dataClass in ActiveDataDictionary)
            {
                message += "\n" + dataClass.Key;
                datas.Add(dataClass.Value);
            }

            // form file content
            var data = new MetaDataStruct()
            {
                playerName = metaData.playerName,
                totalPlayTime = metaData.totalPlayTime,
                timeOnSave = Time.time,
                DataClasses = datas
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
                //Debug.Log("Sucessfully saved " + message);
            }
        }


        [Button("QuickSave", EButtonEnableMode.Playmode)]
        public void QuickSave() => SaveDataToFile(SaveMethod.quick);

        #endregion
        #region loading 

        /*
        // the container for various data profiles in accessable form (i.e. read from disk)
        private static Dictionary<string, Dictionary<string, DataClass>> DataMatrix { get; set; } = new();
        // the methods for accessing them:
        public Dictionary<string, DataClass> ActiveDataDictionary
        {
            get
            {
                if (ActiveDataProfile == null)
                {
                    return null;
                }
                else
                {
                    var output = GetDataDictionary(ActiveDataProfile);
                    
                    string test4 = "test4 dic in ActiveDataDictionary:";
                    foreach (var item in output)
                    {
                        test4 += "\n" + item;
                    }
                    Debug.Log(test4);
                    // deze lijkt ook soms goed
                    TestNextFrame = true;
                    return output;
                }
            }
        }
        public Dictionary<string, DataClass> GetDataDictionary(string profile)
        {
                Dictionary<string, DataClass> output = new();
                if (!DataMatrix.ContainsKey(profile))
                {
                    // need to create dicitonary
                    if (!DataMatrix.TryAdd(ActiveDataProfile, new()))
                    {
                        Debug.LogError("could not create and add dictionary");
                        return null;
                    }
                    else
                    {
                        Debug.Log("created dictionary!");
                        // at the end return true;
                    }
                }
                else
                {
                    // need to get dictionary
                    try
                    {
                        output = DataMatrix[ActiveDataProfile];
                    }
                    catch (Exception)
                    {
                        Debug.LogError("could not get dictionary");
                        throw;
                    }
                    if (output == null)
                    {
                        Debug.LogError("null dictionary");
                        return null;
                    }
                    else
                    {
                        Debug.Log("found dictionary for " + ActiveDataProfile);
                        // test datamatrix for this profile
                        string test3 = "test3 dic in GetDataDictionary:";
                        foreach (var item in output)
                        {
                            test3 += "\n" + item;
                        }
                        Debug.Log(test3);
                        // at the end return true;
                    }
                }
                // hier beland je als je de dictionary hebt gemaakt of gevonden

                // whether created or retreived, we should now have the dataprofile
                if (output == null)
                {
                    Debug.LogError("wtf man");
                    return null;
                }
                else
                {

                    string test1 = "test1 dic in GetDataDictionary, before CheckForReportedData:";
                    foreach (var item in output)
                    {
                        test1 += "\n" + item;
                    }
                    Debug.Log(test1);

                    if (!CheckForReportedData(ref output))
                    {
                        Debug.LogError("fout in de reported data toevoegen");
                        return output;
                    }
                    else
                    {

                        string test2 = "test2 dic in GetDataDictionary, after CheckForReportedData:";
                        foreach (var item in output)
                        {
                            test2 += "\n" + item;
                        }
                        Debug.Log(test2);

                        // DEZE IS MEESTAL WEL GOED
                        return output;
                    }
                }
        }
        */
        public Dictionary<string, DataClass> ActiveDataDictionary { get; } = new();
        public bool CheckForReportedData(ref Dictionary<string, DataClass> dictionaryToAddTo)
        {
            int added = 0;
            bool anyNew = false;
            string message = "attempt to fetch newly presented dataclasses:";
            foreach (var item in reportedData)
            {
                anyNew = true;

                string name = item.GetType().Name;

                if (!dictionaryToAddTo.ContainsKey(name))
                {
                    added++;
                    dictionaryToAddTo.Add(name, item);
                    message += String.Format("\n {0} was added", name);
                }
                else                    message += String.Format("\n {0} is already contained? wait isn't that weird?", name);
            }
            if (anyNew)
            {
                string test0 = "test0 dic from reporteddata:";
                foreach (var item in reportedData)
                {
                    test0 += "\n" + item;
                }
                Debug.Log(test0);

            }
            reportedData.Clear();
            if (anyNew)
            {
                if (added > 0)
                {
                    Debug.Log("succeeded in " + message); return true;
                }
                else
                {
                    Debug.Log("failed " + message); return false;
                }
            }
            else { Debug.Log("no new data things to add"); return true; }
        }

        public T GetDataOrMakeNew<T>() where T : DataClass, new()
        {
            string key = typeof(T).Name;
            if (!ActiveDataDictionary.TryGetValue(key, out var output))
            {
                if (output == null)
                {
                    Debug.Log("Creating new data of type " + key);
                    output = new T();
                }
                if (!ActiveDataDictionary.TryAdd(key, output)) // doersn't this mean the adding from reporteed data is superfluous?
                {
                    Debug.LogError("still not added");
                }
            }
            /*
            string test = "test dic from getdataormakenew:";
            foreach (var item in ActiveDataDictionary)
            {
                test += "\n"+item;
            }
            Debug.Log(test); 
            */
            return (T)output;
        }

        public void LoadMostRecent()//from any profile
        {
            LoadDataFromFile(GetMostRecentFile());
        }

        [Button("QuickLoad")]
        public void QuickLoad()
        {
            LoadDataFromFile(GetMostRecentFile(ActiveDataProfile, SaveMethod.quick));
        }
        public string GetMostRecentFile() // like the below but iterating over multiple profiles
        {
            string mostRecent = "PLACEHOLDER";
            DateTime record = DateTime.MinValue;
            foreach (string profile in DataProfiles)
            {
                foreach (string file in GetFilePaths(profile))
                {
                    DateTime contender = File.GetLastWriteTime(file);
                    if (contender > record)
                    {
                        record = contender;
                        mostRecent = file;
                    }
                }
            }
            return mostRecent;
        }
        public string GetMostRecentFile(string profile)
        {
            string mostRecent = "PLACEHOLDER";
            DateTime record = DateTime.MinValue;
            foreach (string file in GetFilePaths(profile))
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
        public string GetMostRecentFile(string profile, SaveMethod method)
        {
            string mostRecent = "PLACEHOLDER";
            DateTime record = DateTime.MinValue;
            foreach (string file in GetFilePaths(profile, method))
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
        public void LoadDataFromFile(string path, bool relaunchScene =true)
        {
            Debug.Log("attempting to load " + path);
            //check if file available
            if (!File.Exists(path)) throw new Exception("No file detected for path: " +path);

            string profile = DataProfile(path);

            //this is the formatter, you can also use an System.Xml.Serialization.XmlSerializer;
            var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

            // json formatting and deformatting apperantly unused
            //data = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));

            // declare data var
            MetaDataStruct loaded_data;       

            //again we open a filestream but now with fileMode.Open
            using (Stream filestream = File.Open(path, FileMode.Open)) 
            {
                //deserialize directly from that stream.
                loaded_data = (MetaDataStruct)formatter.Deserialize(filestream);
            }

            // NOTE een deel hiervan moet denk ik in de machine of controller gebeuren, ipv hier

            /*
            if (DataMatrix.ContainsKey(profile))
            {
                DataMatrix[profile].Clear(); 
            }
            else
            {
                DataMatrix.Add(profile, new()); // if we haven't yet prepped the data for that porofile this session, that's fine, just create the dictionary
            }*/
            ActiveDataDictionary.Clear();
            foreach (DataClass dataClass in loaded_data.DataClasses)
            {
                // DataMatrix[profile].Add(dataClass.GetType().Name, dataClass);
                if(ActiveDataProfile == profile)
                {
                    ActiveDataDictionary.Add(dataClass.GetType().Name, dataClass);
                }

                // hoe kan er hier al data in zijn??
                // oh er zijn meerdere van hetzelfde type, want een hashset voorkomt dat helemaal niet of wel?
            }
            metaData.timeSinceLastSave = 0;

            if (relaunchScene)
            {
                // after this is done the scene ashould be (re)launched.
                if (StoryController.Instance != null)
                {
                    StoryController.Instance.ResetSceneButton();
                }
                else
                {
                    //GameLauncher.Instance.LaunchGame(); done from gamelauncher already
                }
            }
        
        }


        #endregion
        #region resetting data


        public void WipeDataFromSlot(string profile)
        {
            DirectoryInfo dir = new(DataProfileDirectory(profile));
            dir.Delete(true);
            Debug.Log("Deleted data for profile " + profile);
        }

        [Button("Clear Data from this Profile")]
        public void WipeDataFromSlot()
        {
            WipeDataFromSlot(ActiveDataProfile);
        }

        [Button("Clear data from all profiles")]
        public void WipeDataFromAllSlots()
        {
            DirectoryInfo dir = new(MasterDataDirectory);
            dir.Delete(true);
            Debug.Log("Deleted data in all saveslots");
        }

        #endregion

        #region loop
        bool TestNextFrame = false;
        private void FixedUpdate()
        {
            metaData.totalPlayTime += Time.fixedDeltaTime;
            metaData.timeSinceLastSave += Time.fixedDeltaTime;

            if (TestNextFrame)
            {

                if (ActiveDataDictionary != null)
                {
                    string test5 = "test5 dic from update:";

                    foreach (var item in ActiveDataDictionary)
                    {
                        test5 += "\n" + item;
                    }
                    Debug.Log(test5);
                }
                TestNextFrame = false;
            }
        }
        #endregion
    }
    [Serializable]
    public class MetaData : DataClass
    {
        public float currentPlayTime = 0;// Time.realtimeSinceStartup;
        public float totalPlayTime = 0;
        public float timeSinceLastSave = 0;

        public string testText = "nulled";
        public const string textConst = "const";
        public string playerName = "Sam";
        public MetaData() : base() {}


    }




}