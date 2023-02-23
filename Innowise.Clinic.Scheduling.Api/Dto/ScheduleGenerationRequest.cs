namespace Innowise.Clinic.Scheduling.Api.Dto;

public record ScheduleGenerationRequest(Guid DoctorId, DateOnly GenerateFrom);