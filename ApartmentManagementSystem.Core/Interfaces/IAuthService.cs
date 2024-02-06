using ApartmentManagementSystem.Core.DTOs.AuthDto;
using ApartmentManagementSystem.Models.Shared;

namespace ApartmentManagementSystem.Core.Interfaces;

public interface IAuthService
{
    Task<ResponseDto<string?>> AdminLoginAsync(AuthAdminRequestDto request);
    Task<ResponseDto<string?>> UserLoginAsync(AuthUserRequestDto request);
    Task<ResponseDto<bool>> LogoutAsync();
}