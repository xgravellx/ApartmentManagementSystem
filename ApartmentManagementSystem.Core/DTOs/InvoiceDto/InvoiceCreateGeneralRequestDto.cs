using ApartmentManagementSystem.Models.Enums;

namespace ApartmentManagementSystem.Models.Shared;

public class InvoiceCreateGeneralRequestDto
{
    public string Block { get; set; } = default!;
    public InvoiceType Type { get; set; }
    public decimal Amount { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }

}