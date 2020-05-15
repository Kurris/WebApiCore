using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ligy.Project.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FirstController : ControllerBase
    {
        [Route("Post")]
        [HttpPost]
        public string Post(User user)
        {
            return "success";
        }

        [Route("Get")]
        [HttpGet]
        public IActionResult Get(int iPara)
        {
            if( iPara==0 )
            {
                throw new Exception("yichang");
            }
            return Ok(iPara);
        }

        [Route("GetInFo")]
        [HttpGet]
        public IActionResult GetInFo()
        {
            return Ok(22);
        }
    }
    public class User
    {
        [Required(ErrorMessage = "用户Code不能为空")]
        public string Code { get; set; }

        [Required(ErrorMessage = "用户名称不能为空")]
        public string Name { get; set; }

        [Required(ErrorMessage = "用户年龄不能为空")]
        [Range(1 , 100 , ErrorMessage = "年龄必须介于1~100之间")]
        public int Age { get; set; }

        public string Address { get; set; }
    }

}