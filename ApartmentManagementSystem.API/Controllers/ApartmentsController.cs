using ApartmentManagementSystem.Core.DTOs.ApartmentDto;
using ApartmentManagementSystem.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagementSystem.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ApartmentsController(IApartmentService apartmentService) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await apartmentService.GetAll();
            if (response.AnyError)
            {
                return NotFound(response.Errors);
            }
            return Ok(response.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await apartmentService.GetById(id);
            if (response.AnyError)
            {
                return NotFound(response.Errors);
            }
            return Ok(response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateApartment(ApartmentCreateRequestDto request)
        {
            var response = await apartmentService.CreateApartment(request);
            if (response.AnyError)
            {
                return BadRequest(response.Errors);
            }
            return Created("", response.Data);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateApartment(ApartmentUpdateRequestDto request)
        {
            var response = await apartmentService.UpdateApartment(request);
            if (response.AnyError)
            {
                return BadRequest(response.Errors);
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApartment(int id)
        {
            var response = await apartmentService.DeleteApartment(id);
            if (response.AnyError)
            {
                return BadRequest(response.Errors);
            }
            return NoContent();
        }

        [HttpPost("assign-user")]
        public async Task<IActionResult> AssignUserToApartment(ApartmentAssignUserToRequestDto request)
        {
            var response = await apartmentService.AssignUserToApartment(request);
            if (response.AnyError)
            {
                return BadRequest(response.Errors);
            }
            return NoContent();
        }
    }
}
