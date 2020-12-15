using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using WebApiCore.Utils;

namespace WebApiCore.Cache
{
    internal class MemoryCache : ICache
    {
        private IMemoryCache _cache = GlobalInvariant.ServiceProvider.GetService<IMemoryCache>();

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
