using Domain.Entities;

namespace Domain.Abstractions;

public interface IReservationRepository
{
    Task<List<Reservation>> GetUserReservationsAsync(string userId);
    
    Task<ICollection<Reservation>> GetAllReservationsAsync(int page, int pageSize);
    
    Task<Reservation> GetReservationAsync(long id);
    
    Task<bool> DoesReservationExistAsync(long id);
    
    Task AddNewReservationAsync(Reservation reservation);
}