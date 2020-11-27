using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApiCore.Entity;
using WebApiCore.Entity.Models;
using WebApiCore.IOC.Interface;

namespace Ligy.Project.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EFTestsController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly IClock _clock;
        private readonly ILogger<EFTestsController> _logger;

        public EFTestsController(MyDbContext context, IClock clock, ILogger<EFTestsController> logger)
        {
            this._context = context;
            this._logger = logger;
            _clock = clock;
        }

        [HttpPost]
        public User GetUserInfo(User user)
        {
            string type = _clock.GetType().ToString();
            _logger.LogInformation(_logger.GetType().ToString());
            return _context.Users.Where(x => x.Name == user.Name).FirstOrDefault();
        }
    }
}
