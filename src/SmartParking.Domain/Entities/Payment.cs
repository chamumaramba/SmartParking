using SmartParking.Domain.Enums;

namespace SmartParking.Domain.Entities;

public class Payment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid? ParkingSessionId { get; set; }
    public Guid? SubscriptionId { get; set; }
    public string? ProviderReference { get; set; }
    public decimal Amount { get; set; }
    public string CurrencyCode { get; set; } = "USD";
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? CompletedAt { get; set; }

    public ParkingSession? ParkingSession { get; set; }
    public Subscription? Subscription { get; set; }
}
