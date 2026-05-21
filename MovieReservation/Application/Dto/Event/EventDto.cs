using Application.Dto.Venue;

namespace Application.Dto.Event;

public record EventDto(DateTime StartDate, DateTime EndDate, VenueDto Venue);