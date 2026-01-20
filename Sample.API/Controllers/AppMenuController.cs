//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Sample.Data.KamanaDB.Entities;
//using Sample.Service.Kamana;

//namespace Sample.API.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AppMenuController : ControllerBase
//    {
//        private readonly IAppMenuService _appMenuService;

//        public AppMenuController(IAppMenuService appMenuService)
//        {
//            _appMenuService = appMenuService;
//        }

//        [HttpGet("GetAll")]
//        public async Task<ActionResult<IEnumerable<TblAppMenu>>> GetAll()
//        {
//            var menus = await _appMenuService.GetAllAsync();
//            return Ok(menus);
//        }

//    }
//}
