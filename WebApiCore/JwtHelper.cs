using IdentityModel;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApiCore.Entity.SystemManager;
using WebApiCore.Utils;
using WebApiCore.Utils.Extensions;

namespace Ligy.Project.WebApi
{
    public class JwtHelper
    {
        public static string GenerateToken(User user)
        {
            string secretKey = "12345678987654321";
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            DateTime dtNow = DateTime.Now;
            DateTime dtExpiresAt = DateTime.Now.AddMinutes(1);

            var descriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtClaimTypes.Audience,"api"),
                    new Claim(JwtClaimTypes.Issuer,"ligy.site"),
                    new Claim(JwtClaimTypes.Id, user.UserId.ParseToString()),
                    new Claim(JwtClaimTypes.Name, user.Name),
                    new Claim( JwtClaimTypes.Role, user.Name),
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
