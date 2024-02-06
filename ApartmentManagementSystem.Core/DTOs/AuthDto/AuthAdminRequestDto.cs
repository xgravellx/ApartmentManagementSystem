namespace ApartmentManagementSystem.Core.DTOs.AuthDto;

public class AuthAdminRequestDto
{
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
}