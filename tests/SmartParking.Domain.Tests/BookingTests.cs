using SmartParking.Domain.Entities;
using SmartParking.Domain.Enums;

namespace SmartParking.Domain.Tests;

public class BookingTests
{
    [Fact]
    public void ConfirmReservation_SetsEntryDeadline()
    {
        var confirmedAt = new DateTimeOffset(2026, 10, 24, 12, 0, 0, TimeSpan.Zero);
        var booking = new Booking();

        booking.ConfirmReservation(confirmedAt, TimeSpan.FromMinutes(15));

        Assert.Equal(BookingStatus.Confirmed, booking.Status);
        Assert.Equal(confirmedAt.AddMinutes(15), booking.ReservationExpiresAt);
    }

    [Fact]
    public void ConfirmReservation_RejectsNonPositiveEntryWindow()
    {
        var booking = new Booking();

        Assert.Throws<ArgumentOutOfRangeException>(
            () => booking.ConfirmReservation(DateTimeOffset.UtcNow, TimeSpan.Zero));
    }

    [Fact]
    public void RecordEntry_ActivatesBookingAtDeadline()
    {
        var confirmedAt = new DateTimeOffset(2026, 10, 24, 12, 0, 0, TimeSpan.Zero);
        var booking = new Booking();
        booking.ConfirmReservation(confirmedAt, TimeSpan.FromMinutes(15));

        booking.RecordEntry(confirmedAt.AddMinutes(15));

        Assert.Equal(BookingStatus.Active, booking.Status);
    }

    [Fact]
    public void RecordEntry_RejectsEntryAfterDeadline()
    {
        var confirmedAt = new DateTimeOffset(2026, 10, 24, 12, 0, 0, TimeSpan.Zero);
        var booking = new Booking();
        booking.ConfirmReservation(confirmedAt, TimeSpan.FromMinutes(15));

        Assert.Throws<InvalidOperationException>(
            () => booking.RecordEntry(confirmedAt.AddMinutes(16)));
    }

    [Fact]
    public void ExpireReservation_ExpiresConfirmedBookingAfterDeadline()
    {
        var confirmedAt = new DateTimeOffset(2026, 10, 24, 12, 0, 0, TimeSpan.Zero);
        var booking = new Booking();
        booking.ConfirmReservation(confirmedAt, TimeSpan.FromMinutes(15));

        booking.ExpireReservation(confirmedAt.AddMinutes(15).AddTicks(1));

        Assert.Equal(BookingStatus.Expired, booking.Status);
    }

    [Fact]
    public void ExpireReservation_RejectsExpirationAtDeadline()
    {
        var confirmedAt = new DateTimeOffset(2026, 10, 24, 12, 0, 0, TimeSpan.Zero);
        var booking = new Booking();
        booking.ConfirmReservation(confirmedAt, TimeSpan.FromMinutes(15));

        Assert.Throws<InvalidOperationException>(
            () => booking.ExpireReservation(confirmedAt.AddMinutes(15)));
    }

    [Fact]
    public void RecordEntry_RejectsDuplicateEntry()
    {
        var confirmedAt = new DateTimeOffset(2026, 10, 24, 12, 0, 0, TimeSpan.Zero);
        var booking = new Booking();
        booking.ConfirmReservation(confirmedAt, TimeSpan.FromMinutes(15));
        booking.RecordEntry(confirmedAt.AddMinutes(1));

        Assert.Throws<InvalidOperationException>(
            () => booking.RecordEntry(confirmedAt.AddMinutes(2)));
    }

    [Fact]
    public void ExpireReservation_RejectsBookingAfterSuccessfulEntry()
    {
        var confirmedAt = new DateTimeOffset(2026, 10, 24, 12, 0, 0, TimeSpan.Zero);
        var booking = new Booking();
        booking.ConfirmReservation(confirmedAt, TimeSpan.FromMinutes(15));
        booking.RecordEntry(confirmedAt.AddMinutes(1));

        Assert.Throws<InvalidOperationException>(
            () => booking.ExpireReservation(confirmedAt.AddHours(1)));
    }

    [Fact]
    public void IssueAccessCode_RequiresConfirmedBooking()
    {
        var booking = new Booking();

        Assert.Throws<InvalidOperationException>(() =>
            booking.IssueAccessCode(
                "verifier",
                DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow.AddHours(1),
                DateTimeOffset.UtcNow));
    }

    [Fact]
    public void Cancel_RevokesAccessCodes()
    {
        var confirmedAt = new DateTimeOffset(2026, 10, 24, 12, 0, 0, TimeSpan.Zero);
        var booking = new Booking();
        booking.ConfirmReservation(confirmedAt, TimeSpan.FromMinutes(15));
        var accessCode = booking.IssueAccessCode(
            "verifier",
            confirmedAt,
            confirmedAt.AddHours(2),
            confirmedAt);

        booking.Cancel(confirmedAt.AddMinutes(1));

        Assert.Equal(AccessCodeStatus.Revoked, accessCode.Status);
        Assert.Equal(confirmedAt.AddMinutes(1), accessCode.RevokedAt);
    }

    [Fact]
    public void ExpireReservation_RevokesAccessCodes()
    {
        var confirmedAt = new DateTimeOffset(2026, 10, 24, 12, 0, 0, TimeSpan.Zero);
        var booking = new Booking();
        booking.ConfirmReservation(confirmedAt, TimeSpan.FromMinutes(15));
        var accessCode = booking.IssueAccessCode(
            "verifier",
            confirmedAt,
            confirmedAt.AddHours(2),
            confirmedAt);
        var expiredAt = confirmedAt.AddMinutes(16);

        booking.ExpireReservation(expiredAt);

        Assert.Equal(AccessCodeStatus.Revoked, accessCode.Status);
        Assert.Equal(expiredAt, accessCode.RevokedAt);
    }
}
