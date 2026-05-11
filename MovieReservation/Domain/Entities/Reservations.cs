using Domain.Entities.Identity;

namespace Domain.Entities;

public sealed class Reservations
{
    public long ReservationId { get; set; }
    
    // User navigation
    public long UserId { get; set; }
    public ApplicationUser User { get; set; } =  null!;
    
    
    // Event navigation
    public long EventId { get; set; }
    public Event Event { get; set; } = null!;

    public Guid ReservationCode { get; set; } =  Guid.NewGuid();

    public DateTime CreatedAt { get; set; } =  DateTime.UtcNow;

    public DateTime? ConfirmedAt { get; set; }

    public Decimal TotalPrice { get; set; }

    public DateTime? CancelledAt { get; set; }
    
}