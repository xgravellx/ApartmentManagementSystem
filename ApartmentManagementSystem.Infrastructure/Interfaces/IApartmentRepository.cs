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
    Task<int> GetApartmentIdsByUserIdAsync(Guid userId);
    Task<List<Apartment>> GetActiveApartmentsByBlockAsync(string block);
    Task<bool> AreApartmentIdsExistAsync(List<int> apartmentIds);
    //Task<List<Invoice>> GetInvoicesByApartmentIdAsync(int lastYear);
    Task<bool> CheckApartmentFloorAndNumberExistAsync(int floor, int number);
}