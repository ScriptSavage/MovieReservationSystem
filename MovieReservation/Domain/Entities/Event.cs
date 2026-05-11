namespace Domain.Entities;

public sealed class Event
{
    public long EventId { get; set; }

    public string Name { get; set; } = null!;
    public int TotalCapacity { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public ICollection<Movie> Movies { get; set; } = [];

}