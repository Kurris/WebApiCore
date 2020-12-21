using Microsoft.AspNetCore.Http;
using WebApiCore.Utils;

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
            NetHelper.HttpContext.Session.SetString(key, value);
        }

        /// <summary>
        /// 读取Session的值
        /// </summary>
        /// <param name="key">Session的键名</param>        
        public string GetSession(string key)
        {
            return NetHelper.HttpContext.Session.GetString(key);
        }

        /// <summary>
        /// 删除指定Session
        /// </summary>
        /// <param name="key">Session的键名</param>
        public void RemoveSession(string key)
        {
            NetHelper.HttpContext.Session.Remove(key);
        }
    }
}
