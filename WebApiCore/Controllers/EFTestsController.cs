using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiCore.Entity;
using WebApiCore.Entity.Models;

namespace Ligy.Project.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EFTestsController : ControllerBase
    {
        private readonly MyDbContext context;

        public EFTestsController(MyDbContext context)
        {
            this.context = context;
        }

        [HttpPost]
        public User GetUserInfo(User user)
        {
            return context.Users.Where(x => x.Name == user.Name).FirstOrDefault();
        }
    }
}
