using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiCore.Utils.Model
{
    public class SystemConfig
    {
        public DBConfig DBConfig { get; set; }
        public string CacheProvider { get; set; }
    }
}
