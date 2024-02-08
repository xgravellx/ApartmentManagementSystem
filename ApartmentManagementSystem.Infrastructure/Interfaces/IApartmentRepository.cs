using ApartmentManagementSystem.Models.Entities;

namespace ApartmentManagementSystem.Infrastructure.Interfaces;

public interface IApartmentRepository
{
    Task<IEnumerable<Apartment?>> GetAllAsync();
    Task<Apartment?> GetByIdAsync(int apartmentId);
    Task AddAsync(Apartment apartment);
    Task UpdateAsync(Apartment apartment);
        
    Task DeleteAsync(int apartmentId);
    Task<bool> IsExistAsync(int apartmentId);
    Task<int> GetApartmentIdsByUserIdAsync(Guid userId);
    Task<List<Apartment>> GetActiveApartmentsByBlock(string block);
}