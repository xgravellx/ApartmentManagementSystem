namespace ApartmentManagementSystem.Core.DTOs.UserDto;

public class UserAssignToRoleRequestDto
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
}