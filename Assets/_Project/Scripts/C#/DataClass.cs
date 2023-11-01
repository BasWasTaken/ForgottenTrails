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
        public string Key => Label + GetType().Name;
        public DataClass()
        {
            Label = DataManager.ActiveDataProfile??"Master" + GetType().Name;
            DataManager.ReportDataExists(this);
        }

        public static explicit operator DataClass(UnityEngine.Object v)
        {
            throw new NotImplementedException();
        }
    }
}