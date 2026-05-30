using Application.Dto.TicketType;

namespace Application.Dto.Event;

public record EventTicketTypesDto(string EventName,List<TicketTypeDto> TicketTypes);