namespace ApartmentManagementSystem.Infrastructure.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IApartmentRepository ApartmentRepository { get; }
    public IInvoiceRepository InvoiceRepository { get; }
}