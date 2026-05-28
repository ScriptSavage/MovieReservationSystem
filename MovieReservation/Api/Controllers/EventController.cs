using Application.Abstraction;
using Application.Dto.Event;
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


    [HttpDelete("{eventId:long}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteEvent(long eventId)
    {
        await _eventService.DeleteEventAsync(eventId);
        return Ok(new {message = "Event was deleted successfully" });
    }


    [HttpPut("{eventId:long}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateEvent(long eventId, [FromBody] EventDto dto)
    {
        await _eventService.UpdateEventDetailsAsync(eventId, dto);
        return Ok(new { message = "Event was updated successfully" });
    }
}