using Azure;
using Sample.Data.DTO;
using Sample.Data.RoutingDB;
using Sample.Service;
using Sample.Service.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Sample.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly AuthService _authService;

        public LoginController(ILoginService loginService, AuthService authService)
        {
            _loginService = loginService;
            _authService = authService;
        }

        [HttpPost("userauthentication")]
        public async Task<IActionResult> UserLogin([FromBody] LoginDTO login)
        {
            try
            {
                var response = _loginService.GetLoggedInUserDetail(login.UserName, login.Password);

                if (response.isLoginSuccess == true)
                {
                    var token = _authService.GenerateJwtToken(response.userDetail.Id);
                    return new OkObjectResult(new
                    {
                        UserDetail = response.userDetail,
                        Token = token
                    });
                }
                else
                {
                    return new BadRequestObjectResult(new
                    {
                        ErrorMessage = "User Login Failed"
                    });
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new
                {
                    ErrorMessage = ex.Message
                });
            }
        }
    }
}