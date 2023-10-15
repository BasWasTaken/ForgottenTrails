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
            try
            {
                DataManager.DataMatrix[DataManager.Instance.ActiveDataProfile].Add(this.GetType().Name, this);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static explicit operator DataClass(UnityEngine.Object v)
        {
            throw new NotImplementedException();
        }
    }
}