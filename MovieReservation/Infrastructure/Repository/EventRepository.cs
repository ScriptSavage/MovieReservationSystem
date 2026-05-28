using Domain.Abstractions;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class EventRepository : IEventRepository
{
    private readonly DatabaseContext _context;

    public EventRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Event>> GetEvents(int page, int pageSize)
    {
        return await _context.Events
            .Include(e=>e.Movies)
            .ThenInclude(e=>e.Genres)
            .Include(e=>e.Venue)
            .OrderBy(e=>e.StartDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task AddNewEvent(Event eventEntity)
    {
        await _context.Events.AddAsync(eventEntity);
    }

    public async Task<Event> GetEvent(long eventId)
    {
        return await _context.Events.FindAsync(eventId);
    }

    public async Task<bool> DoesEventExist(long eventId)
    {
        return await _context.Events.AnyAsync(e=>e.EventId == eventId);
    }

    public async Task<int> CountEvents()
    {
        return await _context.Events.CountAsync();
    }

    public async Task DeleteEvent(Event eventEntity)
    {
         _context.Events.Remove(eventEntity);
         await _context.SaveChangesAsync();
    }

    public async Task<Event> GetEventDetails(long eventId)
    {
        var eventEntity = await _context.Events
            .Include(e=>e.Venue)
            .Include(e=>e.Movies)
            .ThenInclude(e=>e.Genres)
            .FirstOrDefaultAsync(e=>e.EventId == eventId);
       
        return eventEntity;
    }
}