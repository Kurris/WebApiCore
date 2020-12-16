using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiCore.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace WebApiCore.Core
{
    internal class CookieHelper
    {
        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="sName">名称</param>
        /// <param name="sValue">值</param>
        public void AddCookie(string sName, string sValue)
        {
            IHttpContextAccessor hca = GlobalInvariant.ServiceProvider?.GetService<IHttpContextAccessor>();
            CookieOptions option = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(15)
            };
            hca?.HttpContext?.Response.Cookies.Append(sName, sValue, option);
        }

        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="sName">名称</param>
        /// <param name="sValue">值</param>
        /// <param name="expires">过期时间(分钟)</param>
        public void AddCookie(string sName, string sValue, int expires)
        {
            IHttpContextAccessor hca = GlobalInvariant.ServiceProvider?.GetService<IHttpContextAccessor>();
            CookieOptions option = new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(expires)
            };
            hca?.HttpContext?.Response.Cookies.Append(sName, sValue, option);
        }

        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="sName">名称</param>
        /// <returns>cookie值</returns>
        public string GetCookie(string sName)
        {
            IHttpContextAccessor hca = GlobalInvariant.ServiceProvider?.GetService<IHttpContextAccessor>();
            return hca?.HttpContext?.Request.Cookies[sName];
        }


        /// <summary>
        /// 删除Cookie对象
        /// </summary>
        /// <param name="sName">Cookie对象名称</param>
        public void RemoveCookie(string name)
        {
            IHttpContextAccessor hca = GlobalInvariant.ServiceProvider?.GetService<IHttpContextAccessor>();
            hca?.HttpContext?.Response.Cookies.Delete(name);
        }
    }
}
