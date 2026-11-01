using SmartParking.Domain.Enums;

namespace SmartParking.Domain.Entities;

/// <summary>
/// Records an access attempt reported by a parking device, including the action,
/// decision, time, and any associated booking or access code. The application uses
/// these records for access history, troubleshooting, and auditing.
/// </summary>
public class AccessEvent
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string RequestId { get; set; } = string.Empty;
    public Guid DeviceId { get; set; }
    public Guid? BookingId { get; set; }
    public Guid? AccessCodeId { get; set; }
    public AccessAction Action { get; set; }
    public AccessDecision Decision { get; set; }
    public DateTimeOffset OccurredAt { get; set; }
   //ublic string? ObservedRegistrationNumber { get; set; }
   //ublic decimal? RecognitionConfidence { get; set; }

    public Device Device { get; set; } = null!;
    public Booking? Booking { get; set; }
    public AccessCode? AccessCode { get; set; }
}
