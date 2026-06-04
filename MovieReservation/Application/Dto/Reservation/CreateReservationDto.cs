namespace Application.Dto.Reservation;

public class CreateReservationDto
{
    public long EventId { get; set; }
    public List<CreateReservationTicketDto> Tickets { get; set; } = [];
}

public sealed class CreateReservationTicketDto
{
    public long TicketTypeId { get; set; }
    public int Quantity { get; set; }
}