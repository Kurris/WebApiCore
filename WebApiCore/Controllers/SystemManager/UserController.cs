using Ligy.Project.WebApi.CustomClass;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebApiCore.Entity.SystemManager;
using WebApiCore.Interface;
using WebApiCore.Utils.Model;

namespace Ligy.Project.WebApi.Controllers.SystemManager
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
        public async Task<User> RefreshToken()
        {
            return await UserService.RefreshToken();
        }

        [HttpPost]
        public async Task<TData<string>> LoginOff(string userName)
        {
            return await UserService.LoginOff(userName);
        }

        [HttpPost]
        public async Task<string> SignUp([FromBody] User user)
        {
            return await UserService.SignUp(user);
        }
    }
}
