using ApartmentManagementSystem.Core.DTOs.AuthDto;
using ApartmentManagementSystem.Models.Shared;

namespace ApartmentManagementSystem.Core.Interfaces;

public interface IAuthService
{
    Task<ResponseDto<string?>> AdminLogin(AuthAdminRequestDto request);
    Task<ResponseDto<string?>> UserLogin(AuthUserRequestDto request);
    Task<ResponseDto<bool>> Logout();
}