﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiCore.Lib.Utils;
using WebApiCore.Lib.Utils.Model;

namespace WebApiCore.CustomClass
{
    /// <summary>
    /// 全局管道异常处理
    /// </summary>
    public class GlobalMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalMiddleware> _logger;

        public GlobalMiddleware(RequestDelegate next, ILogger<GlobalMiddleware> logger)
        {
            this._next = next;
            this._logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);

                if (!context.User.Identity.IsAuthenticated
                 && !context.Request.Path.StartsWithSegments("/api/User/Login")//登录和注册不需要验证
                 && !context.Request.Path.StartsWithSegments("/api/User/SignUp"))
                {
                    string result = JsonHelper.ToJson(new TData<string>()
                    {
                        Data = null,
                        Message = "授权失败",
                        Status = Status.AuthorizationFail
                    });
                    byte[] content = Encoding.UTF8.GetBytes(result);

                    context.Response.ContentType = "application/json";
                    context.Response.ContentLength = content.Length;
                    await context.Response.BodyWriter.WriteAsync(new ReadOnlyMemory<byte>(content));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}