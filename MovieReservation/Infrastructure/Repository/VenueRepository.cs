using Domain.Abstractions;
using Domain.Entities;
using Infrastructure.Context;

namespace Infrastructure.Repository;

public class VenueRepository : IVenueRepository
{
    private readonly DatabaseContext _context;

    public VenueRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task AddNewVenue(Venue venue)
    {
        await _context.Venues.AddAsync(venue);
        await _context.SaveChangesAsync();
    }
}