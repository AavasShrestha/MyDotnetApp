using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sample.Data.DTO;
using Sample.Service.Service.Client;

namespace Sample.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClientDetailController : ControllerBase
    {
        private readonly IClientDetailService _service;

        public ClientDetailController(IClientDetailService service)
        {
            _service = service;
        }




        [HttpGet]
        public IActionResult GetAllClients()
        {
            try
            {
                var clients = _service.GetAllClients();
                return Ok(clients);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetClientById(int id)
        {
            try
            {
                var client = _service.GetClientById(id);

                if (client == null)
                    return NotFound($"Client with ID {id} not found.");

                return Ok(client);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPost]
        public IActionResult CreateClient([FromHeader(Name = "User-ID")] int userID ,[FromForm] AddClientDTO clientDetailDto)
        {
            try
            {
                var result = _service.CreateClient(userID, clientDetailDto);
                return Ok(result); // Returns 200 OK with created client
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // Input validation error
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateClient([FromHeader(Name = "User-ID")] int userID, int id, [FromForm] ClientDetailDto clientDetailDto)
        {
            if (clientDetailDto == null)
                return BadRequest("Client data cannot be null.");

            try
            {
                var result = _service.UpdateClient(userID, id, clientDetailDto);

                if (result == null)
                    return NotFound($"Client with ID {id} not found.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPatch("{id}")]
        public IActionResult PatchClient([FromHeader(Name = "User-ID")] int userID, int id, [FromBody] Dictionary<string, object> updates)
        {
            try
            {
                var updatedClient = _service.PatchClient(userID, id, updates);
                return Ok(updatedClient);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteClient(int id)
        {
            try
            {
                var result = _service.DeleteClient(id);

                if (!result.IsSuccess)
                    return NotFound($"Client with ID {id} not found.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }



    }
}
