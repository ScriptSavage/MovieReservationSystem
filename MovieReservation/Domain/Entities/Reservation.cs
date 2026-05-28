using Domain.Entities.Identity;

namespace Domain.Entities;

public sealed class Reservation
{
    public long ReservationId { get; set; }
    
    // User navigation
    public string UserId { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;
    
    
    // Event navigation
    public long EventId { get; set; }
    public Event Event { get; set; } = null!;


    public ICollection<Ticket> Tickets { get; set; } = [];
    
    public Guid ReservationCode { get; set; } =  Guid.NewGuid();

    public DateTime CreatedAt { get; set; } =  DateTime.UtcNow;

    public DateTime? ConfirmedAt { get; set; }

    public Decimal TotalPrice { get; set; }

    public DateTime? CancelledAt { get; set; }
    
}