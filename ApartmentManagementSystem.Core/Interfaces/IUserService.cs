using ApartmentManagementSystem.Core.DTOs.UserDto;
using ApartmentManagementSystem.Models.Entities;
using ApartmentManagementSystem.Models.Shared;

namespace ApartmentManagementSystem.Core.Interfaces;

public interface IUserService
{
    Task<ResponseDto<List<UserGetAllResponseDto>>> GetAll();
    Task<ResponseDto<User>> GetById(Guid id);
    Task<ResponseDto<Guid?>> CreateUser(UserCreateRequestDto request);
    Task<ResponseDto<bool>> UpdateUser(UserUpdateRequestDto request);
    Task<ResponseDto<bool>> DeleteUser(Guid id);
}