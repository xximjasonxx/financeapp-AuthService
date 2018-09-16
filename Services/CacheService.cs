
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace AuthService.Services
{
    public static class CacheService
    {
        static IDatabase GetDatabase()
        {
            string connectionString = Environment.GetEnvironmentVariable("RedisConnection", EnvironmentVariableTarget.Process);
            ConnectionMultiplexer redisConnection = ConnectionMultiplexer.Connect(connectionString);

            return redisConnection.GetDatabase();
        }

        public static async Task WriteKeyAsync(string keyName, object value)
        {
            var database = GetDatabase();

            var jsonString = JsonConvert.SerializeObject(value);
            await database.StringSetAsync(keyName, jsonString, TimeSpan.FromDays(1));
        }

        public static async Task<T> GetValueForKey<T>(string keyName)
        {
            var database = GetDatabase();
            var stringValue = await database.StringGetAsync(keyName);
            if (string.IsNullOrEmpty(stringValue))
                return default(T);

            return JsonConvert.DeserializeObject<T>(stringValue);
        }
    }
}