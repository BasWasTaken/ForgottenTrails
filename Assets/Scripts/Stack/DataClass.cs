using System;
using System.Reflection;
using NaughtyAttributes;
using UnityEngine;

namespace DataService
{
    /// <summary>
    /// Abstract class for savedata.
    /// </summary>
    [Serializable]
    public abstract class DataClass
    {
        public virtual string Label { get; set; }
        public string Key => Label + GetType().ToString();
        public DataClass(string label)
        {
            Label = label;
        }



        [Button("Manual Stash")]
        public void StashData()
        {
            DataManager.Instance.StashData(this);
        }
    }

    /*
    /// <summary>
    /// Abstract class for savedata.
    /// </summary>
    [Serializable]
    public abstract class DataClass
    {
        public abstract string Label { get; }
        public virtual string Key => Label + GetType().ToString();

        public DateTime created = DateTime.Now;
        public DateTime retrieved = DateTime.Now;
        public DateTime opened = DateTime.Now;
        public DateTime stashed = DateTime.Now;
        public DateTime saved = DateTime.Now;
        public virtual void OnCreation() { created = DateTime.Now; OnLoaded(); OnOpened(); OnStashed(); }
        public virtual void OnLoaded() { retrieved = DateTime.Now; }
        public virtual void OnOpened() { opened = DateTime.Now; }
        public virtual void OnStashed() { stashed = DateTime.Now; }
        public virtual void OnSaved() { saved = DateTime.Now; }

        public DataClass()
        {
            OnCreation();
        }

    }

    /// <summary>
    /// Normal class for non-singleton Data
    /// </summary>
    [Serializable]
    public abstract class InstanciableDataClass : DataClass
    {
        ///___VARIABLES___///
        #region INSPECTOR
        [SerializeField, Tooltip("Unique name for data object.")]
        public string label = "unnamed";
        public override string Label => label;
        #endregion
    }

    /// <summary>
    /// <para>Base Class for creating Singleton-variants of <see cref="DataClass"/>s, meaning that only one instance is active at any time and this instance can easily be reached using a static field.</para>
    /// </summary>
    [Serializable]
    public abstract class SingletonDataClass : DataClass
    {
        public override string Label => "";
    }

    public interface IContainsUniqueData<T> where T : SingletonDataClass
    {
        public T Data { get; set; }
        [Button("ResetData")]
        abstract void CreateBlankData();
        [Button("LoadData")]
        public void LoadData()
        {
            MethodInfo method = typeof(DataManager).GetMethod("GetDataSingle");
            MethodInfo genericMethod = method.MakeGenericMethod(typeof(T));
            T data = (T)genericMethod.Invoke(this, null);
            if (data == null)
            {
                DataManager.StashData(Data);
            }
            else
            {
                Data = data;
            }
        }
        [Button("SaveData")]
        public void SaveData() { DataManager.StashData(Data); }
    }
    public interface IContainsData<T> where T : InstanciableDataClass
    {
        public T Data { get; set; }
        public abstract void CreateBlankData();
        public virtual void LoadData()
        {
            MethodInfo method = typeof(DataManager).GetMethod("GetDataInstanciable");
            MethodInfo genericMethod = method.MakeGenericMethod(typeof(T));
            T data = (T)genericMethod.Invoke(this, new object[] {Data.Key});
            if (data == null)
            {
                DataManager.StashData(Data);
            }
            else
            {
                Data = data;
            }
        }
        public abstract void SaveData();
    } 
    */
}