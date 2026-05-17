using Domain.Entities.Identity;

namespace Domain.Abstractions;

public interface IUserRepository
{
    Task<ApplicationUser?> GetUserAsync(string userId);
}