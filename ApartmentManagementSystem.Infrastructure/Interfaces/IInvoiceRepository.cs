using ApartmentManagementSystem.Models.Entities;

namespace ApartmentManagementSystem.Infrastructure.Interfaces;

public interface IInvoiceRepository
{
    Task<IEnumerable<Invoice>> GetByApartmentIdAsync(int apartmentId);
}