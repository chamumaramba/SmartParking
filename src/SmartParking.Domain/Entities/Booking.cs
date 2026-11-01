using SmartParking.Domain.Enums;

namespace SmartParking.Domain.Entities;

/// <summary>
/// Represents a user's reservation of a parking space and its lifecycle. The
/// application uses it to track reservation payment and entry eligibility,
/// associate the vehicle and access codes, and connect the resulting parking session.
/// </summary>
public class Booking
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public Guid VehicleId { get; set; }
    public Guid ParkingSpaceId { get; set; }
    public DateTimeOffset ReservedFrom { get; set; }
    public DateTimeOffset ReservedUntil { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public BookingStatus Status { get; private set; } = BookingStatus.Pending;
    public DateTimeOffset? ReservationExpiresAt { get; private set; }

    public Vehicle Vehicle { get; set; } = null!;
    public ParkingSpace ParkingSpace { get; set; } = null!;
    public ICollection<AccessCode> AccessCodes { get; set; } = [];
    public ParkingSession? ParkingSession { get; set; }

    public void ConfirmReservation(DateTimeOffset confirmedAt, TimeSpan entryWindow)
    {
        if (Status != BookingStatus.Pending)
        {
            throw new InvalidOperationException(
                "Only pending bookings can be confirmed.");
        }

        if (entryWindow <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(
                nameof(entryWindow),
                "The entry window must be greater than zero.");
        }

        ReservationExpiresAt = confirmedAt.Add(entryWindow);
        Status = BookingStatus.Confirmed;
    }

    public void RecordEntry(DateTimeOffset enteredAt)
    {
        if (Status != BookingStatus.Confirmed)
        {
            throw new InvalidOperationException(
                "Only confirmed bookings can be used to enter.");
        }

        if (ReservationExpiresAt is null || enteredAt > ReservationExpiresAt)
        {
            throw new InvalidOperationException(
                "The reservation entry window has expired.");
        }

        Status = BookingStatus.Active;
    }

    public void ExpireReservation(DateTimeOffset now)
    {
        if (Status != BookingStatus.Confirmed)
        {
            throw new InvalidOperationException(
                "Only confirmed bookings can expire.");
        }

        if (ReservationExpiresAt is null || now <= ReservationExpiresAt)
        {
            throw new InvalidOperationException(
                "The reservation entry window has not passed.");
        }

        Status = BookingStatus.Expired;
        RevokeAccessCodes(now);
    }

    public void Cancel()
    {
        Cancel(DateTimeOffset.UtcNow);
    }

    public void Cancel(DateTimeOffset cancelledAt)
    {
        if (Status != BookingStatus.Confirmed)
        {
            throw new InvalidOperationException(
                "Only confirmed bookings can be cancelled.");
        }

        Status = BookingStatus.Cancelled;
        RevokeAccessCodes(cancelledAt);
    }

    public AccessCode IssueAccessCode(
        string codeVerifier,
        DateTimeOffset validFrom,
        DateTimeOffset validUntil,
        DateTimeOffset createdAt)
    {
        if (Status != BookingStatus.Confirmed)
        {
            throw new InvalidOperationException(
                "Access codes can only be issued for confirmed bookings.");
        }

        var accessCode = AccessCode.Create(
            Id,
            codeVerifier,
            validFrom,
            validUntil,
            createdAt);

        AccessCodes.Add(accessCode);
        return accessCode;
    }

    private void RevokeAccessCodes(DateTimeOffset revokedAt)
    {
        foreach (var accessCode in AccessCodes)
        {
            accessCode.Revoke(revokedAt);
        }
    }
}
