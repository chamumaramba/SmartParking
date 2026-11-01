namespace SmartParking.Domain.Entities;

/// <summary>
/// Represents a managed parking facility. The application uses it to group
/// parking spaces, devices, and the pricing plans available at that location.
/// </summary>
public class ParkingLot
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string ParkingLotName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public ICollection<Device> Devices { get; set; } = [];
    public ICollection<ParkingSpace> ParkingSpaces { get; set; } = [];
    public ICollection<PricingPlan> PricingPlans { get; set; } = [];
}
