using Domain.Entities;
using Domain.Entities.Identity;
using Infrastructure.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public class DatabaseContext : IdentityDbContext<ApplicationUser>
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }
    

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Venue> Venues { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<RefreshToken?> RefreshTokens { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new MovieConfiguration());
        modelBuilder.ApplyConfiguration(new GenreConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new VenueConfiguration());
        modelBuilder.ApplyConfiguration(new EventConfiguration());
        modelBuilder.ApplyConfiguration(new ReservationConfiguration());
        modelBuilder.ApplyConfiguration(new TicketConfiguration());
        modelBuilder.ApplyConfiguration(new TicketTypeConfiguration());
        modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}