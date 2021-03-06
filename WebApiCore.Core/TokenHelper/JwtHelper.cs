﻿using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IdentityModel;
using Microsoft.IdentityModel.Tokens;
using WebApiCore.Data.Entity;
using WebApiCore.Lib.Utils.Config;
using WebApiCore.Lib.Utils.Extensions;

namespace WebApiCore.Core.TokenHelper
{
    /// <summary>
    /// Jwt帮助类
    /// </summary>
    public class JwtHelper
    {
        /// <summary>
        /// 生成Token凭证
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <param name="jwtSetting">Jwt配置信息</param>
        /// <returns>Token</returns>
        public static string GenerateToken(User user, JwtConfig jwtSetting)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.TokenSecretKey));

            DateTime dtNow = DateTime.Now;
            DateTime dtExpires = DateTime.Now.AddMinutes(jwtSetting.Expiration);

            var descriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtClaimTypes.Issuer,jwtSetting.Issuer),
                    new Claim(JwtClaimTypes.Audience,jwtSetting.Audience),
                    new Claim(JwtClaimTypes.Id, user.UserId.ParseToStr()),
                    new Claim(JwtClaimTypes.Name, user.UserName),
                    new Claim("permission", user.UserName),
                    new Claim(JwtClaimTypes.NotBefore, dtNow.ParseToStr()),
                    new Claim(JwtClaimTypes.Expiration,dtExpires.ParseToStr())
                }),
                IssuedAt = dtNow,//颁发时间
                NotBefore = dtNow,
                Expires = dtExpires,
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            };

            var jwtHandler = new JwtSecurityTokenHandler();
            var securityToken = jwtHandler.CreateToken(descriptor);
            return jwtHandler.WriteToken(securityToken);
        }
    }
}
