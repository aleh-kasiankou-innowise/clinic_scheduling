namespace Innowise.Clinic.Scheduling.Services.Dto;

public record TimeSlotReservationDto(Guid AppointmentId, DateTime AppointmentStart, DateTime AppointmentFinish);