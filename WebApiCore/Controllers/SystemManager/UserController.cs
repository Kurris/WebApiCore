using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebApiCore.CustomClass;
using WebApiCore.Entity.SystemManager;
using WebApiCore.Interface.SystemManager;
using WebApiCore.Utils.Model;

namespace WebApiCore.Controllers.SystemManager
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

        [HttpPost]
        public async Task<TData<string>> LoginOff(string userName)
        {
            return await UserService.LoginOff(userName);
        }

        [HttpPost]
        public async Task<TData<string>> SignUp([FromBody] User user)
        {
            return await UserService.SignUp(user);
        }

        [HttpPost]
        public async Task<TData<User>> EditUser([FromBody] User user)
        {
            return await UserService.EditUser(user);
        }
    }
}
