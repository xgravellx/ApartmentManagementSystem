using ApartmentManagementSystem.Core.DTOs.InvoiceDto;
using ApartmentManagementSystem.Models.Shared;

namespace ApartmentManagementSystem.Core.Interfaces;

public interface IInvoiceService
{
    Task<ResponseDto<List<InvoiceResponseDto>>> GetInvoicesByApartmentId(InvoiceByApartmentIdRequestDto request);
}