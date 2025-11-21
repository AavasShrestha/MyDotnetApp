using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sample.Service.Service.BranchService;

namespace Sample.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BranchController : ControllerBase
    {
        private readonly IBranchService _branchService;

        public BranchController(IBranchService branchService)
        {
            _branchService = branchService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var branches = await _branchService.GetAllBranches();
            return Ok(branches);
        }
    }

}
