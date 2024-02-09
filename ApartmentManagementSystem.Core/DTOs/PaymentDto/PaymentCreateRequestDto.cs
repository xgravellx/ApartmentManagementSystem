using ApartmentManagementSystem.Models.Enums;

namespace ApartmentManagementSystem.Core.DTOs.PaymentDto;

public class PaymentCreateRequestDto
{
    public Guid UserId { get; set; }
    public int ApartmentId { get; set; }
    public int InvoiceId { get; set; }
    public decimal Amount { get; set; }

    public PaymentType Type { get; set; }
    public PaymentMethod Method { get; set; }
}