using Innowise.Clinic.Scheduling.Persistence;
using Innowise.Clinic.Scheduling.Persistence.Models;
using Innowise.Clinic.Scheduling.Services.Dto;
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

    public async Task<IEnumerable<Schedule>> GetScheduleAsync(Guid specializationId, Guid officeId, DateOnly from,
        DateOnly to)
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
                    .FirstOrDefault(x => x.ScheduleId == scheduleToUpdate.ShiftId)
                ?? throw new NotImplementedException();
            
            savedScheduleToUpdate.Day = scheduleToUpdate.Day;
        }
        
        _dbContext.UpdateRange(savedSchedulesToUpdate);
        await _dbContext.SaveChangesAsync();
    }
}