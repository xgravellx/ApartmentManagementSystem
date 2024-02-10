namespace ApartmentManagementSystem.Core.DTOs.ApartmentDto;

public class ApartmentCreateRequestDto
{
    public string Block { get; set; } = default!;
    public string BlockId { get; set; } = default!;
    public bool Status { get; set; }
    public string Type { get; set; } = default!;
    public int Floor { get; set; }
    public int Number { get; set; }

    public Guid? UserId { get; set; } // Daire sahibinin kullanıcı Id'si (isteğe bağlı)

}