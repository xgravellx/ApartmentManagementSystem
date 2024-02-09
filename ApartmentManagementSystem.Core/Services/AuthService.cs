using ApartmentManagementSystem.Core.DTOs.AuthDto;
using ApartmentManagementSystem.Core.Helpers;
using ApartmentManagementSystem.Core.Interfaces;
using ApartmentManagementSystem.Models.Entities;
using ApartmentManagementSystem.Models.Shared;
using Microsoft.AspNetCore.Identity;

namespace ApartmentManagementSystem.Core.Services;

public class AuthService(UserManager<User> userManager, SignInManager<User> signInManager, TokenGeneratorHelper tokenGeneratorHelper) : IAuthService
{
    public async Task<ResponseDto<string?>> AdminLogin(AuthAdminRequestDto request)
    {
        var hasAdmin = await userManager.FindByNameAsync(request.UserName);
        if (hasAdmin == null)
        {
            return ResponseDto<string?>.Fail("Username or password is wrong");
        }

        var checkPassword = await userManager.CheckPasswordAsync(hasAdmin, request.Password);

        if (!checkPassword)
        {
            return ResponseDto<string?>.Fail("Username or password is wrong");
        }

        var token = await tokenGeneratorHelper.CreateTokenAsync(hasAdmin);
        return ResponseDto<string?>.Success(token);
    }

    public async Task<ResponseDto<string?>> UserLogin(AuthUserRequestDto request)
    {
        var user = userManager.Users.FirstOrDefault(u => u.IdentityNumber == request.IdentityNumber && u.PhoneNumber == request.PhoneNumber);

        if (user == null)
        {
            return ResponseDto<string?>.Fail("Identity or phone number is wrong");
        }

        var token = await tokenGeneratorHelper.CreateTokenAsync(user);
        return ResponseDto<string?>.Success(token);
    }

    public async Task<ResponseDto<bool>> Logout()
    {
        await signInManager.SignOutAsync();
        return ResponseDto<bool>.Success(true);
    }
}