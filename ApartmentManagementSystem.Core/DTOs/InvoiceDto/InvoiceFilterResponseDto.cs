using ApartmentManagementSystem.Models.Enums;

namespace ApartmentManagementSystem.Core.DTOs.InvoiceDto;

public class InvoiceFilterResponseDto
{
    public decimal TotalAmount { get; set; }
    public List<InvoiceResponseDto> Invoices { get; set; } = [];
}