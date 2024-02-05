namespace ApartmentManagementSystem.Models.Entities;

public class Invoice
{
    public int InvoiceId { get; set; }
    public string Type { get; set; } // "Elektrik", "Su", "Doğalgaz"
    public decimal Amount { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public bool PaymentStatus { get; set; } // Ödenip ödenmediğini gösterir

    // Foreign Key
    public int ApartmentId { get; set; }

    // Navigation properties
    public virtual Apartment Apartment { get; set; }

}