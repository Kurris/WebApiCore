using System;
using System.Threading.Tasks;
using WebApiCore.Entity.SystemManager;
using WebApiCore.Utils;

namespace WebApiCore.Core
{
    public class Operator
    {
        public static Operator Instance { get; } = new Operator();
        private static string _tokenKey = GlobalInvariant.SystemConfig.JwtSetting.TokenName;

        public async Task AddCurrent(User user = null)
        {
            string loginProvider = GlobalInvariant.SystemConfig.LoginProvider;
            string jwtToken = JwtHelper.GenerateToken(user);

            switch (loginProvider)
            {
                case "WebApi":
                    user.Token = jwtToken;
                    break;
                case "Cookie":
                    new CookieHelper().AddCookie(_tokenKey, jwtToken);
                    break;
                case "Session":
                    new SessionHelper().AddSession(_tokenKey, jwtToken);
                    break;
                default:

                    break;
            }
        }
    }
}