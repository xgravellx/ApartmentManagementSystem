using ApartmentManagementSystem.Infrastructure.Interfaces;
using ApartmentManagementSystem.Infrastructure.Repositories;

namespace ApartmentManagementSystem.Infrastructure.Data;

public class UnitOfWork(AppDbContext context, IApartmentRepository apartmentRepository, IInvoiceRepository invoiceRepository, IPaymentRepository paymentRepository) : IUnitOfWork
{
    public IApartmentRepository ApartmentRepository { get; private set; } = apartmentRepository;
    public IInvoiceRepository InvoiceRepository { get; private set; } = invoiceRepository;
    public IPaymentRepository PaymentRepository { get; private set; } = paymentRepository;

    public async Task<int> SaveChangesAsync()
    {
        return await context.SaveChangesAsync();
    }

    public void Dispose()
    {
        context.Dispose();
    }
}