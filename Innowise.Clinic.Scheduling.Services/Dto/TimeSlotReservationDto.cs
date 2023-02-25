namespace Innowise.Clinic.Scheduling.Services.Dto;

public record TimeSlotReservationDto(Guid AppointmentId, Guid DoctorId, DateTime AppointmentStart, DateTime AppointmentFinish);