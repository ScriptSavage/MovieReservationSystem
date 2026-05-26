using Application.Dto.Venue;

namespace Application.Dto.Event;

public record EventDto(string EventName,DateTime StartDate, DateTime EndDate, VenueDto Venue);