using SmartParking.Domain.Enums;

namespace SmartParking.Domain.Entities;

/// <summary>
/// Represents a physical device installed at a parking lot, such as an access
/// gate or occupancy sensor. The application uses it to identify the source of
/// device reports and track device status and last contact time.
/// </summary>
public class Device
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ParkingLotId { get; set; }
    public string DeviceName { get; set; } = string.Empty;
    public string DeviceIdentifier { get; set; } = string.Empty;
    public ParkingDeviceType Type { get; set; }
    public DeviceStatus Status { get; set; } = DeviceStatus.Offline;
    public DateTimeOffset? LastSeenAt { get; set; }

    public ParkingLot ParkingLot { get; set; } = null!;
    public ICollection<AccessEvent> AccessEvents { get; set; } = [];
}
