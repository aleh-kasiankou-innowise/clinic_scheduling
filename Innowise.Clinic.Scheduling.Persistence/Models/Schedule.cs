using System.Text.Json.Serialization;

namespace Innowise.Clinic.Scheduling.Persistence.Models;

public class Schedule
{
    public Guid ScheduleId { get; set; }
    public Guid SpecializationId { get; set; }
    public Guid OfficeId { get; set; }
    public Guid DoctorId { get; set; }
    public DateTime Day { get; set; }
    public Guid? ShiftId { get; set; }
    [JsonIgnore] public virtual Shift? Shift { get; set; }
}