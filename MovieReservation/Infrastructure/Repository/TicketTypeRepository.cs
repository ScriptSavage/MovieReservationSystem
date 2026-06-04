using Domain.Abstractions;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class TicketTypeRepository :  ITicketTypeRepository
{
    private readonly DatabaseContext _context;

    public TicketTypeRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<TicketType> GetTicketType(long ticketTypeId) => 
        _context.TicketTypes.FirstOrDefault(t => t.TicketTypeId == ticketTypeId);
    

    public async Task<bool> DoesTicketTypeExist(long ticketTypeId) => 
        await _context.TicketTypes.AnyAsync(e=>e.TicketTypeId == ticketTypeId);

    public async Task<int> CountActiveTicketsByTicketTypeIdAsync(long ticketTypeId)
    {
        return await _context.Tickets.CountAsync(t =>
                t.TicketTypeId == ticketTypeId &&
                t.Reservation.CancelledAt == null);
    }
}