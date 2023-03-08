namespace DataService 
{
    public interface IDataService
    {
        bool StashData<T>(T data, bool encrypted=false) where T : DataClass;

        T FetchData<T>(string key, bool encrypted=false) where T : DataClass;
    }
}
