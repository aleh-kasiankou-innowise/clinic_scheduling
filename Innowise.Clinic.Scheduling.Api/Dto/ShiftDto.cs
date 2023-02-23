namespace Innowise.Clinic.Scheduling.Api.Dto;

/// <summary>
/// 
/// </summary>
/// <param name="ShiftName">Publicly visible shift title.</param>
/// <param name="ShiftStart">Time when the shift starts.</param>
/// <param name="ShiftEnd">Time when the shift ends.</param>
/// <param name="LunchStart">Time when the lunch break starts.</param>
public record ShiftDto(string ShiftName ,TimeOnly ShiftStart, TimeOnly ShiftEnd, TimeOnly LunchStart);