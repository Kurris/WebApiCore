using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiCore.Entity;
using Newtonsoft.Json;

namespace Ligy.Project.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly MyDbContext context;

        public ValuesController(MyDbContext context)
        {
            this.context = context;
        }

        [HttpPost]
        public void Add()
        {

        }

        [HttpPost]
        public void Delete()
        {

        }

        [HttpGet]
        public string Get()
        {
            return JsonConvert.SerializeObject(context.Users);
        }
    }
}
