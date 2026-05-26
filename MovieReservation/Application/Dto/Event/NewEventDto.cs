using Application.Dto.Venue;

namespace Application.Dto.Event;

public record NewEventDto(string Name,
    int TotalCapacity,
    DateTime StartDate,
    DateTime EndDate,
    VenueDto Venue);