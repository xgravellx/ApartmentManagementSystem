using ApartmentManagementSystem.Core.DTOs.UserDto;
using ApartmentManagementSystem.Core.Interfaces;
using ApartmentManagementSystem.Models.Entities;
using ApartmentManagementSystem.Models.Shared;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ApartmentManagementSystem.Core.Services;

public class UserService(UserManager<User> userManager, RoleManager<Role> roleManager, IMapper mapper) : IUserService
{
    public async Task<ResponseDto<List<UserResponseDto>>> GetAll()
    {
        var users = await userManager.Users.ToListAsync(); 
        var userList = new List<UserResponseDto>();

        foreach (var user in users)
        {
            var roles = await userManager.GetRolesAsync(user);
            userList.Add(new UserResponseDto
            {
                UserId = user.Id,
                FullName = user.FullName,
                IdentityNumber = user.IdentityNumber,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = roles.FirstOrDefault()
            });
        }

        return ResponseDto<List<UserResponseDto>>.Success(userList ?? []);

    }

    public async Task<ResponseDto<UserResponseDto>> GetById(Guid userId)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());

        if (user == null)
        {
            return ResponseDto<UserResponseDto>.Fail("User is not found");
        }

        var foundUser = mapper.Map<UserResponseDto>(user);
        return ResponseDto<UserResponseDto>.Success(foundUser);
    }

    public async Task<ResponseDto<Guid?>> CreateUser(UserCreateRequestDto request)
    {
        var user = new User
        {
            FullName = request.FullName,
            IdentityNumber = request.IdentityNumber,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            UserName = request.IdentityNumber
        };


        var result = await userManager.CreateAsync(user, request.PhoneNumber);

        if (!result.Succeeded)
        {
            return ResponseDto<Guid?>.Fail("User not created.");
        }

        return ResponseDto<Guid?>.Success(user.Id);
    }

    public async Task<ResponseDto<bool>> UpdateUser(UserUpdateRequestDto request)
    {
        var user = await userManager.FindByIdAsync(request.UserId.ToString());

        if (user == null)
        {
            return ResponseDto<bool>.Fail("User not found");
        }

        user.FullName = request.FullName;
        user.IdentityNumber = request.IdentityNumber;
        user.PhoneNumber = request.PhoneNumber;
        user.Email = request.Email;
        user.UserName = request.IdentityNumber;

        var result = await userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            return ResponseDto<bool>.Fail("User failed to update.");
        }

        await userManager.RemovePasswordAsync(user);
        await userManager.AddPasswordAsync(user, request.PhoneNumber);

        return ResponseDto<bool>.Success(true);
    }

    public async Task<ResponseDto<bool>> DeleteUser(Guid userId)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());

        if (user == null)
        {
            return ResponseDto<bool>.Fail("User is not found");
        }

        await userManager.DeleteAsync(user);

        return ResponseDto<bool>.Success(true);
    }

    public async Task<ResponseDto<bool>> AssignRoleToUser(UserAssignToRoleRequestDto request)
    {
        var user = await userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
        {
            return ResponseDto<bool>.Fail("User not found");
        }

        var role = await roleManager.FindByIdAsync(request.RoleId.ToString());
        if (role == null)
        {
            return ResponseDto<bool>.Fail("Role not found");
        }

        await userManager.AddToRoleAsync(user, role.Name);

        return ResponseDto<bool>.Success(true);
    }


}