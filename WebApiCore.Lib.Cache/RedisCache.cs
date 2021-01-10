using System;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using WebApiCore.Lib.CacheAbstractions;
using WebApiCore.Lib.Utils;

namespace WebApiCore.Lib.Cache
{
    internal class RedisCache : ICache, IDisposable
    {
        private readonly IDatabase _cache;
        private readonly IConfiguration _configuration;

        public RedisCache(IConfiguration configuration)
        {
            this._configuration = configuration;
            string redisConnString = this._configuration.GetSection("SystemConfig:RedisConnectionString").Value;

            _cache = ConnectionMultiplexer.Connect(redisConnString).GetDatabase();
        }

        public bool SetCache<T>(string key, T value, DateTime? expireDateTime = null)
        {

            string strValue = JsonHelper.ToJson(value, new JsonSetting() { LoopHandling = LoopHandling.Ignore });
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
            if (_cache.Multiplexer != null)
            {
                _cache.Multiplexer.Close();
                _cache.Multiplexer.Dispose();
            }
            GC.SuppressFinalize(this);
        }
    }
}
