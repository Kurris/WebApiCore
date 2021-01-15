using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApiCore.Business.Abstractions;
using WebApiCore.CustomClass;
using WebApiCore.Data.Entity;
using WebApiCore.Lib.Model;
using System.Linq;
using System.Collections.Generic;

namespace WebApiCore.Controllers.SystemManage
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        public IUserService UserService { get; set; }


        [HttpGet]
        public async Task<TData<object>> GetUserWithPagination([FromQuery] int pageSize, [FromQuery] int pageIndex)
        {
            var page = new Pagination()
            {
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var users = await UserService.FindWithPagination(page);

            return new TData<object>
            {
                Status = users.Status,
                Message = users.Message,
                Data = new
                {
                    total = page.Total,
                    data = users.Data
                }
            };
        }

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
