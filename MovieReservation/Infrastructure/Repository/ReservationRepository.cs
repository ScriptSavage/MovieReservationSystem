using Domain.Abstractions;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class ReservationRepository : IReservationRepository
{
    private readonly DatabaseContext _context;

    public ReservationRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<List<Reservation>> GetUserReservationsAsync(string userId)
    {
        var data = await _context.Reservations
            .Include(e=>e.Event)
            .ThenInclude(e=>e.Venue)
            .Where(e=>e.UserId == userId)
            .AsNoTracking()
            .ToListAsync();
        
        return data;
    }

    public async Task<ICollection<Reservation>> GetAllReservationsAsync(int page, int pageSize)
    {
        return await _context.Reservations
            .Include(e=>e.User)
            .Include(e=>e.Event)
            .OrderBy(x=>x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Reservation> GetReservationAsync(long id)
    {
        var reservation = await _context.Reservations
            .Include(e=>e.User)
            .FirstOrDefaultAsync(e => e.ReservationId == id);

        return reservation;
    }
    
    public async Task<bool> DoesReservationExistAsync(long id)=> await _context.Reservations.AnyAsync(e=>e.ReservationId == id);
}