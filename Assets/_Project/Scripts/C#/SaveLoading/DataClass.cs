using System;

namespace Bas.ForgottenTrails.SaveLoading
{
    /// <summary>
    /// Abstract class for savedata.
    /// </summary>
    [Serializable]
    public abstract class DataClass
    {
        #region Public Constructors

        public DataClass()
        {
            Label = DataManager.ActiveDataProfile ?? "Master" + GetType().Name;
            DataManager.ReportDataExists(this);
        }

        #endregion Public Constructors

        #region Properties

        public virtual string Label { get; set; }
        public string Key => Label + GetType().Name;

        #endregion Properties

        #region Public Methods

        public static explicit operator DataClass(UnityEngine.Object v)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods
    }
}