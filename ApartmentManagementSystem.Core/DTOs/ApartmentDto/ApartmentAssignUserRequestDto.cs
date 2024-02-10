namespace ApartmentManagementSystem.Core.DTOs.ApartmentDto;

public class ApartmentAssignUserRequestDto
{
    public int ApartmentId { get; set; } = default!;
    public Guid UserId { get; set; } = default!;
}