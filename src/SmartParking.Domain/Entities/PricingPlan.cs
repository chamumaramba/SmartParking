using SmartParking.Domain.Enums;

namespace SmartParking.Domain.Entities;

public class PricingPlan
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public PricingPlanType Type { get; set; }
    public bool IsActive { get; set; } = true;
    public bool RequiresReservation { get; set; } = true;
    public string CurrencyCode { get; set; } = "USD";
    public decimal? HourlyRate { get; set; }
    public decimal? MonthlyPrice { get; set; }
    public decimal? MaximumDailyCharge { get; set; }
    public int GracePeriodMinutes { get; set; }

    public ICollection<ParkingLot> CoveredParkingLots { get; set; } = [];
}
