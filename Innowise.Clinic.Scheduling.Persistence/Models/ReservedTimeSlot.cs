namespace Innowise.Clinic.Scheduling.Persistence.Models;

public class ReservedTimeSlot
{
    public Guid ReservedTimeSlotId { get; set; }
    public Guid AppointmentId { get; set; }
    public Guid DoctorId { get; set; }
    public DateTime AppointmentStart { get; set; }
    public DateTime AppointmentFinish { get; set; }
}