namespace ApartmentManagementSystem.Core.DTOs.UserDto;

public class UserCreateRequestDto
{
    public string FullName { get; set; } = default!;
    public string IdentityNumber { get; set; } = default!;
    public string? Email { get; set; }
    public string PhoneNumber { get; set; } = default!;
}