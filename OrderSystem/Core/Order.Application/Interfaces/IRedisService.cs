namespace Order.Application.Interfaces
{
    public interface IRedisService
    {
        Task<string?> GetAsync(string key);
        Task SetAsync(string key, string value);
        Task RemoveAsync(string key);
    }
}
