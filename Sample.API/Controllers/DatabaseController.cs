using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sample.Service.Database;

namespace Sample.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatabaseController : ControllerBase
    {
        private readonly IDatabaseService _databaseService;

        public DatabaseController(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        // GET: api/database
        [HttpGet]
        public IActionResult GetDatabases()
        {
            var result = _databaseService.GetAllDatabases();
            return Ok(result);
        }
    }
}
