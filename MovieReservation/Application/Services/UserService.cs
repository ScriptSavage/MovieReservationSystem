using Application.Abstraction;
using Application.Dto.Auth;
using Application.Dto.Event;
using Application.Dto.Reservation;
using Application.Dto.User;
using Application.Dto.Venue;
using Application.Exceptions;
using Domain.Abstractions;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReservationRepository _reservationRepository;

    public UserService(
        UserManager<ApplicationUser> userManager,
        IUnitOfWork unitOfWork,
        IReservationRepository reservationRepository)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _reservationRepository = reservationRepository;
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

    public async Task ChangeEmailAsync(string userId, ChangeEmailDto dto)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) throw new DoesNotExistsException("User not found");
        
        var emailToken = await _userManager.GenerateChangeEmailTokenAsync(user, dto.NewEmail);
        
        var changeEmailResult = await _userManager.ChangeEmailAsync(user, dto.NewEmail,emailToken);
        if (!changeEmailResult.Succeeded)
        {
            var errors = string.Join(", ", changeEmailResult.Errors.Select(e => e.Description));
            throw new ArgumentException(errors);
        }
    }
    
    public async Task DeleteAccountAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) throw new DoesNotExistsException("User not found");

        await _userManager.DeleteAsync(user);
    }

    public async Task<UserDetailsDto> GetUserDetailsAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) throw new DoesNotExistsException("User not found");
        
        var userDetails = new UserDetailsDto(user.Email,
            user.FirstName, 
            user.LastName, 
            user.PhoneNumber);
        
        return userDetails;
    }

    public async Task<List<ReservationDto>> GetAllReservationsAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) throw new DoesNotExistsException("User not found");

        var userReservations = await _reservationRepository.GetReservationsAsync(userId);

        var result = userReservations.Select(e =>
            new ReservationDto(e.CreatedAt, e.ReservationCode, e.TotalPrice,
                new EventDto(e.Event.StartDate,e.Event.EndDate, 
                    new VenueDto(e.Event.Venue.Name,e.Event.Venue.City,
                        e.Event.Venue.PostalCode,e.Event.Venue.Street)))).ToList();
        
        return result;
    }
}