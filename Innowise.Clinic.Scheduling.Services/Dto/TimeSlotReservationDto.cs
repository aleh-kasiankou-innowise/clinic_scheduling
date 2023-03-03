namespace Innowise.Clinic.Scheduling.Services.Dto;

public record TimeSlotReservationDto(Guid DoctorId, DateTime AppointmentStart, DateTime AppointmentFinish);
