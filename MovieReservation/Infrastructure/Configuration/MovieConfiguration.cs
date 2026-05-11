using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public sealed class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {

        builder.HasKey(e => e.MovieId);

        
        builder.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(255);
        
        builder.Property(e=>e.DurationInMinutes)
            .IsRequired();

        builder.Property(e => e.ReleaseYear)
            .IsRequired();
        
        builder.Property(e=>e.MinimumAgeRecruitment)
            .IsRequired();
        
        




    }
}