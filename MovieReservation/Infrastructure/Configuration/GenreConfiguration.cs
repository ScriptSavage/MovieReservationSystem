using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.HasKey(e => e.GenreId);
        
        builder.Property(e=>e.GenreName)
            .HasMaxLength(250)
            .IsRequired();
        
        builder.HasIndex(e=>e.GenreName)
            .IsUnique();
    }
}