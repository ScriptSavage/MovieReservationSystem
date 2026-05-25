using Application.Dto;
using Application.Dto.Auth;
using Application.Dto.Reservation;
using Application.Dto.User;

namespace Application.Abstraction;

public interface IUserService
{
    Task ChangePasswordAsync(string userId ,ChangePasswordDto dto);
    
    Task ChangeEmailAsync(string userId, ChangeEmailDto dto);
    
    Task DeleteAccountAsync(string userId);
    
    Task<UserDetailsDto> GetUserDetailsAsync(string userId);
    
    Task<List<ReservationDto>> GetAllReservationsAsync(string userId);
    
    Task<PagedResult<UserReservationDto>> GetAllUsersReservationsAsync(int page, int pageSize);
}