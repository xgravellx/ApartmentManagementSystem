using ApartmentManagementSystem.Models.Enums;

namespace ApartmentManagementSystem.Core.DTOs.InvoiceDto;

public class InvoiceUpdateRequestDto
{
    public int InvoiceId { get; set; }
    public string Block { get; set; } = default!;
    public InvoiceType Type { get; set; }
    public decimal Amount { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public bool PaymentStatus { get; set; }
}