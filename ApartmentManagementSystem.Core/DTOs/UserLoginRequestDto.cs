namespace ApartmentManagementSystem.Core.DTOs;

public class UserLoginRequestDto
{
    public string IdentityNumber { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
}