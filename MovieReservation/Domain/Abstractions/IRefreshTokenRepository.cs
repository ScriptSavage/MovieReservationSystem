using Domain.Entities.Identity;

namespace Domain.Abstractions;

public interface IRefreshTokenRepository
{
    Task AddNewRefreshToken(RefreshToken? token);

    Task<RefreshToken?> GetByTokenHashAsync(string tokenHash);
    
    
}