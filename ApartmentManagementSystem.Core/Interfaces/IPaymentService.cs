using ApartmentManagementSystem.Core.DTOs.PaymentDto;
using ApartmentManagementSystem.Models.Entities;
using ApartmentManagementSystem.Models.Shared;

namespace ApartmentManagementSystem.Core.Interfaces;

public interface IPaymentService
{
    Task<ResponseDto<List<PaymentGetAllResponseDto>>> GetAllPayments();
    Task<ResponseDto<List<PaymentGetByApartmentIdResponseDto>>> GetPaymentsByApartmentId(int apartmentId);
    Task<ResponseDto<List<User>>> GetRegularPaymentUsers(PaymentRegularRequestDto request);
}