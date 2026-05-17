using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.HasIndex(e=>e.Token)
            .IsUnique();
        
        builder.HasOne(e=>e.User)
            .WithMany(e => e.RefreshTokens)
            .HasForeignKey(e=>e.UserId)
            .OnDelete(DeleteBehavior.Cascade);
            
    }
}