﻿using System;
using Microsoft.AspNetCore.Http;
using WebApiCore.Lib.Utils;

namespace WebApiCore.Core.TokenHelper
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
            CookieOptions option = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(15)
            };
            NetHelper.HttpContext.Response.Cookies.Append(sName, sValue, option);
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
            NetHelper.HttpContext.Response.Cookies.Append(sName, sValue, option);
        }

        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="sName">名称</param>
        /// <returns>cookie值</returns>
        public string GetCookie(string sName)
        {
            return NetHelper.HttpContext?.Request.Cookies[sName];
        }

        /// <summary>
        /// 删除Cookie对象
        /// </summary>
        /// <param name="sName">Cookie对象名称</param>
        public void RemoveCookie(string sName)
        {
            NetHelper.HttpContext?.Response.Cookies.Delete(sName);
        }
    }
}
