using ApartmentManagementSystem.Models.Enums;

namespace ApartmentManagementSystem.Core.DTOs.InvoiceDto;

public class InvoiceResponseDto
{
    public int InvoiceId { get; set; } = default!;
    public InvoiceType Type { get; set; } = default!; // "Elektrik", "Su", "Doğalgaz"
    public decimal Amount { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public bool PaymentStatus { get; set; }
}