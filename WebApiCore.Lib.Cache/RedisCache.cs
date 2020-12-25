using StackExchange.Redis;
using System;
using WebApiCore.Lib.Utils;

namespace WebApiCore.Lib.Cache
{
    internal class RedisCache : ICache, IDisposable
    {
        private readonly IDatabase _cache;
        private readonly ConnectionMultiplexer _connection;

        public RedisCache()
        {
            _connection = ConnectionMultiplexer.Connect(GlobalInvariant.SystemConfig.RedisConnectionString);
            _cache = _connection.GetDatabase();
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
                return _cache.StringSet(key, strValue);
            }
            else
            {
                return _cache.StringSet(key, strValue, (expireDateTime.Value - DateTime.Now));
            }

        }

        public bool RemoveCache(string key)
        {
            return _cache.KeyDelete(key);
        }

        public T GetCache<T>(string key)
        {
            var t = default(T);

            var value = _cache.StringGet(key);
            if (string.IsNullOrEmpty(value))
            {
                return t;
            }
            t = JsonHelper.ToObejct<T>(value);

            return t;
        }


        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Close();
            }
            GC.SuppressFinalize(this);
        }
    }
}
