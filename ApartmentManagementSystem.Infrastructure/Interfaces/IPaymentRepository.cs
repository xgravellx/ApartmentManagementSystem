using ApartmentManagementSystem.Models.Entities;
using ApartmentManagementSystem.Models.Shared;

namespace ApartmentManagementSystem.Infrastructure.Interfaces;

public interface IPaymentRepository
{
    Task<List<Payment>> GetAllAsync();
    Task<List<Payment>> GetByApartmentIdAsync(int apartmentId);
    Task<List<Guid>> GetUserIdsWithRegularPayments(int month, int year);
    Task AddPaymentAsync(Payment payment);
    Task<Payment> GetByIdAsync(int paymentId);
    Task UpdatePaymentAsync(Payment payment);
    Task DeletePaymentAsync(int paymentId);
    Task<Invoice?> GetInvoiceByPaymentIdAsync(int paymentId);

}