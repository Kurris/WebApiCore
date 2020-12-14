﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApiCore.Utils;
using WebApiCore.Utils.Extensions;

namespace Ligy.Project.WebApi.CustomClass
{

    public class ApiAuthAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly string[] _permission;

        public ApiAuthAttribute(params string[] roles)
        {
            this._permission = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var claimsIndentity = context.HttpContext.User.Identity as ClaimsIdentity;

            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                var expTimeStr = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "exp")?.Value;
                var expTimeLong = expTimeStr.ParseToLong();
                var expTime = DateTimeHelper.GetUnixTimeStamp(new DateTime(1970, 1, 1, 8, 0, 0).AddSeconds(expTimeLong));
                var nowTime = DateTimeHelper.GetUnixTimeStamp(DateTime.Now);

                if (expTime - nowTime < 60 * 60 * 10)
                {
                    context.HttpContext.Response.Cookies.Append("access_token", JwtHelper.GenerateToken(new WebApiCore.Entity.SystemManager.User() { UserId = 1, Name = "ligy" }));
                }

                if (this._permission == null || this._permission.Length == 0) return;
                var claimJson = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "permission")?.Value;

                //TODO Check Permission
                bool roleFlag = false;
                if (!roleFlag)
                {
                    context.Result = new ObjectResult(
                                                       new ResultModel(
                                                       code: 401,
                                                       message: "当前没有操作权限",
                                                       result: null,
                                                       returnStatus: ReturnStatus.Fail)
                                                       );
                }

            }
            else
            {

                context.Result = new ObjectResult(
                    new ResultModel(
                    code: 401,
                    message: "授权失败",
                    result: null,
                    returnStatus: ReturnStatus.Fail)
                    );
            }
        }
    }
}