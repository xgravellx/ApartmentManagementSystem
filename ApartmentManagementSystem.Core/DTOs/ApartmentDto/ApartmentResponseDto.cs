using ApartmentManagementSystem.Core.DTOs.InvoiceDto;

namespace ApartmentManagementSystem.Core.DTOs.ApartmentDto;

public class ApartmentResponseDto
{
    public int ApartmentId { get; set; }
    public string Block { get; set; }
    public bool Status { get; set; }
    public string Type { get; set; }
    public int Floor { get; set; }
    public int Number { get; set; }

    public Guid? UserId { get; set; }

    public List<InvoiceResponseDto> Invoice { get; set; }
}