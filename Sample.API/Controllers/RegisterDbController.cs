using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sample.Data.DTO;
using Sample.Service.Service.RegisterDbService;

namespace Sample.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
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
        public ActionResult<ValidationDTO> Create([FromForm] RegisterDbDto registerDbDto, [FromQuery] int userId)
        {
            if (registerDbDto == null)
                return BadRequest(new ValidationDTO { IsSuccess = false, Message = "Input cannot be null" });

            var result = _registerDbService.CreateDatabase(userId, registerDbDto);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateDatabase(int userId, int id, RegisterDbDto registerDbDto)
        {
            if(registerDbDto == null)
            {
                return BadRequest("Db not found");
            }
            try
            {
                var result = _registerDbService.UpdateDatabase(userId, id, registerDbDto);
                if(result == null)
                {
                    return NotFound($"Db with ID {id} not found.");

                }
                return Ok(result);

            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
        //public ActionResult<ValidationDTO> DeleteDatabase(int id)
        //{
        //    if (id <= 0)
        //        return BadRequest(new ValidationDTO 
        //        {
        //            IsSuccess = false, 
        //            Message = "Invalid database Id." });

        //    var result = _registerDbService.DeleteDatabase(id);

        //    if (!result.IsSuccess)
        //        return BadRequest(result);

        //    return Ok(result);
        //}

        [HttpPatch("{id}")]
        public IActionResult PatchDatabase(int id, [FromBody] Dictionary<string, object> patchData)
        {
            if (patchData == null || patchData.Count == 0)
            {
                return BadRequest("No fields provided for patching.");
            }

            try
            {
                var updatedDb = _registerDbService.PatchDatabase(1, id, patchData); // Example: userID = 1

                if (updatedDb == null)
                {
                    return NotFound($"Database with ID {id} not found.");
                }

                return Ok(updatedDb);
            }
            catch (ArgumentException ex)
            {
                // invalid or random database name
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteClient(int id)
        {
            var result = _registerDbService.DeleteDatabase(id);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                if (result.Message.Contains("Not found"))
                    return NotFound(result);

                return BadRequest(result);
            }
        }
    }
}
