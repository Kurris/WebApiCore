using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebApiCore.Business.Abstractions;
using WebApiCore.CustomClass;
using WebApiCore.Data.Entity.SystemManage;
using WebApiCore.Lib.Utils.Model;

namespace WebApiCore.Controllers.SystemManage
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public IUserService UserService { get; set; }
        public ILogger<UserController> Logger { get; set; }

        [HttpPost]
        public async Task<TData<User>> Login([FromBody] User user)
        {
            return await UserService.Login(user?.UserName, user?.Password);
        }

        [ApiAuth]
        [HttpGet]
        public async Task<string> RefreshToken()
        {
            return await UserService.RefreshToken();
        }

        [ApiAuth]
        [HttpGet]
        public async Task<TData<string>> LoginOff()
        {
            return await UserService.LoginOff();
        }

        [HttpPost]
        public async Task<TData<string>> SignUp([FromBody] User user)
        {
            return await UserService.SignUp(user);
        }

        [ApiAuth]
        [HttpPost]
        public async Task<TData<User>> EditUser([FromBody] User user)
        {
            return await UserService.EditUser(user);
        }
    }
}
