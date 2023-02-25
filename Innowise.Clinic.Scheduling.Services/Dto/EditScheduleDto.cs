namespace Innowise.Clinic.Scheduling.Services.Dto;

/// <summary>
/// Represents the schedule update for the whole month.
/// </summary>
/// <param name="ScheduleUpdateForMonth"></param>
public record EditScheduleForMonthDto(IEnumerable<EditScheduleForDayDto> ScheduleUpdateForMonth);

/// <summary>
/// Represents the schedule update information.
/// </summary>
/// <param name="Day">Day when shift should be changed.</param>
/// <param name="ShiftId">Id of the shift to set for the date. Null value means the employee is absent.</param>
public record EditScheduleForDayDto(DateTime Day, Guid? ShiftId);