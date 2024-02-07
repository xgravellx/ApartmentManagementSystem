namespace ApartmentManagementSystem.Core.DTOs.UserDto;

public class UserGetAllResponseDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = default!;
    public string IdentityNumber { get; set; } = default!;
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string PhoneNumber { get; set; } = default!;
    public string? Role { get; set; }
}