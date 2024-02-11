using ApartmentManagementSystem.Models.Enums;

namespace ApartmentManagementSystem.Models.Entities;

public class Invoice
{
    public int InvoiceId { get; set; }
    public InvoiceType Type { get; set; } = default!;
    public decimal Amount { get; set; }
    public decimal PayableAmount { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public DateTime DueDate { get; set; }
    public bool PaymentStatus { get; set; }

    public int ApartmentId { get; set; }

    public virtual Apartment Apartment { get; set; }

}