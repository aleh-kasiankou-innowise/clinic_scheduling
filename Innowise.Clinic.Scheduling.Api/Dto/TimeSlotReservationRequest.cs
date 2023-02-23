namespace Innowise.Clinic.Scheduling.Api.Dto;

/// <summary>
/// Request for reserving appointment time.
/// </summary>
/// <param name="DoctorId">Id of the doctor</param>
/// <param name="AppointmentStart"></param>
/// <param name="AppointmentEnd"></param>
public record TimeSlotReservationRequest(Guid DoctorId, DateTime AppointmentStart, DateTime AppointmentEnd);