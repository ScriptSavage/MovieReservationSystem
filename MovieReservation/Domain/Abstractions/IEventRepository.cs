using Domain.Entities;

namespace Domain.Abstractions;

public interface IEventRepository
{
    
    Task <IEnumerable<Event>> GetEvents(int pagr, int pageNumber);
    
    Task AddNewEvent(Event eventEntity);
    
    Task<Event> GetEvent(long eventId);
    
    Task<bool> DoesEventExist(long eventId);
    
    Task<int> CountEvents();
    
    Task DeleteEvent(Event eventEntity);
    
    Task<Event> GetEventDetails(long eventId);
    
    Task<Event> GetEventTicketTypes(long eventId);
}