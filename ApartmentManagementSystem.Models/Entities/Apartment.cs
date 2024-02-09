namespace ApartmentManagementSystem.Models.Entities;

public class Apartment
{
    public int ApartmentId { get; set; }
    public string Block { get; set; } = default!;
    public string BlockId { get; set; } = default!;
    public bool Status { get; set; } // "Dolu" veya "Boş"
    public string Type { get; set; } = default!; // Örneğin: "2+1"
    public int Floor { get; set; }
    public int Number { get; set; } // Daire numarası

    // Foreign Key (Nullable)
    public Guid? UserId { get; set; }

    // Navigation properties
    public virtual User? User { get; set; }
    public virtual ICollection<Invoice> Invoice { get; set; }

}