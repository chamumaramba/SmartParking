using SmartParking.Domain.Enums;

namespace SmartParking.Domain.Entities;

/// <summary>
/// Represents a vehicle's actual stay in a parking space, from recorded entry
/// until exit. The application uses its timestamps and pricing snapshots to
/// calculate the parking charge and associate related payments.
/// </summary>
public class ParkingSession
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid BookingId { get; set; }
    public DateTimeOffset EntryTime { get; set; }
    public DateTimeOffset? ExitTime { get; set; }
    public ParkingSessionStatus Status { get; set; } = ParkingSessionStatus.Active;
    public decimal? CalculatedAmount { get; set; }
    public string? CurrencyCode { get; set; }
    public string? PricingPlanNameSnapshot { get; set; }
    public decimal? HourlyRateSnapshot { get; set; }
    public decimal? MonthlyPriceSnapshot { get; set; }
    public decimal? MaximumDailyChargeSnapshot { get; set; }
    public int? GracePeriodMinutesSnapshot { get; set; }

    public Booking Booking { get; set; } = null!;
    public ICollection<Payment> Payments { get; set; } = [];
}
