using ApartmentManagementSystem.Core.DTOs.UserDto;
using ApartmentManagementSystem.Models.Entities;
using ApartmentManagementSystem.Models.Shared;

namespace ApartmentManagementSystem.Core.Interfaces;

public interface IUserService
{
    Task<ResponseDto<List<UserResponseDto>>> GetAll();
    Task<ResponseDto<UserResponseDto>> GetById(Guid userId);
    Task<ResponseDto<Guid?>> CreateUser(UserCreateRequestDto request);
    Task<ResponseDto<bool>> UpdateUser(UserUpdateRequestDto request);
    Task<ResponseDto<bool>> DeleteUser(Guid userId);
    Task<ResponseDto<bool>> AssignRoleToUser(UserAssignToRoleRequestDto request);
}