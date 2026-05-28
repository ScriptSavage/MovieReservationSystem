using Application.Dto.Venue;
using Domain.Entities;

namespace Application.Abstraction;

public interface IVenueService 
{
    Task<List<VenueDto>> GetAllVenues();
    
    Task<VenueDto> GetVenue(long id);
    
    Task UpdateVenue(long id, VenueDto dto);
    
}