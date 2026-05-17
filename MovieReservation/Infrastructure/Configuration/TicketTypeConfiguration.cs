using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public sealed class TicketTypeConfiguration : IEntityTypeConfiguration<TicketType>
{
    public void Configure(EntityTypeBuilder<TicketType> builder)
    {
        builder.HasKey(e => e.TickerTypeId);
        
        builder.Property(e=>e.Name)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(e => e.Capacity)
            .IsRequired();
        
        builder.Property(e=>e.Price)
            .HasColumnType("decimal(18,2)")
            .IsRequired();
        
       
        builder.HasOne(e=>e.Event)
            .WithMany(e=>e.TicketTypes)
            .HasForeignKey(e=>e.EventId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(e=>e.Tickets)
            .WithOne(e=>e.TicketType)
            .HasForeignKey(e=>e.TicketTypeId)
            .OnDelete(DeleteBehavior.NoAction);
            
    }
}