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
}