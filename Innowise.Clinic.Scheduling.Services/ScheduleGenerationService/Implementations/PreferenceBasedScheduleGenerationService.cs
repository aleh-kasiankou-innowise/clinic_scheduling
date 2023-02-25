using Innowise.Clinic.Scheduling.Persistence;
using Innowise.Clinic.Scheduling.Persistence.Models;
using Innowise.Clinic.Scheduling.Services.Dto;
using Innowise.Clinic.Scheduling.Services.Options;
using Innowise.Clinic.Scheduling.Services.ScheduleGenerationService.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Innowise.Clinic.Scheduling.Services.ScheduleGenerationService.Implementations;

public class PreferenceBasedScheduleGenerationService : IScheduleGenerationService
{
    private readonly SchedulingDbContext _dbContext;
    private readonly ScheduleGenerationConfigOptions _generationConfig;

    public PreferenceBasedScheduleGenerationService(SchedulingDbContext dbContext,
        IOptions<ScheduleGenerationConfigOptions> generationConfig)
    {
        _dbContext = dbContext;
        _generationConfig = generationConfig.Value;
    }

    public async Task GenerateScheduleAsync()
    {
        var today = DateTime.Today;
        var endGenerationMonth = today.AddMonths(_generationConfig.GenerateForMonths).Month;
        var doctorsPreferences = await _dbContext.ShiftPreferences.ToListAsync();
        var generatedSchedule = new List<Schedule>();

        for (var i = today; i.Month <= endGenerationMonth; i = i.AddDays(1))
        {
            foreach (var preference in doctorsPreferences)
            {
                generatedSchedule.Add(new()
                {
                    SpecializationId = preference.SpecializationId,
                    OfficeId = preference.OfficeId,
                    DoctorId = preference.DoctorId,
                    Day = i.Date,
                    ShiftId = preference.ShiftId,
                });
            }
        }

        await _dbContext.Schedules.AddRangeAsync(generatedSchedule);
        await _dbContext.SaveChangesAsync();
    }

    public async Task GenerateScheduleAsync(ScheduleGenerationRequest generationRequest)
    {
        var endGenerationMonth = generationRequest.GenerateFrom.AddMonths(_generationConfig.GenerateForMonths).Month;
        var doctorPreference =
            await _dbContext.ShiftPreferences.FirstOrDefaultAsync(x =>
                x.DoctorId == generationRequest.DoctorId) ?? throw new NotImplementedException();

        var generatedSchedule = new List<Schedule>();

        for (var i = generationRequest.GenerateFrom; i.Month <= endGenerationMonth; i = i.AddDays(1))
        {
            generatedSchedule.Add(new()
            {
                SpecializationId = doctorPreference.SpecializationId,
                OfficeId = doctorPreference.OfficeId,
                DoctorId = doctorPreference.DoctorId,
                Day = i.Date,
                ShiftId = doctorPreference.ShiftId,
            });
        }

        await _dbContext.Schedules.AddRangeAsync(generatedSchedule);
        await _dbContext.SaveChangesAsync();
    }
}