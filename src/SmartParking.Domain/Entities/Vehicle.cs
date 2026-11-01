namespace SmartParking.Domain.Entities;

/// <summary>
/// Represents a vehicle registered to a user. The application uses it to
/// associate bookings with the vehicle expected to use the parking facility.
/// </summary>
public class Vehicle
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string RegistrationNumber { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public ICollection<Booking> Bookings { get; set; } = [];
}
