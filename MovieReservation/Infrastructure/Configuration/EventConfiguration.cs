using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public sealed class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasKey(e => e.EventId);
        
        builder.Property(e=>e.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.TotalCapacity)
            .IsRequired();

        builder.Property(e => e.StartDate)
            .IsRequired();
        
        builder.Property(e => e.EndDate)
            .IsRequired();
        
        builder.HasOne(e=>e.Venue)
            .WithMany(e => e.Events)
            .HasForeignKey(e=>e.VenueId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}