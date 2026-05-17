namespace Domain.Entities.Identity;

public class RefreshToken
{
    public Guid Id { get; set; }

    public string Token { get; set; } = null!;

    public DateTime ExpiresOut { get; set; }

    public DateTime? RevokedAt { get; set; }

    public string? ReplacedByTokenHash { get; set; }

    public string UserId { get; set; } = null!;

    public ApplicationUser User { get; set; } = null!;

    public bool IsActive => RevokedAt == null && ExpiresOut > DateTime.UtcNow;
}