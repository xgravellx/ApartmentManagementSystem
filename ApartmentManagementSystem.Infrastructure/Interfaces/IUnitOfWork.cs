namespace ApartmentManagementSystem.Infrastructure.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IApartmentRepository ApartmentRepository { get; }
    IInvoiceRepository InvoiceRepository { get; }
    IPaymentRepository PaymentRepository { get; }
    Task<int> SaveChangesAsync();
}