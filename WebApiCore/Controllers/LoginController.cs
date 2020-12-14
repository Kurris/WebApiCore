using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiCore.Entity.SystemManager;

namespace Ligy.Project.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        public string Login([FromBody] User user)
        {
            if (user.Name.Equals("ligy") && user.Password.Equals("123"))
            {
                Response.Cookies.Append("access_token", JwtHelper.GenerateToken(user));
                return "OK";
            }
            else
            {
                return "false";
            }
        }
    }
}
