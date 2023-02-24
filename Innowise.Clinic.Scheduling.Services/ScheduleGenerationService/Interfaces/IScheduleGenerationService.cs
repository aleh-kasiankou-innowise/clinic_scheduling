using Innowise.Clinic.Scheduling.Services.Dto;

namespace Innowise.Clinic.Scheduling.Services.ScheduleGenerationService.Interfaces;

public interface IScheduleGenerationService
{
    Task GenerateScheduleAsync();
    Task GenerateScheduleAsync(ScheduleGenerationRequest generationRequest);
}