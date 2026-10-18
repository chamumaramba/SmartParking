using SmartParking.Domain.Enums;

namespace SmartParking.Domain.Entities;

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
