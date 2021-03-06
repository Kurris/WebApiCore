﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiCore.Lib.Utils.Config
{
    /// <summary>
    /// Jwt配置
    /// </summary>
    public class JwtConfig
    {
        /// <summary>
        /// Token名称
        /// </summary>
        public string TokenName { get; set; }

        /// <summary>
        /// Token秘钥
        /// </summary>
        public string TokenSecretKey { get; set; }

        /// <summary>
        /// 颁发者
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// 受众
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// 失效时间(min)
        /// </summary>
        public int Expiration { get; set; }
    }
}
