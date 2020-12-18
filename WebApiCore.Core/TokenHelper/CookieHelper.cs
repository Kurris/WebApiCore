using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using WebApiCore.Utils;

namespace WebApiCore.Core.TokenHelper
{
    internal class CookieHelper
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
        /// 写cookie值
        /// </summary>
        /// <param name="sName">名称</param>
        /// <param name="sValue">值</param>
        public void AddCookie(string sName, string sValue)
        {
            CookieOptions option = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(15)
            };
            HttpContextAccessor.HttpContext.Response.Cookies.Append(sName, sValue, option);
        }

        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="sName">名称</param>
        /// <param name="sValue">值</param>
        /// <param name="expires">过期时间(分钟)</param>
        public void AddCookie(string sName, string sValue, int expires)
        {
            CookieOptions option = new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(expires)
            };
            HttpContextAccessor.HttpContext.Response.Cookies.Append(sName, sValue, option);
        }

        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="sName">名称</param>
        /// <returns>cookie值</returns>
        public string GetCookie(string sName)
        {
            return HttpContextAccessor.HttpContext?.Request.Cookies[sName];
        }

        /// <summary>
        /// 删除Cookie对象
        /// </summary>
        /// <param name="sName">Cookie对象名称</param>
        public void RemoveCookie(string sName)
        {
            HttpContextAccessor.HttpContext?.Response.Cookies.Delete(sName);
        }
    }
}
