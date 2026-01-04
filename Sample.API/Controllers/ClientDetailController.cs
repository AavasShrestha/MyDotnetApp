using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sample.Data.DTO;
using Sample.Data.RoutingDB;
using Sample.Repository;
using Sample.Service.Service.Client;

namespace Sample.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ClientDetailController : ControllerBase
    {
        private readonly IClientDetailService _service;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _env;
        private readonly IUnitOfWork _unitOfWork;

        public ClientDetailController(IClientDetailService service, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env, IUnitOfWork unitOfWork)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
            _env = env;
            _unitOfWork = unitOfWork;
        }



        //[HttpGet("view-logo/{id}")]
        //public IActionResult ViewLogo(int id)
        //{
        //    var client = _service.GetClientById(id);
        //    if (client == null || string.IsNullOrEmpty(client.Logo))
        //        return NotFound();

        //    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", client.Logo.Replace("/", "\\"));
        //    if (!System.IO.File.Exists(filePath))
        //        return NotFound();

        //    var mimeType = "image/" + Path.GetExtension(filePath).TrimStart('.');
        //    return PhysicalFile(filePath, mimeType);
        //}




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
        public IActionResult CreateClient([FromHeader(Name = "User-ID")] int userID, [FromForm] AddClientDTO clientDetailDto)
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
            var result = _service.DeleteClient(id);
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




        [HttpGet("image/{id}/{fileName}")]
        public IActionResult GetImage(int id, string fileName)
        {
            // Dynamically get path from wwwroot/ uploads / logos
            var basePath = Path.Combine(_env.WebRootPath, "uploads", "logos");

            var tenantDB = _service.GetClientById(id).db_name;


            // Build the actual file path
            string actualFileName = fileName; // "8973p.jpg"
            string folder = tenantDB;       // or dynamic if needed
            string filePath = Path.Combine(
               basePath,
                folder,
                actualFileName
            );

            if (!System.IO.File.Exists(filePath))
                return NotFound("Image not found.");

            string ext = Path.GetExtension(filePath).ToLowerInvariant();
            string contentType = ext switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                _ => "application/octet-stream"
            };

            byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);
            return File(imageBytes, contentType);
        }
    }
}
