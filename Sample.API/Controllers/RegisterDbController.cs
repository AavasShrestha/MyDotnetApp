using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sample.Data.DTO;
using Sample.Service.Service.RegisterDbService;

namespace Sample.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterDbController : ControllerBase
    {
        private readonly IRegisterDbService _registerDbService;

        public RegisterDbController(IRegisterDbService registerDbService)
        {
            _registerDbService = registerDbService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<RegisterDbDto>> GetAll()
        {
            var databases = _registerDbService.GetAllDatabases();
            return Ok(databases); 
        }

      
        [HttpGet("{id:int}")]
        public ActionResult<RegisterDbDto> GetById(int id)
        {
            var db = _registerDbService.GetDatabaseById(id);
            if (db == null) return NotFound(new { Message = "Database not found" });
            return Ok(db);
        }

      
        [HttpPost]
        public ActionResult<ValidationDTO> Create([FromBody] RegisterDbDto registerDbDto, [FromQuery] int userId)
        {
            if (registerDbDto == null)
                return BadRequest(new ValidationDTO { IsSuccess = false, Message = "Input cannot be null" });

            var result = _registerDbService.CreateDatabase(userId, registerDbDto);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
