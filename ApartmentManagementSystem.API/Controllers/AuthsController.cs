using ApartmentManagementSystem.Core.DTOs;
using ApartmentManagementSystem.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthsController(IAuthService authService) : ControllerBase
    {
        [HttpPost("admin-login")]
        public async Task<IActionResult> AdminLogin(AdminLoginRequestDto request)
        {
            var response = await authService.AdminLoginAsync(request);
            if (response.AnyError)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("user-login")]
        public async Task<IActionResult> UserLogin(UserLoginRequestDto request)
        {
            var response = await authService.UserLoginAsync(request);
            if (response.AnyError)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var response = await authService.LogoutAsync();
            if (response.AnyError)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
