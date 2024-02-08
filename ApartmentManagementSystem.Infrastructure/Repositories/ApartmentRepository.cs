﻿using ApartmentManagementSystem.Infrastructure.Data;
using ApartmentManagementSystem.Infrastructure.Interfaces;
using ApartmentManagementSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApartmentManagementSystem.Infrastructure.Repositories;

public class ApartmentRepository(AppDbContext context) : IApartmentRepository
{
    public async Task<IEnumerable<Apartment>> GetAllAsync()
    {
        return await context.Apartment.ToListAsync();
    }

    public async Task<Apartment> GetByIdAsync(int apartmentId)
    {
        return await context.Apartment.FindAsync(apartmentId);
    }

    public async Task AddAsync(Apartment apartment)
    {
        await context.Apartment.AddAsync(apartment);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Apartment apartment)
    {
        context.Apartment.Update(apartment);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int apartmentId)
    {
        var apartment = await context.Apartment.FindAsync(apartmentId);
        if (apartment != null)
        {
            context.Apartment.Remove(apartment);
            await context.SaveChangesAsync();
        }
    }

    public async Task<bool> IsExistAsync(int apartmentId)
    {
        return await context.Apartment.AnyAsync(x => x.ApartmentId == apartmentId);
    }

    public async Task<int> GetApartmentIdsByUserIdAsync(Guid userId)
    {
        return await context.Apartment
            .Where(a => a.UserId == userId)
            .Select(a => (int)a.ApartmentId)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Apartment>> GetActiveApartmentsByBlock(string block)
    {
        return await context.Apartment
            .Where(a => a.Block == block && a.Status == true)
            .ToListAsync();
    }

}
