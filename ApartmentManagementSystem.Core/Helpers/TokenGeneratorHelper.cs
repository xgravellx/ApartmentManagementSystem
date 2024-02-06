using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApartmentManagementSystem.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ApartmentManagementSystem.Core.Helpers;

public class TokenGeneratorHelper(UserManager<User> userManager, IConfiguration configuration)
{
    public async Task<string> CreateTokenAsync(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var signatureKey = configuration.GetSection("TokenOptions")["SignatureKey"]!;
        var tokenExpireAsHour = configuration.GetSection("TokenOptions")["Expire"]!;
        var issuer = configuration.GetSection("TokenOptions")["Issuer"]!;

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signatureKey));

        var signingCredentials =
            new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Sub, user.UserName!),
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new (ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        // Add role claims
        var roles = await userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddHours(Convert.ToDouble(tokenExpireAsHour)),
            SigningCredentials = signingCredentials,
            Issuer = issuer
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

}