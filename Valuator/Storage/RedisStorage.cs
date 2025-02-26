using StackExchange.Redis;
using System.Text.Json;

namespace Valuator.ViewModel
{
    public class RedisStorage 
    {
        private readonly IConnectionMultiplexer _redis;

        public RedisStorage(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }
        public async Task AddKeyValue(string key, string value)
        {
            var db = _redis.GetDatabase();
            await db.StringSetAsync(key, value);
        }

        public async Task<string> GetValue(string id)
        {
            var db = _redis.GetDatabase();
            var result = await db.StringGetAsync(id);
            return result.IsNullOrEmpty ? "" : result.ToString();
        }

        public async Task<string?> FindKeyByValueAsync(string value, string existingKey)
        {
            var db = _redis.GetDatabase();
            var server = _redis.GetServer(_redis.GetEndPoints().First());

            foreach (RedisKey key in server.Keys(pattern: "TEXT-*"))
            {
                if (key.ToString() == existingKey)
                {
                    continue;
                }
                var keyValue = await db.StringGetAsync(key);
                if (keyValue.ToString() == value)
                {
                    return key;
                }
            }

            return null;
        }

    }
}
