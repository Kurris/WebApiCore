using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiCore.Cache
{
    public interface ICache
    {
        bool SetCache<T>(string key, T value, DateTime? expireTime = null);
        T GetCache<T>(string key);
        bool RemoveCache(string key);
    }
}
