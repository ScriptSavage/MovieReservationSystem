using Domain.Abstractions;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repository;

public class UserRepository : IUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserRepository(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ApplicationUser?> GetUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        return user;
    }
}