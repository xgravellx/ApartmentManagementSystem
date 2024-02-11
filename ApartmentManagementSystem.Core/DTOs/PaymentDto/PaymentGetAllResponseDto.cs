using ApartmentManagementSystem.Models.Enums;

namespace ApartmentManagementSystem.Core.DTOs.PaymentDto;

public class PaymentGetAllResponseDto
{
    public int PaymentId { get; set; }
    public PaymentType Type { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public PaymentMethod Method { get; set; }
    public Guid UserId { get; set; }
}