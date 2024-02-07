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
            .Where(x => x.ApartmentId == apartmentId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Invoice>> GetFilteredAsync(InvoiceFilterRequestDto request)
    {
        IQueryable<Invoice> query = context.Invoice.Include(i => i.Apartment).AsQueryable();

        // Apartman ID'leri kontrolü
        if (request.ApartmentIds != null && request.ApartmentIds.Any())
        {
            query = query.Where(invoice => request.ApartmentIds.Contains(invoice.ApartmentId));
        }

        // Aylar kontrolü
        if (request.Months != null && request.Months.Any())
        {
            query = query.Where(invoice => request.Months.Contains(invoice.Month));
        }

        // Yıllar kontrolü
        if (request.Years != null && request.Years.Any())
        {
            query = query.Where(invoice => request.Years.Contains(invoice.Year));
        }

        // Kullanıcı ID'leri kontrolü
        if (request.UserIds != null && request.UserIds.Any())
        {
            query = query.Where(invoice => invoice.Apartment.UserId.HasValue && request.UserIds.Contains(invoice.Apartment.UserId.Value));
        }

        // Ödeme durumu kontrolü
        if (request.PaymentStatus.HasValue)
        {
            query = query.Where(invoice => invoice.PaymentStatus == request.PaymentStatus.Value);
        }

        return await query.ToListAsync();
    }




}