
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
    /// Allows other gameobjects to save and load data through use of <see cref="IDataService"/>.
    /// </summary>
    public class DataManager: MonoSingleton<DataManager>, IDataService
    {
        ///___VARIABLES___///
        #region INSPECTOR
        [Tooltip("Name of the folder to read from and write to")]
        [SerializeField, ReadOnly]
        private string masterFolder = "PlayerData";
        [Tooltip("SaveSlot to read from and write to.")]
        [SerializeField, Range(1,3)]
        private int saveSlot = 1;
        [Tooltip("File extension to use.")]
        [SerializeField, ReadOnly]
        private string fileExtension = ".json";

        [SerializeField]
        protected MetaData metaData = new("Meta02211281838");
        public MetaData MetaData => metaData;
        #endregion
        #region backend
        private readonly Dictionary<string, DataClass> dataQueue = new(); // data changed since last save and marked to be commited on the next save
        private readonly Dictionary<string, DataClass> dataCache = new(); // easy storage for accessed data (to re-access without having to read from disk again) 
                                                                                                  //  but do i need a seperate cache? anything both in the queue and cashe should nnot be gotten from cache.
                                                                                                  // perhaps i should make it so that on fetch, it first attempts to get it from queue, then cache, then disk.
                                                                                                  // nee, dit klopt niet helemaal, want ookal zit er stuff in de queue wil je het misschien juist terughalen van cache of disk!
        private const string key = "ggdPhkeOoiv6YMiPWa34kIuOdDUL7NwQFg6l1DVdwN8=";
        private const string iv = "JZuM0HQsWSBVpRHTeRZMYQ==";


        private string FolderPath()
        {
            string masterFolderPath = Application.persistentDataPath + "/" + masterFolder;
            string subFolderPath = masterFolderPath + "/" + "Slot" + saveSlot;
            if (!Directory.Exists(masterFolderPath))
            {
                Directory.CreateDirectory(masterFolderPath);
            }
            if (!Directory.Exists(subFolderPath))
            {
                Directory.CreateDirectory(subFolderPath);
            }
            return subFolderPath;
        }
        private string DataPath(string dataKey) 
        {
            return FolderPath() + "/" + dataKey + fileExtension; 
        }
        #endregion
        ///___METHODS___///
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }
        #region saving
        public bool StashData<T>(T data, bool encrypted = false) where T : DataClass
        {
            metaData.stashed = DateTime.Now.Ticks;
            // NOTE: not doing anything with the encrypted bool, and it's problematic to employ here
            try
            {
                if (dataQueue.TryAdd(data.Key, data))
                { /// if we added the data to queue...

                    if (!dataCache.TryAdd(data.Key, data))
                    {/// if  the data was already in cashe, update it
                        dataCache[data.Key] = data;
                    }
                }
                else
                { /// if the data was already in queue (and thus cache) update both

                    dataQueue[data.Key] = data;
                    dataCache[data.Key] = data;
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Unable to stash data due to: {e.Message} {e.StackTrace}");
                return false;
            }

            /// at the end of this, both queue and cache should contain this data and it is primed for saving to disk

// i have to stash quite often with this.
        }
        [Button("SaveData")]
        public void WriteStashedDataToDisk(bool encrypted = false)
        {
            string message ="Saved following data:";
            int i = 0;
            foreach (DataClass data in dataQueue.Values)
            {
                i++;
                WriteDataToDisk(data, encrypted);
                //TODO: metadata.onsaved
                message += "\n"+data.Key;
            }
            // Clear both que of to-be-saved data as well as the cache of recently viewed data (to avoid conflict with the data we just saved)
            dataQueue.Clear();
            dataCache.Clear();
            if (i==0)
            {
                message = "Dit not save anything.";
            }
                Debug.Log(message);
            metaData.written = DateTime.Now.Ticks;
            WriteDataToDisk(metaData);
        }

        private bool WriteDataToDisk<T>(T data, bool encrypted= false) where T : DataClass
        {
            string path = DataPath(data.Key);

            try
            {
                if (File.Exists(path))
                {
//                    Debug.Log("Data exists. Deleting old file and writing a new one!");
                    //File.Delete(path); onnodig; .creat kan gewoon overwriten
                }
                else
                {
//                    Debug.Log("Writing file for the first time!");
                }
                using FileStream stream = File.Create(path);
                if (encrypted)
                {
                    WriteEncryptedDataToDisk(data, stream); // waarom wordt hieronder de stream niet geclosed? ook niet meer in de enclused functie
                }
                else
                {
                    stream.Close();
                    File.WriteAllText(path, JsonConvert.SerializeObject(data));
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Unable to save data due to: {e.Message} {e.StackTrace}");
                return false;
            }
        }
        /// <summary>
        /// from https://github.com/llamacademy/persistent-data/
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Data"></param>
        /// <param name="Stream"></param>
        private void WriteEncryptedDataToDisk<T>(T Data, FileStream Stream)
        {
            Debug.LogWarning("This method is untested and may not be fully integrated!");
            using Aes aesProvider = Aes.Create();
            aesProvider.Key = Convert.FromBase64String(key);
            aesProvider.IV = Convert.FromBase64String(iv);
            using ICryptoTransform cryptoTransform = aesProvider.CreateEncryptor();
            using CryptoStream cryptoStream = new(
                Stream,
                cryptoTransform,
                CryptoStreamMode.Write
            );

            // You can uncomment the below to see a generated value for the IV & key.
            // You can also generate your own if you wish
            //Debug.Log($"Initialization Vector: {Convert.ToBase64String(aesProvider.IV)}");
            //Debug.Log($"Key: {Convert.ToBase64String(aesProvider.Key)}");
            cryptoStream.Write(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(Data)));
        }
        #endregion
        #region loading
        public bool DataAvailable(string key)
        {
            return File.Exists(DataPath(key));
        }
        public T FetchData<T>(string key, bool encrypted=false) where T:DataClass
        {
            metaData.fetched = DateTime.Now.Ticks;
            if (dataCache.TryGetValue(key, out DataClass dataOut))
            {/// if it's available in cache, return it from there.
         //       Debug.Log("Data succesfully fetched from cache");
                return (T)dataOut;
            }
            else
            {///otherwise read it from disk

                return ReadDataFromDisk<T>(key, encrypted);
            }
        }

        [Button("LoadData")]
        public void LoadAllDataFromDisk()
        {
            throw new NotImplementedException();
            // iets van foreach file found en dan in de stash gooien? maar tbh kan ik dit beter gewoon niet gebruiken denk ik, en data loaden as needed
        }
        
        public T ReadDataFromDisk<T>(string key, bool encrypted=false) where T : DataClass
        {
            metaData.read = DateTime.Now.Ticks;
            string path = DataPath(key);

            if (!File.Exists(path))
            {/// if file does not exist, throw and exit
                Debug.LogError($"Cannot load file at {path}. File does not exist!");
                throw new FileNotFoundException($"{path} does not exist!");
            }
            try
            {
                T data;
                if (encrypted)
                {
                    data = ReadEncryptedDataFromDisk<T>(path);
                }
                else
                {
                    data = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
                }

                if (!dataCache.TryAdd(data.Key, data))
                {/// if since the last time we checked, the data is already in cache... (then this was probably put there from the savemethod)
                    Debug.LogWarning("Data got added to cache while loading from disk. Likely there is now a conflict between queue and cache. Overwriting cache with data from queue. (queue->cache)");
                    ///maintain the data in queue.
                    data = (T)dataCache[data.Key];
                }
                return data;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load data due to: {e.Message} {e.StackTrace}");
                throw e;
            }
        }

        private T ReadEncryptedDataFromDisk<T>(string Path)
        {
            Debug.LogWarning("This method is untested and may not be fully integrated!");
            byte[] fileBytes = File.ReadAllBytes(Path);
            using Aes aesProvider = Aes.Create();

            aesProvider.Key = Convert.FromBase64String(key);
            aesProvider.IV = Convert.FromBase64String(iv);

            using ICryptoTransform cryptoTransform = aesProvider.CreateDecryptor(
                aesProvider.Key,
                aesProvider.IV
            );
            using MemoryStream decryptionStream = new(fileBytes);
            using CryptoStream cryptoStream = new(
                decryptionStream,
                cryptoTransform,
                CryptoStreamMode.Read
            );
            using StreamReader reader = new(cryptoStream);

            string result = reader.ReadToEnd();

            Debug.Log($"Decrypted result (if the following is not legible, probably wrong key or iv): {result}");
            return JsonConvert.DeserializeObject<T>(result);
        }
        #endregion
        #region resetting data
        public void RemoveFromCache<T>(string key) where T: DataClass
        {
            if (dataCache.ContainsKey(key))
            {
                dataCache.Remove(key);
            }
            else
            {
                Debug.LogWarning("attempted to remove data that didn't exist in cache.");
            }
        }

        [Button("Clear Data From Disk", EButtonEnableMode.Editor)]
        protected void WipeAllData()
        {
            DirectoryInfo dir = new(FolderPath());
            foreach (FileInfo file in dir.GetFiles()) //shouldn't this loop be inside the other loop?
            {
                file.Delete();
            }
            foreach (DirectoryInfo subdir in dir.GetDirectories())
            {
                subdir.Delete(true);
            }
            dir.Delete();
            Debug.Log("Deleted folder");
        }
        #endregion

        #region loop
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                Debug.Log(MetaData.Report);
            }
            else if (Input.GetKeyDown(KeyCode.N))
            {
                Debug.Log(FetchData<MetaData>(MetaData.Key).Report);
            }
            else if (Input.GetKey(KeyCode.C))
            {
                if (Input.GetKeyDown(KeyCode.S))
                {
                    WriteStashedDataToDisk();
                }
            }
        }
        #endregion
    }
    [Serializable]
    public class MetaData : DataClass
    {
        public long created, fetched, read, stashed, written = DateTime.Now.Ticks;

        public string testText = "nulled";
        public const string textConst = "const";

        public string Report
        {
            get 
            {
                return 
                    "Timestamps Below." +
                    "\nCreated:\t" + created + 
                    ".\nStashed:\t" + stashed +
                    ".\nWritten:\t" + written +
                    ".\nFetched:\t" + fetched +
                    ".\nRead:\t" + read + ".";
            }
        }

        public MetaData(string label) : base(label) {}


    }




}