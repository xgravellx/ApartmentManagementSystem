using System.Security.Claims;
using ApartmentManagementSystem.Core.DTOs.InvoiceDto;
using ApartmentManagementSystem.Core.Interfaces;
using ApartmentManagementSystem.Core.Services;
using ApartmentManagementSystem.Models.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApartmentManagementSystem.Models.Shared;
using ApartmentManagementSystem.Models.Entities;

namespace ApartmentManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController(IInvoiceService invoiceService) : ControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await invoiceService.GetAll();
            if (response.AnyError)
            {
                return NotFound(response.Errors);
            }
            return Ok(response.Data);
        }

        [Authorize(Roles = "User, Admin")]
        [HttpGet("by-apartment-id")]
        public async Task<IActionResult> GetByApartmentId(int ApartmentId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");
            var response = await invoiceService.GetInvoicesByApartmentId(ApartmentId, userId, isAdmin);
            if (response.AnyError)
            {
                return NotFound(response.Errors);
            }
            return Ok(response.Data);
        }

        [Authorize(Roles = "User, Admin")]
        [HttpGet("filter")]
        public async Task<IActionResult> GetFiltered(InvoiceFilterRequestDto request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");
            var response = await invoiceService.GetFiltered(request, userId, isAdmin);
            if (response.AnyError)
            {
                return NotFound(response.Errors);
            }
            return Ok(response.Data);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddInvoice(InvoiceCreateGeneralRequestDto request)
        {
            var response = await invoiceService.CreateGeneralInvoice(request);
            if (response.AnyError)
            {
                return BadRequest(response.Errors);
            }
            return Created("", response.Data);
        }

    }
}
