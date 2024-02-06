using ApartmentManagementSystem.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using static System.Formats.Asn1.AsnWriter;

namespace ApartmentManagementSystem.Core.Services;

public class StartupService(IServiceProvider serviceProvider) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            // Yönetici rolünün var olup olmadığını kontrol et
            var adminRoleExists = await roleManager.RoleExistsAsync("Admin");
            if (!adminRoleExists)
            {
                // Yönetici rolünü oluştur
                await roleManager.CreateAsync(new Role { Name = "Admin" });
            }

            // Yönetici kullanıcının var olup olmadığını kontrol et
            var adminUser = await userManager.FindByNameAsync("adminUser");
            if (adminUser == null)
            {
                // Yönetici kullanıcıyı oluştur
                var user = new User
                {
                    UserName = "admin", 
                    Email = "admin@admin.com", 
                    FullName = "Admin", 
                    IdentityNumber = "11111111111"
                };
                await userManager.CreateAsync(user, "Admin.123");

                // Yönetici kullanıcıya yönetici rolünü ata
                await userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        // Uygulama kapanırken gerekli temizlik işlemleri burada yapılabilir.
        return Task.CompletedTask;
    }

}