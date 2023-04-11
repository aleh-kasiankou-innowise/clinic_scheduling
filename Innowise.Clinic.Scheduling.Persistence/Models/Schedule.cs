using System.Text.Json.Serialization;
using Innowise.Clinic.Shared.BaseClasses;

namespace Innowise.Clinic.Scheduling.Persistence.Models;

public class Schedule : IEntity
{
    public Guid ScheduleId { get; set; }
    public Guid DoctorId { get; set; }
    [JsonIgnore] public virtual Doctor Doctor { get; set; }
    public DateTime Day { get; set; }
    public Guid? ShiftId { get; set; }
    [JsonIgnore] public virtual Shift? Shift { get; set; }
}