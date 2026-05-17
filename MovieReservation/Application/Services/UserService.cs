using Application.Abstraction;
using Application.Dto.Auth;
using Application.Exceptions;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task ChangePasswordAsync(string userId, ChangePasswordDto dto)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            throw new DoesNotExistsException("User not found");
        }

        var passwordResult = await _userManager.ChangePasswordAsync(
            user,
            dto.OldPassword,
            dto.NewPassword);

        if (!passwordResult.Succeeded)
        {
            throw new ArgumentException("Passwords don't match");
        }
    }
    
}