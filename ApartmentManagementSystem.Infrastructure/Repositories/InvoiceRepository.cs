using ApartmentManagementSystem.Infrastructure.Data;
using ApartmentManagementSystem.Infrastructure.Interfaces;
using ApartmentManagementSystem.Models.Entities;
using ApartmentManagementSystem.Models.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ApartmentManagementSystem.Models.Enums;

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

    public async Task<IEnumerable<Invoice>> GetInvoicesByIdsAsync(IEnumerable<int> invoiceIds)
    {
        return await context.Invoice
            .Where(invoice => invoiceIds.Contains(invoice.InvoiceId))
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

        if (request.Months != null && request.Months.Any())
        {
            query = query
                .Where(i => request.Months
                .Contains(i.Month));
        }

        if (request.Years != null && request.Years.Any())
        {
            query = query
                .Where(i => request.Years
                .Contains(i.Year));
        }

        if (request.UserIds != null && request.UserIds.Any())
        {
            query = query
                .Where(i => i.Apartment.UserId.HasValue && request.UserIds
                .Contains(i.Apartment.UserId.Value));
        }

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

    public async Task<Invoice?> GetByUserIdAsync(Guid userId)
    {
        return await context.Invoice
            .Include(i => i.Apartment)
            .FirstOrDefaultAsync(i => i.Apartment.UserId == userId);
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

    public async Task<IEnumerable<Invoice>> GetUnpaidAndOverdueInvoicesAsync(DateTime today)
    {
        return await context.Invoice
            .Where(invoice => !invoice.PaymentStatus && invoice.DueDate < today)
            .ToListAsync();
    }

    public async Task<bool> IsPaidDuesForMonthAsync(int apartmentId, int year, int month)
    {
            return await context.Invoice
                .AnyAsync(i => i.ApartmentId == apartmentId && 
                               i.Year == year && 
                               i.Month == month &&
                               i.PaymentStatus && 
                               i.Type == InvoiceType.Dues);
    }

    public async Task<decimal> GetTotalAmountByApartmentIdAsync(int apartmentId)
    {
        return await context.Invoice
            .Where(i => i.ApartmentId == apartmentId)
            .SumAsync(i => i.Amount);
    }

    public async Task<decimal?> GetAmountByInvoiceIdAsync(int invoiceId)
    {
        return await context.Invoice
            .Where(i => i.InvoiceId == invoiceId)
            .Select(i => i.Amount)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> GetPaidAmountAsync(int incoiceId)
    {
        return await context.Invoice
            .Where(i => i.InvoiceId == incoiceId)
            .Select(i => i.PaymentStatus)
            .FirstOrDefaultAsync();
    }

    public async Task<decimal> GetTotalDebtAsync(int apartmentId, int? year, int? month)
    {
        var query = context.Invoice.AsQueryable();

        query = query.Where(i => i.ApartmentId == apartmentId);

        if (year.HasValue)
        {
            query = query.Where(i => i.Year == year.Value);
        }

        if (month.HasValue)
        {
            query = query.Where(i => i.Month == month.Value);
        }

        return await query.SumAsync(i => i.PayableAmount);
    }

    
}