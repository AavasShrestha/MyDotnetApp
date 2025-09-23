using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sample.Data.DTO;
using Sample.Data.RoutingDB;
using Sample.Service.Service.LogoService;

namespace Sample.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogoController : ControllerBase
    {
        private readonly ILogoService _logoService;

        public LogoController(ILogoService logoService)
        {
            _logoService = logoService;
        }

     
        [HttpPost("upload")]
        public IActionResult Upload([FromForm] LogoDto dto)
        {
            var result = _logoService.UploadLogo(dto.File);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

       
        [HttpPut("update/{id}")]
        public IActionResult Update(int id, [FromForm] LogoDto dto)
        {
            var result = _logoService.UpdateLogo(id, dto.File);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

      
        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var result = _logoService.DeleteLogo(id);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

      
        [HttpGet]
        public ActionResult<IEnumerable<Logo>> GetAll()
        {
            var logos = _logoService.GetAllLogos();
            return Ok(logos);
        }

       
        [HttpGet("{id}")]
        public ActionResult<Logo> GetById(int id)
        {
            var logo = _logoService.GetLogoById(id);
            if (logo == null)
                return NotFound("Logo not found");

            return Ok(logo);
        }
    }
}
