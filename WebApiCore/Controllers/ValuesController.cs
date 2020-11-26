using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiCore.Entity;
using Newtonsoft.Json;
using WebApiCore.Entity.Models;
using Microsoft.Extensions.Logging;

namespace Ligy.Project.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly MyDbContext context;
        private readonly ILogger<ValuesController> ilogger;

        public ValuesController(MyDbContext context,ILogger<ValuesController> ilogger)
        {
            this.context = context;
            this.ilogger = ilogger;
        }

        [HttpPost]
        public int Add(User user)
        {
            context.Users.Add(user);
            context.SaveChanges();
            return 1;
        }

        [HttpPost]
        public void Delete()
        {

        }

        [HttpGet]
        public string Get()
        {
            ilogger.LogInformation("77777777777777777");
            return JsonConvert.SerializeObject(context.Users);
        }
    }
}
