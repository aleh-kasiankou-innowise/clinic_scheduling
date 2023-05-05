using Innowise.Clinic.Scheduling.Persistence;
using Innowise.Clinic.Scheduling.Persistence.Models;
using Innowise.Clinic.Scheduling.Services.Dto;
using Innowise.Clinic.Scheduling.Services.Options;
using Innowise.Clinic.Scheduling.Services.ScheduleGenerationService.Interfaces;
using Innowise.Clinic.Scheduling.Services.ShiftService.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Innowise.Clinic.Scheduling.Services.ScheduleGenerationService.Implementations;

public class PreferenceBasedScheduleGenerationService : IScheduleGenerationService
{
    private readonly SchedulingDbContext _dbContext;
    private readonly ScheduleGenerationConfigOptions _generationConfig;
    private readonly IShiftService _shiftService;

    public PreferenceBasedScheduleGenerationService(SchedulingDbContext dbContext,
        IOptions<ScheduleGenerationConfigOptions> generationConfig, IShiftService shiftService)
    {
        _dbContext = dbContext;
        _shiftService = shiftService;
        _generationConfig = generationConfig.Value;
    }

    public async Task GenerateScheduleAsync()
    {
        var today = DateTime.Today;
        var endGenerationMonth = today.AddMonths(_generationConfig.GenerateForMonths).Month;
        var savedScheduleData = await
            _dbContext.Schedules.Where(x => x.Day >= today && x.Day.Month <= endGenerationMonth)
                .Select(e => new { e.DoctorId, e.Day }).ToListAsync();
        var doctorsPreferences = await _dbContext.ShiftPreferences.ToListAsync();
        var generatedSchedule = new List<Schedule>();

        for (var i = today; i.Month <= endGenerationMonth; i = i.AddDays(1))
        {
            if (savedScheduleData.Count(x => x.Day == i) <
                doctorsPreferences.Count)
            {
                foreach (var preference in doctorsPreferences)
                {
                    if (preference is null)
                    {
                        continue;
                    }

                    if (savedScheduleData.All(x => x.DoctorId != preference.DoctorId && x.Day != i))
                    {
                        generatedSchedule.Add(new()
                        {
                            DoctorId = preference.DoctorId,
                            Day = i.Date,
                            ShiftId = preference.ShiftId,
                        });
                    }
                }
            }
        }

        await _dbContext.Schedules.AddRangeAsync(generatedSchedule);
        await _dbContext.SaveChangesAsync();
    }

    public async Task GenerateScheduleAsync(ScheduleGenerationRequest generationRequest)
    {
        var endGenerationMonth = generationRequest.GenerateFrom.AddMonths(_generationConfig.GenerateForMonths).Month;
        var doctorPreference = await _shiftService.GetShiftPreferenceAsync(generationRequest.DoctorId) ??
                               throw new ApplicationException("The doctor should save the preferred schedule.");
        var savedScheduleData = await
            _dbContext.Schedules.Where(x =>
                x.Day >= generationRequest.GenerateFrom.Date && x.Day.Month <= endGenerationMonth &&
                x.DoctorId == generationRequest.DoctorId).ToListAsync();

        _dbContext.Schedules.RemoveRange(savedScheduleData);
        
        var generatedSchedule = new List<Schedule>();
        for (var i = generationRequest.GenerateFrom; i.Month <= endGenerationMonth; i = i.AddDays(1))
        {
            generatedSchedule.Add(new()
            {
                DoctorId = doctorPreference.DoctorId,
                Day = i.Date,
                ShiftId = doctorPreference.ShiftId,
            });
        }

        await _dbContext.Schedules.AddRangeAsync(generatedSchedule);
        await _dbContext.SaveChangesAsync();
    }
}