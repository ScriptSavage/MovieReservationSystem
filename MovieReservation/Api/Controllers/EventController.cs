using Application.Abstraction;
using Application.Dto.Event;
using Application.Dto.TicketType;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/events")]
public class EventController : ControllerBase
{
    private readonly IEventService _eventService;

    public EventController(IEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpGet]
    public async Task<IActionResult> GetEvents([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _eventService.GetAllEventsAsync(page, pageSize);
        return Ok(result);
    }

    
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetEventDetails(long id)
    {
        var eventDetails = await _eventService.GetEventDetailsAsync(id);
        return Ok(eventDetails);
    }

    [HttpGet("{eventId:long}/tickets-types")]
    public async Task<IActionResult> GetEventTicketTypes(long eventId)
    {
        var result = await _eventService.GetEventTicketTypesAsync(eventId);
        return Ok(result);
    }



    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddNewEventAsync([FromBody] NewEventDto dto)
    {
        await _eventService.AddNewEventVenueAsync(dto);
        return Ok(new { message = "Event was added successfully" });
    }

    [HttpPost("{eventId:long}/movies/{movieId:long}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddNewMovieToEventAsync(long eventId, long movieId)
    {
        await _eventService.AddMovieToEventAsync(eventId, movieId);
        return Ok(new { message = "Movie was added successfully" });
    }

    [HttpPost("{eventId:long}/ticket-types")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddNewTicketTypeToEventAsync(long eventId,[FromBody] TicketTypeDto dto)
    {
        await _eventService.AddTicketTypesToEventAsync(eventId, dto);
        return Ok(new  { message = "Ticket type was added successfully" });
    }



    [HttpDelete("{eventId:long}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteEvent(long eventId)
    {
        await _eventService.DeleteEventAsync(eventId);
        return Ok(new {message = "Event was deleted successfully" });
    }

    
    [HttpDelete("{eventId:long}/ticket-types/{ticketTypeId:long}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteEventTicketTypes(long eventId, long ticketTypeId)
    {
        await _eventService.DeleteTicketTypeAsync(eventId, ticketTypeId);
        return Ok(new { message = "Ticket type was deleted successfully" });
    }


    [HttpPut("{eventId:long}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateEvent(long eventId, [FromBody] EventDto dto)
    {
        await _eventService.UpdateEventDetailsAsync(eventId, dto);
        return Ok(new { message = "Event was updated successfully" });
    }
}