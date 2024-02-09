using ApartmentManagementSystem.Core.DTOs.AuthDto;
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
        public async Task<IActionResult> AdminLogin(AuthAdminRequestDto request)
        {
            var response = await authService.AdminLogin(request);
            if (response.AnyError)
            {
                return BadRequest(response.Errors);
            }
            return Ok(response.Data);
        }

        [HttpPost("user-login")]
        public async Task<IActionResult> UserLogin(AuthUserRequestDto request)
        {
            var response = await authService.UserLogin(request);
            if (response.AnyError)
            {
                return BadRequest(response.Errors);
            }
            return Ok(response.Data);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var response = await authService.Logout();
            if (response.AnyError)
            {
                return BadRequest(response);
            }
            return NoContent();
        }
    }
}
