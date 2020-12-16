using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiCore.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using WebApiCore.Utils.Extensions;

namespace WebApiCore.Core.TokenHelper
{
    internal class SessionHelper 
    {

        /// <summary>
        /// 写Session
        /// </summary>
        /// <param name="key">Session的键名</param>
        /// <param name="value">Session的键值</param>
        public void AddSession(string key, string value)
        {
            if (key.IsEmpty())
            {
                throw new NullReferenceException(key);
            }
            IHttpContextAccessor hca = GlobalInvariant.ServiceProvider?.GetService<IHttpContextAccessor>();
            hca.HttpContext.Session.SetString(key, value);
        }

        /// <summary>
        /// 读取Session的值
        /// </summary>
        /// <param name="key">Session的键名</param>        
        public string GetSession(string key)
        {
            if (key.IsEmpty())
            {
                return string.Empty;
            }
            IHttpContextAccessor hca = GlobalInvariant.ServiceProvider?.GetService<IHttpContextAccessor>();
            return hca.HttpContext.Session.GetString(key);
        }

        /// <summary>
        /// 删除指定Session
        /// </summary>
        /// <param name="key">Session的键名</param>
        public void RemoveSession(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }
            IHttpContextAccessor hca = GlobalInvariant.ServiceProvider?.GetService<IHttpContextAccessor>();
            hca.HttpContext.Session.Remove(key);
        }
    }
}
