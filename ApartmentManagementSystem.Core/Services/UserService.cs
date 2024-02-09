using ApartmentManagementSystem.Core.DTOs.UserDto;
using ApartmentManagementSystem.Core.Interfaces;
using ApartmentManagementSystem.Infrastructure.Interfaces;
using ApartmentManagementSystem.Models.Entities;
using ApartmentManagementSystem.Models.Enums;
using ApartmentManagementSystem.Models.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace ApartmentManagementSystem.Core.Services;

public class UserService(UserManager<User> userManager, RoleManager<Role> roleManager, IUnitOfWork unitOfWork) : IUserService
{
    public async Task<ResponseDto<List<UserGetAllResponseDto>>> GetAll()
    {
        var users = await userManager.Users.ToListAsync(); // Kullanıcıları önce listeye çek
        var userList = new List<UserGetAllResponseDto>();

        foreach (var user in users)
        {
            var roles = await userManager.GetRolesAsync(user); // Kullanıcının rollerini asenkron olarak al
            userList.Add(new UserGetAllResponseDto
            {
                Id = user.Id,
                FullName = user.FullName,
                IdentityNumber = user.IdentityNumber,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = roles.FirstOrDefault() // Roller listesinin ilk elemanını alır, roller boşsa null döner
            });
        }

        if (!userList.Any())
        {
            return ResponseDto<List<UserGetAllResponseDto>>.Fail("No users found.");
        }

        return ResponseDto<List<UserGetAllResponseDto>>.Success(userList);

    }


    public async Task<ResponseDto<User>> GetById(Guid id)
    {
        var user = await userManager.FindByIdAsync(id.ToString());

        if (user == null)
        {
            return ResponseDto<User>.Fail("User is not found");
        }

        return ResponseDto<User>.Success(user);
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
        {   // Error durumunu düzelt
            var errors = result.Errors.Select(e => e.Description).ToArray();
            return ResponseDto<Guid?>.Fail(string.Join("", errors));
        }
        else
        {
            return ResponseDto<Guid?>.Success(user.Id);
        }
    }

    public async Task<ResponseDto<bool>> UpdateUser(UserUpdateRequestDto request)
    {
        var user = await userManager.FindByIdAsync(request.Id.ToString());

        if (user == null)
        {
            return ResponseDto<bool>.Fail("User is not found");
        }

        user.FullName = request.FullName;
        user.IdentityNumber = request.IdentityNumber;
        user.PhoneNumber = request.PhoneNumber;
        user.Email = request.Email;
        user.UserName = request.IdentityNumber;

        var result = await userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToArray();
            return ResponseDto<bool>.Fail(string.Join("", errors));
        }

        var removePasswordResult = await userManager.RemovePasswordAsync(user);

        if (!removePasswordResult.Succeeded)
        {
            var errors = removePasswordResult.Errors.Select(e => e.Description).ToArray();
            return ResponseDto<bool>.Fail(string.Join(" ", errors));
        }

        var addPasswordResult = await userManager.AddPasswordAsync(user, request.PhoneNumber);
        if (!addPasswordResult.Succeeded)
        {
            var errors = addPasswordResult.Errors.Select(e => e.Description).ToArray();
            return ResponseDto<bool>.Fail(string.Join(" ", errors));
        }

        return ResponseDto<bool>.Success(true);
    }

    public async Task<ResponseDto<bool>> DeleteUser(Guid id)
    {
        var user = await userManager.FindByIdAsync(id.ToString());

        if (user == null)
        {
            return ResponseDto<bool>.Fail("User is not found");
        }

        var result = await userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToArray();
            return ResponseDto<bool>.Fail(string.Join(" ", errors));
        }

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

        var result = await userManager.AddToRoleAsync(user, role.Name);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToArray();
            return ResponseDto<bool>.Fail(string.Join(" ", errors));
        }

        return ResponseDto<bool>.Success(true);
    }


}