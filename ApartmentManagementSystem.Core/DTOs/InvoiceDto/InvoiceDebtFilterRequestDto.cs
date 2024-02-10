using ApartmentManagementSystem.Models.Enums;

namespace ApartmentManagementSystem.Core.DTOs.InvoiceDto;

public class InvoiceDebtFilterRequestDto
{
    public int ApartmentId { get; set; }
    public int? Year { get; set; }
    public int? Month { get; set; }
}