using ApartmentManagementSystem.Infrastructure.Data;
using ApartmentManagementSystem.Infrastructure.Interfaces;
using ApartmentManagementSystem.Models.Entities;
using ApartmentManagementSystem.Models.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ApartmentManagementSystem.Infrastructure.Repositories;

public class InvoiceRepository(AppDbContext context) : IInvoiceRepository
{
    public async Task<IEnumerable<Invoice>> GetAllAsync()
    {
        return await context.Invoice.ToListAsync();
    }

    public async Task<IEnumerable<Invoice>> GetByApartmentIdAsync(int apartmentId)
    {
        return await context.Invoice
            .Where(i => i.ApartmentId == apartmentId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Invoice>> GetFilteredAsync(InvoiceFilterRequestDto request)
    {
        IQueryable<Invoice> query = context.Invoice.Include(i => i.Apartment).AsQueryable();

        if (request.ApartmentIds != null && request.ApartmentIds.Any())
        {
            query = query
                .Where(i => request.ApartmentIds
                .Contains(i.ApartmentId));
        }

        // Aylar kontrolü
        if (request.Months != null && request.Months.Any())
        {
            query = query
                .Where(i => request.Months
                .Contains(i.Month));
        }

        // Yıllar kontrolü
        if (request.Years != null && request.Years.Any())
        {
            query = query
                .Where(i => request.Years
                .Contains(i.Year));
        }

        // Kullanıcı ID'leri kontrolü
        if (request.UserIds != null && request.UserIds.Any())
        {
            query = query
                .Where(i => i.Apartment.UserId.HasValue && request.UserIds
                .Contains(i.Apartment.UserId.Value));
        }

        // Ödeme durumu kontrolü
        if (request.PaymentStatus.HasValue)
        {
            query = query
                .Where(i => i.PaymentStatus == request.PaymentStatus.Value);
        }

        return await query.ToListAsync();
    }

    public async Task<Invoice> AddInvoiceAsync(Invoice invoice)
    {
        context.Invoice.Add(invoice);
        await context.SaveChangesAsync();
        return invoice;
    }

    public async Task<Invoice> GetByIdAsync(int invoiceId)
    {
        return await context.Invoice.FindAsync(invoiceId);
    }

    public async Task<bool> UpdateInvoiceAsync(Invoice invoice)
    {
        context.Invoice.Update(invoice);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteInvoiceAsync(int invoiceId)
    {
        var invoice = await context.Invoice.FindAsync(invoiceId);
        context.Invoice.Remove(invoice!);
        return await context.SaveChangesAsync() > 0;
    }


}