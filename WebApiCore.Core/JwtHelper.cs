using IdentityModel;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiCore.Entity.SystemManager;
using WebApiCore.Utils;
using WebApiCore.Utils.Extensions;

namespace WebApiCore.Core
{
    /// <summary>
    /// Jwt帮助类
    /// </summary>
    public class JwtHelper
    {
        /// <summary>
        /// 生成Token凭证
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GlobalInvariant.SystemConfig.JwtSetting.TokenKey));

            DateTime dtNow = DateTime.Now;
            DateTime dtExpiresAt = DateTime.Now.AddMinutes(60);

            var descriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtClaimTypes.Issuer,GlobalInvariant.SystemConfig.JwtSetting.Issuer),
                    new Claim(JwtClaimTypes.Audience,GlobalInvariant.SystemConfig.JwtSetting.Audience),
                    new Claim(JwtClaimTypes.Id, user.UserId.ParseToString()),
                    new Claim(JwtClaimTypes.Name, user.UserName),
                    new Claim("permission", user.UserName),
                    new Claim(JwtClaimTypes.NotBefore, dtNow.ParseToString()),
                    new Claim(JwtClaimTypes.Expiration,dtExpiresAt.ParseToString())
                }),
                IssuedAt = dtNow,//颁发时间
                NotBefore = dtNow,
                Expires = dtExpiresAt,
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            };

            var jwtHandler = new JwtSecurityTokenHandler();
            var securityToken = jwtHandler.CreateToken(descriptor);
            return jwtHandler.WriteToken(securityToken);
        }
    }
}
