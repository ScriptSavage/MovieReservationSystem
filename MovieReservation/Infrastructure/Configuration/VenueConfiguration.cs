using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public sealed class VenueConfiguration : IEntityTypeConfiguration<Venue>
{
    public void Configure(EntityTypeBuilder<Venue> builder)
    {
        builder.HasKey(e => e.VenueId);
        
        builder.Property(e => e.Name)
            .HasMaxLength(250)
            .IsRequired();
        
        builder.Property(e=>e.Street)
            .HasMaxLength(250)
            .IsRequired();
        
        builder.Property(e => e.PostalCode)
            .HasMaxLength(250)
            .IsRequired();
        
        builder.Property(e=>e.City)
            .HasMaxLength(250)
            .IsRequired();
        
    }
}