using SmartParking.Domain.Enums;

namespace SmartParking.Domain.Entities;

public class AccessCode
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid BookingId { get; set; }
    public string CodeVerifier { get; set; } = string.Empty;
    public DateTimeOffset ValidFrom { get; set; }
    public DateTimeOffset ValidUntil { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? RevokedAt { get; set; }
    public AccessCodeStatus Status { get; set; } = AccessCodeStatus.Active;

    public Booking Booking { get; set; } = null!;
}
