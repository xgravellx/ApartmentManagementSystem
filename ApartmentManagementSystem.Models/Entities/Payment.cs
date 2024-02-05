namespace ApartmentManagementSystem.Models.Entities;

public class Payment
{
    public int PaymentId { get; set; }
    public string Type { get; set; } // "Aidat" veya "Fatura"
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentMethod { get; set; } // "Kredi Kartı" veya "Nakit"

    // Foreign Keys
    public int UserId { get; set; }
    public int InvoiceId { get; set; }

    // Navigation properties
    public virtual User User { get; set; }
    public virtual Invoice Invoice { get; set; }

}