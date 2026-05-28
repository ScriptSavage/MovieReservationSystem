using Application.Dto;
using Application.Dto.User;

namespace Application.Abstraction;

public interface IReservationService
{
    Task<PagedResult<UserReservationDto>> GetReservations(int page, int pageSize);
}