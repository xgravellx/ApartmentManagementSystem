﻿using ApartmentManagementSystem.Models.Entities;
using ApartmentManagementSystem.Models.Shared;

namespace ApartmentManagementSystem.Infrastructure.Interfaces;

public interface IInvoiceRepository
{
    Task<IEnumerable<Invoice>> GetAllAsync();

    Task<IEnumerable<Invoice>> GetByApartmentIdAsync(int apartmentId);

    Task<IEnumerable<Invoice>> GetFilteredAsync(InvoiceFilterRequestDto request);
    Task<Invoice> AddInvoiceAsync(Invoice invoice);
    Task<Invoice> GetByIdAsync(int invoiceId);
    Task<bool> UpdateInvoiceAsync(Invoice invoice);
    Task<bool> DeleteInvoiceAsync(int invoiceId);
}