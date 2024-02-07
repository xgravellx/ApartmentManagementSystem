namespace ApartmentManagementSystem.Core.DTOs.InvoiceDto;

public class InvoiceByApartmentIdRequestDto
{
    public int ApartmentId { get; set; } = default!;
    public Guid UserId { get; set; } = default!;
}