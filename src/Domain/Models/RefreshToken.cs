namespace JuliGastos.Domain.Models;

public class RefreshToken
{
    public long Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public long UserId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }
    public bool IsRevoked { get; set; }
    public DateTimeOffset? RevokedAt { get; set; }
    public string IpAddress { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
    public string? ReplacedByToken { get; set; }
}
