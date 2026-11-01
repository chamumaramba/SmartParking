using SmartParking.Domain.Enums;

namespace SmartParking.Domain.Entities;

/// <summary>
/// Represents a credential associated with a booking. The application uses it
/// to validate access attempts during its validity period and to revoke access
/// when the booking or credential is no longer valid.
/// </summary>
public class AccessCode
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid BookingId { get; private set; }
    public string CodeVerifier { get; private set; } = string.Empty;
    public DateTimeOffset ValidFrom { get; private set; }
    public DateTimeOffset ValidUntil { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? RevokedAt { get; private set; }
    public AccessCodeStatus Status { get; private set; } = AccessCodeStatus.Active;

    public Booking Booking { get; set; } = null!;

    public static AccessCode Create(
        Guid bookingId,
        string codeVerifier,
        DateTimeOffset validFrom,
        DateTimeOffset validUntil,
        DateTimeOffset createdAt)
    {
        if (bookingId == Guid.Empty)
        {
            throw new ArgumentException("A booking ID is required.", nameof(bookingId));
        }

        if (string.IsNullOrWhiteSpace(codeVerifier))
        {
            throw new ArgumentException("A code verifier is required.", nameof(codeVerifier));
        }

        if (validUntil <= validFrom)
        {
            throw new ArgumentException(
                "The code must expire after it becomes valid.",
                nameof(validUntil));
        }

        return new AccessCode
        {
            BookingId = bookingId,
            CodeVerifier = codeVerifier,
            ValidFrom = validFrom,
            ValidUntil = validUntil,
            CreatedAt = createdAt
        };
    }

    public bool IsValidAt(DateTimeOffset time)
    {
        return Status == AccessCodeStatus.Active
            && time >= ValidFrom
            && time <= ValidUntil;
    }

    public void Revoke(DateTimeOffset revokedAt)
    {
        if (Status != AccessCodeStatus.Active)
        {
            return;
        }

        Status = AccessCodeStatus.Revoked;
        RevokedAt = revokedAt;
    }

    public void Expire(DateTimeOffset now)
    {
        if (Status != AccessCodeStatus.Active)
        {
            return;
        }

        if (now <= ValidUntil)
        {
            throw new InvalidOperationException(
                "An access code cannot expire before its validity period ends.");
        }

        Status = AccessCodeStatus.Expired;
    }
}
