using ApartmentManagementSystem.Models.Enums;

namespace ApartmentManagementSystem.Core.DTOs.PaymentDto;

public class PaymentGetByApartmentIdResponseDto
{
    public int PaymentId { get; set; }
    public PaymentType Type { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public int ApartmentId { get; set; }
    public string Block { get; set; } = default!;
}