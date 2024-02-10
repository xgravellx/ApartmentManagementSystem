using ApartmentManagementSystem.Models.Entities;

namespace ApartmentManagementSystem.Infrastructure.Interfaces;

public interface IApartmentRepository
{
    Task<IEnumerable<Apartment?>> GetAllAsync();
    Task<Apartment?> GetByIdAsync(int apartmentId);
    Task AddAsync(Apartment apartment);
    Task UpdateAsync(Apartment apartment);
    Task DeleteAsync(int apartmentId);
    Task<bool> IsUserAssignedToAnyApartmentAsync(Guid userId);
    Task<int> GetApartmentIdByUserIdAsync(Guid userId);
    Task<List<Apartment>> GetActiveApartmentsByBlockAsync(string block);
    Task<bool> AreApartmentIdsExistAsync(List<int> apartmentIds);
    Task<bool> CheckApartmentFloorAndNumberExistAsync(int floor, int number);
    Task<bool> AreAllApartmentsActiveAsync(List<int> apartmentIds);
    Task<bool> IsUserIdAssignedToAnotherApartment(int apartmentId, string userId);
    Task<List<Apartment>> FindByUserIdAsync(Guid userId);
}