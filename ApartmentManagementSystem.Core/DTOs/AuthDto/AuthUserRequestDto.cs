namespace ApartmentManagementSystem.Core.DTOs.AuthDto;

public class AuthUserRequestDto
{
    public string IdentityNumber { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
}