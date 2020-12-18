using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using WebApiCore.Utils;

namespace WebApiCore.Core.TokenHelper
{
    internal class SessionHelper 
    {

        private IHttpContextAccessor HttpContextAccessor
        {
            get
            {
                IHttpContextAccessor httpContextAccessor = GlobalInvariant.ServiceProvider?.GetService<IHttpContextAccessor>();
                if (httpContextAccessor == null)
                {
                    throw new NullReferenceException("HttpContext对象为NULL");
                }

                return httpContextAccessor;
            }
        }

        /// <summary>
        /// 写Session
        /// </summary>
        /// <param name="key">Session的键名</param>
        /// <param name="value">Session的键值</param>
        public void AddSession(string key, string value)
        {
            HttpContextAccessor.HttpContext.Session.SetString(key, value);
        }

        /// <summary>
        /// 读取Session的值
        /// </summary>
        /// <param name="key">Session的键名</param>        
        public string GetSession(string key)
        {
            return HttpContextAccessor.HttpContext.Session.GetString(key);
        }

        /// <summary>
        /// 删除指定Session
        /// </summary>
        /// <param name="key">Session的键名</param>
        public void RemoveSession(string key)
        {
            HttpContextAccessor.HttpContext.Session.Remove(key);
        }
    }
}
