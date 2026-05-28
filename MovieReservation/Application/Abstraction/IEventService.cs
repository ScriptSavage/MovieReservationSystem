using Application.Dto;
using Application.Dto.Event;
using Domain.Entities;

namespace Application.Abstraction;

public interface IEventService
{
    Task AddNewEventVenueAsync(NewEventDto dto);
    
    Task AddMovieToEventAsync(long eventId, long movieId);
    
    Task<PagedResult<EventDetailsDto>> GetAllEventsAsync(int page, int pageSize);
    
    Task<EventDetailsDto> GetEventDetailsAsync(long eventId);
    
    Task DeleteEventAsync(long eventId);
    Task UpdateEventDetailsAsync(long eventId, EventDto dto);
}