using Application.Dto;
using Application.Dto.Event;
using Application.Dto.TicketType;

namespace Application.Abstraction;

public interface IEventService
{
    Task AddNewEventVenueAsync(NewEventDto dto);
    
    Task AddMovieToEventAsync(long eventId, long movieId);
    
    Task<PagedResult<EventDetailsDto>> GetAllEventsAsync(int page, int pageSize);
    
    Task<EventDetailsDto> GetEventDetailsAsync(long eventId);
    
    Task DeleteEventAsync(long eventId);
    Task UpdateEventDetailsAsync(long eventId, EventDto dto);
    
    Task AddTicketTypesToEventAsync(long eventId, TicketTypeDto dto);
    
    Task<EventTicketTypesDto> GetEventTicketTypesAsync(long eventId);
    
    Task DeleteTicketTypeAsync(long eventId, long ticketTypeId);
}