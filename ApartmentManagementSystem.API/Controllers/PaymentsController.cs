using ApartmentManagementSystem.Core.DTOs.PaymentDto;
using ApartmentManagementSystem.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController(IPaymentService paymentService) : ControllerBase
    {
        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        public async Task<IActionResult> Create(PaymentCreateRequestDto request)
        {
            var isAdmin = User.IsInRole("Admin");
            var response = await paymentService.CreatePayment(request, isAdmin);
            if (response.AnyError)
            {
                return BadRequest(response.Errors);
            }
            return Ok(response.Data);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPut]
        public async Task<IActionResult> Update(PaymentUpdateRequestDto request)
        {
            var response = await paymentService.UpdatePayment(request);
            if (response.AnyError)
            {
                return BadRequest(response.Errors);
            }
            return Ok(response.Data);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int paymentId)
        {
            var response = await paymentService.DeletePayment(paymentId);
            if (response.AnyError)
            {
                return BadRequest(response.Errors);
            }
            return Ok(response.Data);
        }
    }
}
