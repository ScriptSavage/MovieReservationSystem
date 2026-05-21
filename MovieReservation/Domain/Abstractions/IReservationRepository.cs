using Domain.Entities;

namespace Domain.Abstractions;

public interface IReservationRepository
{
    Task<List<Reservation>> GetReservationsAsync(string userId);
}