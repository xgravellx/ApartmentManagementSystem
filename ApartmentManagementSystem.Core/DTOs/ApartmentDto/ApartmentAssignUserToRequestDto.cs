namespace ApartmentManagementSystem.Core.DTOs.ApartmentDto;

public class ApartmentAssignUserToRequestDto
{
    public int ApartmentId { get; set; } = default!;
    public Guid UserId { get; set; } = default!;
}