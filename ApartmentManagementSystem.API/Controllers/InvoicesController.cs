using ApartmentManagementSystem.Core.DTOs.InvoiceDto;
using ApartmentManagementSystem.Core.Interfaces;
using ApartmentManagementSystem.Core.Services;
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
    }
}
