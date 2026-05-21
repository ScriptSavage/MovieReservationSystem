using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Seeders;

public static class AdminSeeder
{
    public static async Task SeedAdminAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        const string adminEmail = "admin@movieReservations.com";
        const string adminPassword = "SuperStrongPassword123!@#";

        var existingAdmin = await userManager.FindByEmailAsync(adminEmail);

        if (existingAdmin is not null)
            return;

        var newAdmin = new ApplicationUser
        {
            FirstName = "Admin",
            LastName = "Admin",
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };

        var createResult = await userManager.CreateAsync(newAdmin, adminPassword);

        if (!createResult.Succeeded)
        {
            var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
            throw new Exception($"Admin seeding failed: {errors}");
        }

        var roleResult = await userManager.AddToRoleAsync(newAdmin, Roles.Admin);

        if (!roleResult.Succeeded)
        {
            var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
            throw new Exception($"Adding admin role failed: {errors}");
        }
    }
}