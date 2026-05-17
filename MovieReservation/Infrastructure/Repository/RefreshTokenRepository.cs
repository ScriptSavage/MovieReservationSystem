using Domain.Abstractions;
using Domain.Entities.Identity;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly DatabaseContext _context;

    public RefreshTokenRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task AddNewRefreshToken(RefreshToken? token)
    {
        _context.RefreshTokens.Add(token);
        await _context.SaveChangesAsync();
    }

    public async Task<RefreshToken?> GetByTokenHashAsync(string tokenHash)
    {
        return await _context.RefreshTokens
            .Include(e=>e.User)
            .FirstOrDefaultAsync(e=>e.Token == tokenHash);
    }
}