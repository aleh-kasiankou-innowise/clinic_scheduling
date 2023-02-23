namespace Innowise.Clinic.Scheduling.Persistence.Models;

public class ShiftPreference
{
    public Guid ShiftPreferenceId { get; set; }
    public Guid DoctorId { get; set; }
    public Guid ShiftId { get; set; }
    public virtual Shift Shift { get; set; }
}