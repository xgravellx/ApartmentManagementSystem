using System.Security.Claims;
using ApartmentManagementSystem.Core.DTOs.InvoiceDto;
using ApartmentManagementSystem.Core.Interfaces;
using ApartmentManagementSystem.Core.Services;
using ApartmentManagementSystem.Models.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagementSystem.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController(IInvoiceService invoiceService) : ControllerBase
    {
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

        [Authorize(Roles = "Admin, User")]
        [HttpGet("by-apartment-id")]
        public async Task<IActionResult> GetByApartmentId(InvoiceByApartmentIdRequestDto request)
        {
            var response = await invoiceService.GetInvoicesByApartmentId(request);
            if (response.AnyError)
            {
                return NotFound(response.Errors);
            }
            return Ok(response.Data);
        }

        [Authorize(Roles = "Admin, User")]
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

    }
}
