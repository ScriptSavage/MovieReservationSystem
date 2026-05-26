using Domain.Entities;

namespace Domain.Abstractions;

public interface IVenueRepository
{
    Task AddNewVenue(Venue venue);
}