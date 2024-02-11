using ApartmentManagementSystem.Core.DTOs.InvoiceDto;
using ApartmentManagementSystem.Models.Shared;

namespace ApartmentManagementSystem.Core.Interfaces;

public interface IInvoiceService
{
    Task<ResponseDto<List<InvoiceResponseDto>>> GetAll();
    Task<ResponseDto<List<InvoiceResponseDto>>> GetInvoicesByApartmentId(int ApartmentId, string identityNumber, bool isAdmin);
    Task<ResponseDto<InvoiceFilterResponseDto>> GetFiltered(InvoiceFilterRequestDto request, string identityNumber, bool isAdmin);
    Task<ResponseDto<decimal>> GetDebtFilter(InvoiceDebtFilterRequestDto request);
    Task<ResponseDto<bool>> CreateGeneralInvoice(InvoiceCreateGeneralRequestDto request);
    Task<ResponseDto<bool>> CreateDuesInvoice(InvoiceCreateDuesRequestDto request);
    Task<ResponseDto<bool>> UpdateInvoice(InvoiceUpdateRequestDto request);
    Task<ResponseDto<bool?>> DeleteInvoice(int invoiceId);
}