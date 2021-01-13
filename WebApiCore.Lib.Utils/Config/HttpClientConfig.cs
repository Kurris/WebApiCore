using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiCore.Lib.Utils.Config
{
    public class HttpClientConfig
    {
        public string BaseUrl { get; set; }
        public int Timeout { get; set; }
    }
}
