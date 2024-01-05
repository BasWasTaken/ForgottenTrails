using Bas.ForgottenTrails.SaveLoading;

namespace SaveLoading
{
    public interface IDataService
    {
        #region Public Methods

        bool StashData<T>(T data, bool encrypted = false) where T : DataClass;

        T FetchData<T>(string key, bool encrypted = false) where T : DataClass;

        #endregion Public Methods
    }
}