using StackExchange.Redis;

namespace Valuator.Storage
{
    public interface IRedisStorage
    {
        public Task AddKeyValue(string key, string value);

        public Task<string> GetValue(string id);

        public Task<string?> FindKeyByValueAsync(string value);
    }
}
