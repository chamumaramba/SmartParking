using SmartParking.Domain.Entities;
using SmartParking.Domain.Enums;

namespace SmartParking.Domain.Tests;

public class AccessCodeTests
{
    private static readonly DateTimeOffset ValidFrom =
        new(2026, 10, 31, 12, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset ValidUntil = ValidFrom.AddHours(2);

    [Fact]
    public void Create_RejectsEmptyVerifier()
    {
        Assert.Throws<ArgumentException>(() =>
            AccessCode.Create(
                Guid.NewGuid(),
                " ",
                ValidFrom,
                ValidUntil,
                ValidFrom));
    }

    [Fact]
    public void Create_RejectsInvalidValidityPeriod()
    {
        Assert.Throws<ArgumentException>(() =>
            AccessCode.Create(
                Guid.NewGuid(),
                "verifier",
                ValidFrom,
                ValidFrom,
                ValidFrom));
    }

    [Fact]
    public void IsValidAt_IncludesValidityPeriodBoundaries()
    {
        var accessCode = CreateAccessCode();

        Assert.True(accessCode.IsValidAt(ValidFrom));
        Assert.True(accessCode.IsValidAt(ValidUntil));
    }

    [Fact]
    public void IsValidAt_ReturnsFalseOutsideValidityPeriod()
    {
        var accessCode = CreateAccessCode();

        Assert.False(accessCode.IsValidAt(ValidFrom.AddTicks(-1)));
        Assert.False(accessCode.IsValidAt(ValidUntil.AddTicks(1)));
    }

    [Fact]
    public void Revoke_MakesCodeInvalidAndRecordsRevocationTime()
    {
        var accessCode = CreateAccessCode();
        var revokedAt = ValidFrom.AddMinutes(30);

        accessCode.Revoke(revokedAt);

        Assert.Equal(AccessCodeStatus.Revoked, accessCode.Status);
        Assert.Equal(revokedAt, accessCode.RevokedAt);
        Assert.False(accessCode.IsValidAt(revokedAt));
    }

    [Fact]
    public void Expire_AfterValidityPeriodMakesCodeInvalid()
    {
        var accessCode = CreateAccessCode();

        accessCode.Expire(ValidUntil.AddTicks(1));

        Assert.Equal(AccessCodeStatus.Expired, accessCode.Status);
        Assert.False(accessCode.IsValidAt(ValidUntil));
    }

    [Fact]
    public void Expire_RejectsExpirationBeforeValidityPeriodEnds()
    {
        var accessCode = CreateAccessCode();

        Assert.Throws<InvalidOperationException>(() =>
            accessCode.Expire(ValidUntil));
    }

    private static AccessCode CreateAccessCode()
    {
        return AccessCode.Create(
            Guid.NewGuid(),
            "verifier",
            ValidFrom,
            ValidUntil,
            ValidFrom.AddMinutes(-5));
    }
}
