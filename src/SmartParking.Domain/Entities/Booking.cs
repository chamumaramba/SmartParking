using SmartParking.Domain.Enums;

namespace SmartParking.Domain.Entities;

public class Booking
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public Guid VehicleId { get; set; }
    public Guid ParkingSpaceId { get; set; }
    public DateTimeOffset ReservedFrom { get; set; }
    public DateTimeOffset ReservedUntil { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Pending;

    public Vehicle Vehicle { get; set; } = null!;
    public ParkingSpace ParkingSpace { get; set; } = null!;
    public ICollection<AccessCode> AccessCodes { get; set; } = [];
    public ParkingSession? ParkingSession { get; set; }
}
