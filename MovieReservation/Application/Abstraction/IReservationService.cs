using Application.Dto;
using Application.Dto.Reservation;
using Application.Dto.User;

namespace Application.Abstraction;

public interface IReservationService
{
    Task<PagedResult<UserReservationDto>> GetReservations(int page, int pageSize);
    
    Task<UserReservationDetailsDto> GetReservation(long id);
    
    Task CreateReservationAsync(string UserId, CreateReservationDto createReservationDto);
}