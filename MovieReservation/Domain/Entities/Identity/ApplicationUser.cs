using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Identity;

public sealed class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } =  null!;
    public string LastName { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    public ICollection<Reservations> Reservations { get; set; } = [];
}