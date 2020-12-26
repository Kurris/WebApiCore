using System;
using Microsoft.Extensions.Caching.Memory;
using WebApiCore.Lib.CacheAbstractions;

namespace WebApiCore.Lib.Cache
{
    internal class MemoryCache : ICache
    {
        private readonly IMemoryCache _cache = null;

        public MemoryCache(IMemoryCache cache)
        {
            _cache = cache;
        }

        public T GetCache<T>(string key)
        {
            return _cache.Get<T>(key);
        }

        public bool RemoveCache(string key)
        {
            _cache.Remove(key);
            return true;
        }

        public bool SetCache<T>(string key, T value, DateTime? expireTime = null)
        {
            try
            {
                if (expireTime == null)
                {
                    return _cache.Set<T>(key, value) != null;
                }
                else
                {
                    return _cache.Set(key, value, (expireTime.Value - DateTime.Now)) != null;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
