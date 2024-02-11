using ApartmentManagementSystem.Models.Enums;

namespace ApartmentManagementSystem.Core.DTOs.InvoiceDto;

public class InvoiceCreateDuesRequestDto
{
    public string Block { get; set; } = default!;
    public InvoiceType Type { get; set; }
    public decimal Amount { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public List<int>? ApartmentIds { get; set; }

}