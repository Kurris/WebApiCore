using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ligy.Project.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EFTestsController : ControllerBase
    {
        public DbContext MyContext { get; set; }

        public EFTestsController()
        {

        }

        [HttpGet]
        public void GetAAAA()
        {

        }
    }
}
