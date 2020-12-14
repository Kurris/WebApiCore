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
            throw new NotImplementedException();
        }    

        public bool SetCache<T>(string key, T value, DateTime? expireTime = null)
        {
            throw new NotImplementedException();
        }
    }
}
