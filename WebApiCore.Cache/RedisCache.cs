using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiCore.Cache
{
   internal class RedisCache : ICache
    {
        public T GetCache<T>(string key)
        {
            throw new NotImplementedException();
        }

        public bool RemoveCache(string key)
        {
            throw new NotImplementedException();
        }

        public bool SetCache<T>(string key, T value, DateTime? expireDateTime = null)
        {
            throw new NotImplementedException();
        }
    }
}
