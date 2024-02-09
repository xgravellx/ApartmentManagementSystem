using ApartmentManagementSystem.Core.DTOs.PaymentDto;
using ApartmentManagementSystem.Models.Entities;
using ApartmentManagementSystem.Models.Shared;

namespace ApartmentManagementSystem.Core.Interfaces;

public interface IPaymentService
{
    Task<ResponseDto<List<PaymentGetAllResponseDto>>> GetAllPayments();
    Task<ResponseDto<List<PaymentGetByApartmentIdResponseDto>>> GetPaymentsByApartmentId(int apartmentId);
    Task<ResponseDto<List<User>>> GetRegularPaymentUsers(PaymentRegularRequestDto request);
    Task<ResponseDto<int>> CreatePayment(PaymentCreateRequestDto request, bool isAdmin);
    Task<ResponseDto<bool>> UpdatePayment(PaymentUpdateRequestDto request);
    Task<ResponseDto<bool>> DeletePayment(int paymentId);
}