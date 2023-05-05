using Innowise.Clinic.Scheduling.Services.Dto;

namespace Innowise.Clinic.Scheduling.Services.ShiftService.Implementations;

public class ShiftPreferenceSaveEventArgs : EventArgs
{
    public ScheduleGenerationRequest ScheduleGenerationRequest { get; }

    public ShiftPreferenceSaveEventArgs(ScheduleGenerationRequest generationRequest)
    {
        ScheduleGenerationRequest = generationRequest;
    }
}