namespace Innowise.Clinic.Scheduling.Services.Dto;

/// <summary>
/// 
/// </summary>
/// <param name="ShiftName">Publicly visible shift title.</param>
/// <param name="ShiftStart">Time when the shift starts.</param>
/// <param name="WorkingHours">Number of working hours during shift.</param>>
/// <param name="LunchStart">Time when the lunch break starts.</param>
public record ShiftDto(string ShiftName ,TimeSpan ShiftStart, TimeSpan
    WorkingHours, TimeSpan LunchStart);