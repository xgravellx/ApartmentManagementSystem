using ApartmentManagementSystem.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ApartmentManagementSystem.Core.Initialization;

public class RolesInitialization(IServiceProvider serviceProvider) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            // Rol oluşturma işlemleri
            await CreateRolesAsync(roleManager);

            // Yönetici kullanıcı oluşturma işlemleri
            await CreateAdminUserAsync(userManager);
        }
    }

    private async Task CreateRolesAsync(RoleManager<Role> roleManager)
    {
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new Role { Name = "Admin" });
        }

        if (!await roleManager.RoleExistsAsync("User"))
        {
            await roleManager.CreateAsync(new Role { Name = "User" });
        }
    }

    private async Task CreateAdminUserAsync(UserManager<User> userManager)
    {
        // Kullanıcı adı "admin" olduğu için, "admin" ile arama yapılmalıdır.
        var adminUser = await userManager.FindByNameAsync("admin");

        if (adminUser == null)
        {
            adminUser = new User
            {
                FullName = "admin",
                Email = "admin@admin.com",
                UserName = "admin",
                IdentityNumber = "12345678900",
            };

            var result = await userManager.CreateAsync(adminUser, "Admin.1");
            if (!result.Succeeded)
            {
                var errorDescription = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to create admin user: {errorDescription}");
            }

            var addToRoleResult = await userManager.AddToRoleAsync(adminUser, "Admin");
            if (!addToRoleResult.Succeeded)
            {
                var roleErrorDescription = string.Join(", ", addToRoleResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to assign 'Admin' role to the admin user: {roleErrorDescription}");
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        // Uygulama kapanırken gerekli temizlik işlemleri burada yapılabilir.
        return Task.CompletedTask;
    }

}