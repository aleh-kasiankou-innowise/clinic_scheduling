namespace Innowise.Clinic.Scheduling.Services.Dto;

public record ScheduleGenerationRequest(Guid DoctorId, DateOnly GenerateFrom);