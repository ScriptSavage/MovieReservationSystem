using Domain.Entities;

namespace Domain.Abstractions;

public interface IVenueRepository
{
    Task AddNewVenue(Venue venue);
    
    Task<Venue> GetVenue(long id);
    
    Task<IEnumerable<Venue>> GetAllVenues();
    
    Task <bool> DoesVenueExist(long id);
}