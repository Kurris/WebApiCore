using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiCore.Utils.Model
{
    public class SystemConfig
    {
        public DBConfig DBConfig { get; set; }

        /// <summary>
        /// 缓存引擎
        /// </summary>
        public string CacheProvider { get; set; }

        /// <summary>
        /// 登录引擎
        /// </summary>
        public string LoginProvider { get; set; }
    }
}
