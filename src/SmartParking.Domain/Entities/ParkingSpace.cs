namespace SmartParking.Domain.Entities;

public class ParkingSpace
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ParkingLotId { get; set; }
    public int ParkingFloorNumber { get; set; }
    public string ParkingSlotName { get; set; } = string.Empty;
    public bool IsOccupied { get; set; }
    public DateTimeOffset? LastOccupancyReportedAt { get; set; }

    public ParkingLot ParkingLot { get; set; } = null!;
    public ICollection<Booking> Bookings { get; set; } = [];
}
