using ApartmentManagementSystem.Infrastructure.Data;
using ApartmentManagementSystem.Infrastructure.Interfaces;
using ApartmentManagementSystem.Models.Entities;
using ApartmentManagementSystem.Models.Shared;
using Microsoft.EntityFrameworkCore;

namespace ApartmentManagementSystem.Infrastructure.Repositories;

public class PaymentRepository(AppDbContext context) : IPaymentRepository
{
    public async Task<List<Payment>> GetAllAsync()
    {
        return await context.Payment.ToListAsync();
    }

    public async Task<List<Payment>> GetByApartmentIdAsync(int apartmentId)
    {
        return await context.Payment
            .Include(p => p.Invoice)
            . Where(p => p.Invoice.ApartmentId == apartmentId)
            .ToListAsync();
    }

    public async Task<List<Guid>> GetUserIdsWithRegularPayments(int month, int year)
    {
        var lastDayOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));

        return await context.Payment
            .Include(p => p.Invoice)
            .Where(p => p.Invoice.Month == month &&
                        p.Invoice.Year == year &&
                        p.Date <= lastDayOfMonth)
            .GroupBy(p => p.UserId)
            .Select(g => g.Key)
            .ToListAsync();
    }


}