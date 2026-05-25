using Domain.Abstractions;
using Domain.Entities.Identity;
using Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class UserRepository : IUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly DatabaseContext _databaseContext;

    public UserRepository(
        UserManager<ApplicationUser> userManager,
        DatabaseContext databaseContext)
    {
        _userManager = userManager;
        _databaseContext = databaseContext;
    }

    public async Task<ApplicationUser?> GetUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        return user;
    }

    public async Task<ICollection<ApplicationUser>> GetUsersReservationsAsync(int page, int pageSize)
    {
        var data = await _databaseContext.ApplicationUsers
            .Include(e=>e.Reservations)
            .ThenInclude(e=>e.Event)
            .ThenInclude(e=>e.Venue)
            .OrderBy(e=>e.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        return data;
    }

    public Task<int> CountAllUsersAsync()
    {
        return _databaseContext.ApplicationUsers.CountAsync();
    }
}