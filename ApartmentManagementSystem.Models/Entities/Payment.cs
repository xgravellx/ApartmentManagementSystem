namespace ApartmentManagementSystem.Models.Entities;

// Payment'ın ise bu ödeme talebinin karşılanması (yani ödemenin gerçekleştirilmiş olması) anlamına
public class Payment
{
    public int PaymentId { get; set; }
    public string Type { get; set; } = default!; // "Aidat" veya "Fatura"
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentMethod { get; set; } = default!; // "Kredi Kartı" veya "Nakit"

    // Foreign Keys
    public Guid UserId { get; set; }
    public int InvoiceId { get; set; }

    // Navigation properties
    public virtual User User { get; set; }
    public virtual Invoice Invoice { get; set; }

}