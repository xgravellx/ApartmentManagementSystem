namespace ApartmentManagementSystem.Models.Shared;

public class InvoiceFilterRequestDto
{
    public List<int>? ApartmentIds { get; set; }
    public List<int>? Months { get; set; }
    public List<int>? Years { get; set; }
    public List<Guid>? UserIds { get; set; }
    public bool? PaymentStatus { get; set; }
}
