using ApartmentManagementSystem.Core.DTOs.UserDto;
using ApartmentManagementSystem.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagementSystem.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserService userService) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await userService.GetAll();

            if (response.AnyError)
            {
                return NotFound(response.Errors);
            }

            return Ok(response.Data);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await userService.GetById(id);

            if (response.AnyError)
            {
                return NotFound(response.Errors);
            }

            return Ok(response.Data);
        }


        [HttpPost]
        public async Task<IActionResult> CreateUser(UserCreateRequestDto request)
        {
            var response = await userService.CreateUser(request);

            if (response.AnyError)
            {
                return BadRequest(response.Errors);
            }

            return Created("", response.Data);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UserUpdateRequestDto request)
        {
            var response = await userService.UpdateUser(request);

            if (response.AnyError)
            {
                return BadRequest(response.Errors);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var response = await userService.DeleteUser(id);

            if (response.AnyError)
            {
                return BadRequest(response.Errors);
            }

            return NoContent();
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRoleToUser(UserAssignToRoleRequestDto request)
        {
            var response = await userService.AssignRoleToUser(request);

            if (response.AnyError)
            {
                return BadRequest(response.Errors);
            }

            return NoContent();
        }
    }
}
