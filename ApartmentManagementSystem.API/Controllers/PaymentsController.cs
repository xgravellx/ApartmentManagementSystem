using ApartmentManagementSystem.Core.DTOs.PaymentDto;
using ApartmentManagementSystem.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagementSystem.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController(IPaymentService paymentService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await paymentService.GetAllPayments();
            if (response.AnyError)
            {
                return NotFound(response.Errors);
            }
            return Ok(response.Data);
        }

        [HttpGet("by-apartment-id")]
        public async Task<IActionResult> GetByApartmentId(int apartmentId)
        {
            var response = await paymentService.GetPaymentsByApartmentId(apartmentId);
            if (response.AnyError)
            {
                return NotFound(response.Errors);
            }
            return Ok(response.Data);
        }

        [HttpGet("regular")]
        public async Task<IActionResult> GetRegularPaymentUsers(PaymentRegularRequestDto request)
        {
            var response = await paymentService.GetRegularPaymentUsers(request);
            if (response.AnyError)
            {
                return NotFound(response.Errors);
            }
            return Ok(response.Data);
        }
    }
}
