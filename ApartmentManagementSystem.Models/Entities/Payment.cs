using ApartmentManagementSystem.Models.Enums;

namespace ApartmentManagementSystem.Models.Entities;

public class Payment
{
    public int PaymentId { get; set; }
    public PaymentType Type { get; set; } = default!; // "Aidat" veya "Fatura"
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public PaymentMethod Method { get; set; } = default!; // "Kredi Kartı" veya "Nakit"

    // Foreign Keys
    public Guid UserId { get; set; }
    public int InvoiceId { get; set; }

    // Navigation properties
    public virtual User User { get; set; }
    public virtual Invoice Invoice { get; set; }

}