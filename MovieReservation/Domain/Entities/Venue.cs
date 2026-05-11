namespace Domain.Entities;

public sealed class Venue
{
    public long VenueId { get; set; }

    public string? Name { get; set; }

    public string City { get; set; } = null!;
    public string PostalCode { get; set; } =  null!;
    public string Street { get; set; } = null!;
}