namespace ApartmentManagementSystem.Models.Entities;

public class Apartment
{
    public int ApartmentId { get; set; }
    public string Block { get; set; }
    public bool Status { get; set; } // "Dolu" veya "Boş"
    public string Type { get; set; } // Örneğin: "2+1"
    public int Floor { get; set; }
    public string Number { get; set; } // Daire numarası

    // Foreign Key
    public int UserId { get; set; }

    // Navigation properties
    public virtual User User { get; set; }
    public virtual ICollection<Invoice> Invoice { get; set; }

}