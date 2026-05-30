namespace Application.Dto.TicketType;

public record TicketTypeDto(
    string Name,
    decimal Price,
    int Capacity);