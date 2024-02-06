namespace ApartmentManagementSystem.Core.DTOs.UserDto;

public class UserGetAllResponseDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string IdentityNumber { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

}