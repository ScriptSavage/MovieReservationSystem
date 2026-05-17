using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public sealed class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.HasKey(x => x.ReservationId);
        
       
        builder.HasOne(e=>e.User)
            .WithMany(u=>u.Reservations)
            .HasForeignKey(e=>e.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        
        builder.HasMany(e=>e.Tickets)
            .WithOne(t => t.Reservation)
            .HasForeignKey(e=>e.ReservationId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(e=>e.Event)
            .WithMany(e => e.Reservations)
            .HasForeignKey(e=>e.EventId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(r => r.TotalPrice)
            .HasColumnType("decimal(18,2)");
    }
}