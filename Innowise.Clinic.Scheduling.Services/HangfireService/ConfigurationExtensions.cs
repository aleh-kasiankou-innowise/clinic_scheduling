using Hangfire;
using Innowise.Clinic.Scheduling.Services.ScheduleGenerationService.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Innowise.Clinic.Scheduling.Services.HangfireService;

public static class ConfigurationExtensions
{
    public static async Task ConfigureBackgroundScheduleGeneration(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var scheduleGenerator = scope.ServiceProvider.GetRequiredService<IScheduleGenerationService>();
        await scheduleGenerator.GenerateScheduleAsync();
        RecurringJob.AddOrUpdate("GenerateDoctorSchedule", () => scheduleGenerator.GenerateScheduleAsync(),
            Cron.Monthly(25));
    }
}