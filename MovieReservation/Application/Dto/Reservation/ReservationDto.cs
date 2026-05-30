using Application.Dto.Event;

namespace Application.Dto.Reservation;

public record ReservationDto(DateTime Created, Guid ReservationCode, Decimal TotalPrice);

public record ReservationDtoDetails(DateTime Created, Guid ReservationCode, Decimal TotalPrice,EventDto Event);