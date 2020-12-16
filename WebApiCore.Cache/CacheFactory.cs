using WebApiCore.Utils;

namespace WebApiCore.Cache
{
    /// <summary>
    /// 缓存工厂
    /// </summary>
    public class CacheFactory
    {
        private static ICache _cache = null;
        private static readonly object _lock = new object();

        /// <summary>
        /// 缓存实例
        /// </summary>
        public static ICache Instance
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
