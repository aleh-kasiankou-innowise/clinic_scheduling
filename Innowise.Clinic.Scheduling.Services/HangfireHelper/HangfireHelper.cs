using Hangfire;
using Innowise.Clinic.Scheduling.Services.Dto;
using Innowise.Clinic.Scheduling.Services.ScheduleGenerationService.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Innowise.Clinic.Scheduling.Services.HangfireHelper;

public class HangfireHelper
{
    private readonly IServiceProvider _serviceProvider;
    public HangfireHelper(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void GenerateScheduleForDoctor(ScheduleGenerationRequest scheduleGenerationRequest)
    {
        using var scope = _serviceProvider.CreateScope();
        var serviceInstance = scope.ServiceProvider.GetRequiredService<IScheduleGenerationService>();
        BackgroundJob.Enqueue(() => serviceInstance.GenerateScheduleAsync(scheduleGenerationRequest));
    }
}