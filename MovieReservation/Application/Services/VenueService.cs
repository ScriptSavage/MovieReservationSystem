using Application.Abstraction;
using Application.Dto.Venue;
using Application.Exceptions;
using Domain.Abstractions;
using Domain.Entities;

namespace Application.Services;

public class VenueService : IVenueService
{
    private readonly IVenueRepository _venueRepository;

    public VenueService(IVenueRepository venueRepository)
    {
        _venueRepository = venueRepository;
    }

    public async Task<List<VenueDto>> GetAllVenues()
    {
        var data = await _venueRepository.GetAllVenues();

        return  data.Select(e =>
                new VenueDto(e.Name, e.City, e.PostalCode, e.Street))
            .ToList();
        
    }

    public async Task<VenueDto> GetVenue(long id)
    {
        var doesVenueExist = await _venueRepository.DoesVenueExist(id);
        if (!doesVenueExist)
        {
            throw new DoesNotExistsException("Venue not found");
        }

        var venue = await _venueRepository.GetVenue(id);
        
        return new VenueDto(venue.Name, venue.City, venue.PostalCode, venue.Street);
    }

    public async Task UpdateVenue(long id, VenueDto dto)
    {
        var doesVenueExist = await _venueRepository.DoesVenueExist(id);

        if (!doesVenueExist)
        {
            throw new DoesNotExistsException("Venue not found");
        }

        var venue = await _venueRepository.GetVenue(id);
        
        if(string.IsNullOrWhiteSpace(dto.Name)) venue.Name = dto.Name;
        if(string.IsNullOrWhiteSpace(dto.City)) venue.City = dto.City;
        if(string.IsNullOrWhiteSpace(dto.PostalCode)) venue.PostalCode = dto.PostalCode;
        if(string.IsNullOrWhiteSpace(dto.Street)) venue.Street = dto.Street;
        
    }
}