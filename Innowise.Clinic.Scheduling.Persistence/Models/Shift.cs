namespace Innowise.Clinic.Scheduling.Persistence.Models;

public class Shift
{
    public Guid ShiftId { get; set; }
    public string ShiftName { get; set; }
    public TimeOnly ShiftStart { get; set; }
    public TimeOnly ShiftEnd { get; set; }
    public TimeOnly LunchStart { get; set; } // lunch duration is set in configuration
}