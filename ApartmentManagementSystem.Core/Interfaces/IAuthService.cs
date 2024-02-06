using ApartmentManagementSystem.Core.DTOs;
using ApartmentManagementSystem.Models.Shared;

namespace ApartmentManagementSystem.Core.Interfaces;

public interface IAuthService
{
    Task<ResponseDto<string?>> AdminLoginAsync(AdminLoginRequestDto request);
    Task<ResponseDto<string?>> UserLoginAsync(UserLoginRequestDto request);
    Task<ResponseDto<bool>> LogoutAsync();
}