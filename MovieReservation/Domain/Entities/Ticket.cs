namespace Domain.Entities;

public sealed class Ticket
{
    public long TicketId { get; set; }

    public Reservation Reservation { get; set; } = null!;
    public long ReservationId { get; set; }

    public Guid TicketCode { get; set; } = Guid.NewGuid();

    public TicketType TicketType { get; set; } = null!;
    public long TicketTypeId { get; set; }
}