using Domain.Entities;

namespace Domain.Abstractions;

public interface IReservationRepository
{
    Task<List<Reservation>> GetUserReservationsAsync(string userId);
    
    Task<ICollection<Reservation>> GetAllReservationsAsync(int page, int pageSize);
}