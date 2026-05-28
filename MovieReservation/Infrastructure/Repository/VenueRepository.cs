using Domain.Abstractions;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

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
    }

    

    public async Task<Venue> GetVenue(long id)
    {
        return  await _context.Venues.FindAsync(id);
    }

    public async Task<IEnumerable<Venue>> GetAllVenues()
    {
        return await _context.Venues.ToListAsync();
    }

    public async Task<bool> DoesVenueExist(long id)
    {
        return await _context.Venues.AnyAsync(e=>e.VenueId == id);
    }
}