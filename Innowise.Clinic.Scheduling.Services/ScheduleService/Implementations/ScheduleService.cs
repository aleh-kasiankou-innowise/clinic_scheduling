using Innowise.Clinic.Scheduling.Persistence;
using Innowise.Clinic.Scheduling.Persistence.Models;
using Innowise.Clinic.Scheduling.Services.Dto;
using Innowise.Clinic.Scheduling.Services.Exceptions;
using Innowise.Clinic.Scheduling.Services.ScheduleService.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Innowise.Clinic.Scheduling.Services.ScheduleService.Implementations;

public class ScheduleService : IScheduleService
{
    private readonly SchedulingDbContext _dbContext;

    public ScheduleService(SchedulingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Schedule>> GetScheduleAsync(Guid specializationId, Guid officeId, DateTime from,
        DateTime to)
    {
        return await _dbContext.Schedules
            .Where(x =>
                x.SpecializationId == specializationId
                && x.OfficeId == officeId
                && x.Day >= from
                && x.Day <= to)
            .OrderBy(x => x.Day)
            .ThenBy(x => x.DoctorId)
            .ToListAsync();
    }

    public async Task UpdateScheduleAsync(Guid doctorId, EditScheduleForMonthDto editScheduleForMonthDto)
    {
        var datesToUpdate = editScheduleForMonthDto.ScheduleUpdateForMonth
            .Select(x => x.Day)
            .ToArray();

        var savedSchedulesToUpdate = _dbContext.Schedules
            .Where(x =>
                x.DoctorId == doctorId
                && x.Day >= datesToUpdate.Min()
                && x.Day <= datesToUpdate.Max());

        foreach (var scheduleToUpdate in editScheduleForMonthDto.ScheduleUpdateForMonth)
        {
            var savedScheduleToUpdate =
                savedSchedulesToUpdate
                    .FirstOrDefault(x => x.Day == scheduleToUpdate.Day.Date)
                ?? throw new MissingEntryException(
                    $"The schedule for the day is not generated:{scheduleToUpdate.Day.Date}.");
            savedScheduleToUpdate.ShiftId = scheduleToUpdate.ShiftId;
        }

        _dbContext.UpdateRange(savedSchedulesToUpdate);
        await _dbContext.SaveChangesAsync();
    }
}