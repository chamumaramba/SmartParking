using SmartParking.Domain.Enums;

namespace SmartParking.Domain.Entities;

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
