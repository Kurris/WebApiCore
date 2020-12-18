using StackExchange.Redis;
using System;
using WebApiCore.Utils;

namespace WebApiCore.Cache
{
    internal class RedisCache : ICache, IDisposable
    {
        private IDatabase cache;
        private ConnectionMultiplexer connection;

        public RedisCache()
        {
            connection = ConnectionMultiplexer.Connect(GlobalInvariant.SystemConfig.RedisConnectionString);
            cache = connection.GetDatabase();
        }

        public bool SetCache<T>(string key, T value, DateTime? expireDateTime = null)
        {

            string strValue = JsonHelper.ToJsonIgnoreLoop(value);
            if (string.IsNullOrEmpty(strValue))
            {
                return false;
            }

            if (expireDateTime == null)
            {
                return cache.StringSet(key, strValue);
            }
            else
            {
                return cache.StringSet(key, strValue, (expireDateTime.Value - DateTime.Now));
            }

        }

        public bool RemoveCache(string key)
        {
            return cache.KeyDelete(key);
        }

        public T GetCache<T>(string key)
        {
            var t = default(T);

            var value = cache.StringGet(key);
            if (string.IsNullOrEmpty(value))
            {
                return t;
            }
            t = JsonHelper.ToObejct<T>(value);

            return t;
        }


        public void Dispose()
        {
            if (connection != null)
            {
                connection.Close();
            }
            GC.SuppressFinalize(this);
        }
    }
}
