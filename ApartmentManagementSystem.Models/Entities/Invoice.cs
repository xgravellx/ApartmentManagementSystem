using ApartmentManagementSystem.Models.Enums;

namespace ApartmentManagementSystem.Models.Entities;

// Invoice'ın bir ödeme talebini (yani ödenmesi gereken bir miktarı ve bunun ne için olduğunu) temsil etmesi
public class Invoice
{
    public int InvoiceId { get; set; }
    public InvoiceType Type { get; set; } = default!; // "Elektrik", "Su", "Doğalgaz" // todo -> aidat eklenecek
    public decimal Amount { get; set; }
    public decimal PayableAmount { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public DateTime DueDate { get; set; }
    public bool PaymentStatus { get; set; } // Ödenip ödenmediğini gösterir

    // Foreign Key
    public int ApartmentId { get; set; }

    // Navigation properties
    public virtual Apartment Apartment { get; set; }

}