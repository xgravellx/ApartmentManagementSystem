namespace ApartmentManagementSystem.Core.DTOs.UserDto;

public class UserResponseDto
{
    public Guid UserId { get; set; }
    public string FullName { get; set; } = default!;
    public string IdentityNumber { get; set; } = default!;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; } = default!;
    public string? Role { get; set; }
}