using SmartParking.Domain.Enums;

namespace SmartParking.Domain.Entities;

public class Subscription
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public Guid PricingPlanId { get; set; }
    public DateTimeOffset StartsAt { get; set; }
    public DateTimeOffset EndsAt { get; set; }
    public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Pending;
    public string? PaymentGatewaySubscriptionReference { get; set; }

    public PricingPlan PricingPlan { get; set; } = null!;
    public ICollection<Payment> Payments { get; set; } = [];
}
