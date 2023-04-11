using Innowise.Clinic.Shared.BaseClasses;

namespace Innowise.Clinic.Scheduling.Persistence.Models;

public class ShiftPreference : IEntity
{
    public Guid ShiftPreferenceId { get; set; }
    public Guid DoctorId { get; set; }
    public virtual Doctor Doctor { get; set; }
    public Guid ShiftId { get; set; }
    public virtual Shift Shift { get; set; }
}