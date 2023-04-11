using Innowise.Clinic.Shared.BaseClasses;

namespace Innowise.Clinic.Scheduling.Persistence.Models;

public class Shift : IEntity
{
    public Guid ShiftId { get; set; }
    public string ShiftName { get; set; }
    public TimeSpan ShiftStart { get; set; }
    public TimeSpan LunchStart { get; set; }
    public TimeSpan WorkingHours { get; set; } = new(8,0,0);
}