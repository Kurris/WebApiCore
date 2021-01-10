using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApiCore.Lib.Utils;
using WebApiCore.Lib.Utils.Extensions;
using WebApiCore.Lib.Model;

namespace WebApiCore.CustomClass
{

    /// <summary>
    /// 自定义Auth鉴权
    /// </summary>
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
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                var expTimeStr = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "exp")?.Value;
                var expTimeLong = expTimeStr.ParseToLong();
                var expTime = DateTimeHelper.GetUnixTimeStamp(new DateTime(1970, 1, 1, 8, 0, 0).AddSeconds(expTimeLong));
                var nowTime = DateTimeHelper.GetUnixTimeStamp(DateTime.Now);

                if (expTime - nowTime < 60 * 60 * 10)
                {

                }

                if (this._permission == null || this._permission.Length == 0) return;
                var claimJson = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "permission")?.Value;

                //TODO Check Permission
                bool roleFlag = false;
                if (!roleFlag)
                {
                    //context.Result = new ObjectResult();
                }
            }
            else
            {

                context.Result = new ObjectResult(new TData<string>("授权失败", string.Empty, Status.NoPermission));
            }
        }
    }
}
