namespace ApartmentManagementSystem.Core.DTOs;

public class AdminLoginRequestDto
{
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
}