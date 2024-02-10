using ApartmentManagementSystem.Models.Entities;
using ApartmentManagementSystem.Models.Shared;

namespace ApartmentManagementSystem.Infrastructure.Interfaces;

public interface IInvoiceRepository
{
    Task<IEnumerable<Invoice>> GetAllAsync();

    Task<IEnumerable<Invoice>> GetByApartmentIdAsync(int apartmentId);

    Task<IEnumerable<Invoice>> GetFilteredAsync(InvoiceFilterRequestDto request);
    Task<Invoice> AddInvoiceAsync(Invoice invoice);
    Task<Invoice> GetByIdAsync(int invoiceId);
    Task<Invoice?> GetByUserIdAsync(Guid userId);
    Task<bool> UpdateInvoiceAsync(Invoice invoice);
    Task<bool> DeleteInvoiceAsync(int invoiceId);
    Task<IEnumerable<Invoice>> GetUnpaidAndOverdueInvoicesAsync(DateTime today);
    Task<bool> IsPaidDuesForMonthAsync(int apartmentId, int year, int month);
    Task<decimal> GetTotalAmountByApartmentIdAsync(int apartmentId);
    Task<decimal?> GetAmountByInvoiceIdAsync(int invoiceId);
    Task<decimal> GetTotalDebtAsync(int aparmentId, int? year, int? month);
}