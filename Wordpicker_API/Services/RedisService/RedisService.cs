using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using Wordpicker_API.Configs;

namespace Wordpicker_API.Services.RedisService
{
    public class RedisService : IRedisService
    {
        private readonly IAppConfigs _appConfigs;
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _database;

        public RedisService(IAppConfigs appConfigs) 
        {
            _appConfigs = appConfigs;
            _redis = ConnectionMultiplexer.Connect(_appConfigs.GetRedisConnection());
            _database = _redis.GetDatabase();
        }

        public async Task SetAsync(string key, string value)
        {
            await _database.StringSetAsync(key, value);
        }

        public async Task<string> GetAsync(string key)
        {
            var result = await _database.StringGetAsync(key);
            if (string.IsNullOrEmpty(result))
            {
                return "";
            }

            return result;
        }

        public async Task<bool> RemoveAsync(string key)
        {
            return await _database.KeyDeleteAsync(key);
        }
    }
}
