using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Abstraction;
using Application.Dto.Auth;
using Application.Exceptions;
using Application.Security;
using Domain.Abstractions;
using Domain.Entities.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    private readonly IValidator<RegisterUserDto.RegisterUserRequest> _registerUserDtoValidator;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        IUnitOfWork unitOfWork,
        IConfiguration configuration,
        IValidator<RegisterUserDto.RegisterUserRequest> registerUserDtoValidator,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _configuration = configuration;
        _registerUserDtoValidator = registerUserDtoValidator;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<IdentityResult> RegisterUser(RegisterUserDto.RegisterUserRequest dto)
    {
        var validationResult = await _registerUserDtoValidator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var newUser = new ApplicationUser
        {
            Email = dto.Email,
            UserName = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            PhoneNumber = dto.PhoneNumber
        };

        var transaction = await _unitOfWork.BeginTransactionAsync();

        try
        {
            var createResult = await _userManager.CreateAsync(newUser, dto.Password);

            if (!createResult.Succeeded)
            {
                transaction.Rollback();
                return createResult;
            }

            var roleResult = await _userManager.AddToRoleAsync(newUser, Roles.User);

            if (!roleResult.Succeeded)
            {
                transaction.Rollback();
                return roleResult;
            }

            transaction.Commit();

            return IdentityResult.Success;
        }
        catch (Exception ex)
        {
            transaction.Rollback();

            return IdentityResult.Failed(new IdentityError
            {
                Code = "RegistrationFailed",
                Description = ex.Message
            });
        }
    }

    public async Task<LoginUserDto.Response> LoginAsync(LoginUserDto.Request dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user is null)
        {
            throw new InvalidLoginException("Incorrect login or password");
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Password);

        if (!isPasswordValid)
        {
            throw new InvalidLoginException("Incorrect login or password");
        }

        var claims = await CreateClaimsAsync(user);

        var accessToken = JwtConfiguration.GenerateToken(claims, _configuration);

        var refreshTokenValue = GenerateRefreshToken();
        var refreshTokenHash = HashRefreshToken(refreshTokenValue);

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = refreshTokenHash,
            ExpiresOut = DateTime.UtcNow.AddHours(72)
        };

        await _refreshTokenRepository.AddNewRefreshToken(refreshToken);
        await _unitOfWork.SaveChangesAsync(CancellationToken.None);

        return new LoginUserDto.Response(
            AccessToken: accessToken,
            RefreshToken: refreshTokenValue
        );
    }

    public async Task<LoginUserDto.Response> RefreshTokenAsync(RefreshTokenDto.Request dto)
    {
        var refreshTokenHash = HashRefreshToken(dto.RefreshToken);

        var existingRefreshToken = await _refreshTokenRepository
            .GetByTokenHashAsync(refreshTokenHash);

        if (existingRefreshToken is null || !existingRefreshToken.IsActive)
        {
            throw new InvalidLoginException("Invalid refresh token");
        }

        var user = existingRefreshToken.User;

        if (user is null)
        {
            throw new InvalidLoginException("Invalid refresh token");
        }

        var claims = await CreateClaimsAsync(user);

        var newAccessToken = JwtConfiguration.GenerateToken(claims, _configuration);

        var newRefreshTokenValue = GenerateRefreshToken();
        var newRefreshTokenHash = HashRefreshToken(newRefreshTokenValue);

        existingRefreshToken.RevokedAt = DateTime.UtcNow;
        existingRefreshToken.ReplacedByTokenHash = newRefreshTokenHash;

        var newRefreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = newRefreshTokenHash,
            ExpiresOut = DateTime.UtcNow.AddHours(72)
        };

        await _refreshTokenRepository.AddNewRefreshToken(newRefreshToken);
        await _unitOfWork.SaveChangesAsync(CancellationToken.None);

        return new LoginUserDto.Response(
            AccessToken: newAccessToken,
            RefreshToken: newRefreshTokenValue
        );
    }

    private async Task<List<Claim>> CreateClaimsAsync(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email ?? string.Empty)
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        return claims;
    }

    private static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }

    private static string HashRefreshToken(string refreshToken)
    {
        var bytes = Encoding.UTF8.GetBytes(refreshToken);
        var hash = SHA256.HashData(bytes);

        return Convert.ToBase64String(hash);
    }
}