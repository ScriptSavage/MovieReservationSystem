namespace Domain.Entities;

public sealed class TicketType
{
    public long TicketTypeId { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public int Capacity { get; set; }

    public Event Event { get; set; } = null!;
    public long EventId { get; set; }

    public ICollection<Ticket> Tickets { get; set; } = [];
}