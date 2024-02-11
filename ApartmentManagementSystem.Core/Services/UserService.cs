using ApartmentManagementSystem.Core.DTOs.UserDto;
using ApartmentManagementSystem.Core.Interfaces;
using ApartmentManagementSystem.Infrastructure.Interfaces;
using ApartmentManagementSystem.Infrastructure.Repositories;
using ApartmentManagementSystem.Models.Entities;
using ApartmentManagementSystem.Models.Shared;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ApartmentManagementSystem.Core.Services;

public class UserService(UserManager<User> userManager, RoleManager<Role> roleManager, IMapper mapper, IUnitOfWork unitOfWork) : IUserService
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
                ApartmentId = user.ApartmentId,
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

        var apartments = await unitOfWork.ApartmentRepository.FindByUserIdAsync(userId);

        foreach (var apartment in apartments)
        {
            apartment.UserId = null;
            apartment.Status = false;
            await unitOfWork.ApartmentRepository.UpdateAsync(apartment);
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

        await userManager.AddToRoleAsync(user, role.Name);

        return ResponseDto<bool>.Success(true);
    }

    public async Task<ResponseDto<List<UserRegularResponseDto>>> GetRegularPayingUsers()
    {
        var users = await userManager.Users.Where(u => u.Regular).ToListAsync();
        var userList = new List<UserRegularResponseDto>();

        foreach (var user in users)
        {
            userList.Add(new UserRegularResponseDto
            {
                UserId = user.Id,
                FullName = user.FullName,
                IdentityNumber = user.IdentityNumber,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Regular = user.Regular,
                ApartmentId = user.ApartmentId
            });
        }

        return ResponseDto<List<UserRegularResponseDto>>.Success(userList);
    }

    public async Task UpdateRegularUserAsync()
    {
        var lastCompleteYear = DateTime.UtcNow.AddYears(-1).Year;
        var yearStart = new DateTime(lastCompleteYear, 1, 1);
        var yearEnd = new DateTime(lastCompleteYear, 12, 31);

        var users = await userManager.Users.ToListAsync();

        foreach (var user in users)
        {
            var roles = await userManager.GetRolesAsync(user);

            if (user.ApartmentId.HasValue && roles.Contains("User"))
            {
                var invoices = await unitOfWork.InvoiceRepository.GetByApartmentIdAsync(user.ApartmentId!.Value);
                if (!invoices.Any())
                {
                    continue;
                }

                var paymentsInLastYear = user.Payment?
                    .Where(p => p.Date >= yearStart && p.Date <= yearEnd)
                    .ToList() ?? [];

                user.Regular = paymentsInLastYear.Count > 0;
                await userManager.UpdateAsync(user);
            }
        }

    }

}