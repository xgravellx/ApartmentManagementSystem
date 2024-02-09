namespace ApartmentManagementSystem.Core.DTOs.UserDto;

public class UserUpdateRequestDto
{
    public Guid UserId { get; set; }
    public string FullName { get; set; }
    public string IdentityNumber { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}