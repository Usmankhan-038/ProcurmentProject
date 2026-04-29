namespace ProcurmentProject.Interfaces
{
    public interface ICacheService
    {
        public Task<T> GetOrSetCache<T>(string cacheKey, Func<Task<T>> cacheFunction, TimeSpan? expiration=null);

        public void RemoveCache(string key);

        public T? GetCache<T>(string key);
    }
}
