using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(e=>e.FirstName)
            .HasMaxLength(250)
            .IsRequired();
        
        builder.Property(e=>e.LastName)
            .HasMaxLength(250)
            .IsRequired();
        
    }
}