using WebApiCore.Utils;

namespace WebApiCore.Cache
{
    public class CacheFactory
    {
        private static ICache _cache = null;
        private static readonly object _lock = new object();

        /// <summary>
        /// 缓存
        /// </summary>
        public static ICache Cache
        {
            get
            {

                if (_cache == null)
                {
                    lock (_lock)
                    {
                        if (_cache == null)
                        {
                            _cache = GlobalInvariant.SystemConfig.CacheProvider switch
                            {
                                "Redis" => new RedisCache(),
                                _ => new MemoryCache(),
                            };
                        }
                    }
                }
                return _cache;
            }
        }
    }
}
