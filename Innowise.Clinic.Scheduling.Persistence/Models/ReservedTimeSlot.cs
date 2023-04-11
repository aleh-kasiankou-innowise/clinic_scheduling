using Innowise.Clinic.Shared.BaseClasses;

namespace Innowise.Clinic.Scheduling.Persistence.Models;

public class ReservedTimeSlot : IEntity
{
    public Guid ReservedTimeSlotId { get; set; }
    public Guid DoctorId { get; set; }
    public virtual Doctor Doctor { get; set; }
    public DateTime AppointmentStart { get; set; }
    public DateTime AppointmentFinish { get; set; }
}