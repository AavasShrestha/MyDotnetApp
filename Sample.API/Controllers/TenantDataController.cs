using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sample.Service.Service.Tenant;

namespace Sample.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantDataController : ControllerBase
    {
        private readonly TenantService _tenantService;

        public TenantDataController(TenantService tenantService)
        {
            _tenantService = tenantService;
        }

        [HttpGet("{tenantKey}/{tableName}")]
        public async Task<IActionResult> GetTenantTable(string tenantKey, string tableName)
        {
            var data = await _tenantService.QueryTenantTableAsync(tenantKey, tableName);
            return Ok(data);
        }
    }
}
