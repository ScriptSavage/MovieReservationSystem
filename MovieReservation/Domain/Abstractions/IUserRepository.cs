using Domain.Entities.Identity;

namespace Domain.Abstractions;

public interface IUserRepository
{
    Task<ApplicationUser?> GetUserAsync(string userId);

    Task<ICollection<ApplicationUser>> GetUsersReservationsAsync(int  page, int pageSize);
    
    Task<int> CountAllUsersAsync();
}