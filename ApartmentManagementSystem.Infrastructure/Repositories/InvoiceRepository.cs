using ApartmentManagementSystem.Infrastructure.Data;
using ApartmentManagementSystem.Infrastructure.Interfaces;
using ApartmentManagementSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApartmentManagementSystem.Infrastructure.Repositories;

public class InvoiceRepository(AppDbContext context) : IInvoiceRepository
{
    public async Task<IEnumerable<Invoice>> GetByApartmentIdAsync(int apartmentId)
    {
        return await context.Invoice
            .Where(x => x.ApartmentId == apartmentId)
            .ToListAsync();
    }
}