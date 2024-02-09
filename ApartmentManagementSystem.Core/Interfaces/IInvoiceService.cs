using ApartmentManagementSystem.Core.DTOs.InvoiceDto;
using ApartmentManagementSystem.Models.Shared;

namespace ApartmentManagementSystem.Core.Interfaces;

public interface IInvoiceService
{
    Task<ResponseDto<List<InvoiceResponseDto>>> GetAll();
    Task<ResponseDto<List<InvoiceResponseDto>>> GetInvoicesByApartmentId(int ApartmentId, string userId, bool isAdmin);
    Task<ResponseDto<List<InvoiceResponseDto>>> GetFiltered(InvoiceFilterRequestDto request, string userId, bool isAdmin);
    Task<ResponseDto<bool?>> CreateGeneralInvoice(InvoiceCreateGeneralRequestDto request);
    Task<ResponseDto<bool?>> CreateDuesInvoice(InvoiceCreateDuesRequestDto request);
    Task<ResponseDto<bool?>> UpdateInvoice(InvoiceUpdateRequestDto request);
    Task<ResponseDto<bool?>> DeleteInvoice(int invoiceId);
    Task CheckAndApplyOverDue();
}