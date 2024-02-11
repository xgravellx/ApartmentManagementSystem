namespace ApartmentManagementSystem.Models.Entities;

public class Apartment
{
    public int ApartmentId { get; set; }
    public string Block { get; set; } = default!;
    public int BlockId { get; set; }
    public bool Status { get; set; } // "Dolu" veya "Boş"
    public string Type { get; set; } = default!;
    public int Floor { get; set; }
    public int Number { get; set; }

    // Foreign Key
    public Guid? UserId { get; set; }

    public virtual User? User { get; set; }
    public virtual ICollection<Invoice> Invoice { get; set; }

}