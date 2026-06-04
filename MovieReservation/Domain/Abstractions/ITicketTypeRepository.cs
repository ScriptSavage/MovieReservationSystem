using Domain.Entities;

namespace Domain.Abstractions;

public interface ITicketTypeRepository
{
    Task<TicketType> GetTicketType(long eventId);
    
    Task<bool> DoesTicketTypeExist(long eventId);
    
    Task<int> CountActiveTicketsByTicketTypeIdAsync(long ticketTypeId);
}