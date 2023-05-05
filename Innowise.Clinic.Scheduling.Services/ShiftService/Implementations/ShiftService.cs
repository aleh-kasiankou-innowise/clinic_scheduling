using Innowise.Clinic.Scheduling.Persistence;
using Innowise.Clinic.Scheduling.Persistence.Models;
using Innowise.Clinic.Scheduling.Services.Dto;
using Innowise.Clinic.Scheduling.Services.Exceptions;
using Innowise.Clinic.Scheduling.Services.ShiftService.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Innowise.Clinic.Scheduling.Services.ShiftService.Implementations;

// TODO CREATE A SHIFT PREFERENCE SERVICE
// TODO MOVE DB LOGIC TO A REPO
public class ShiftService : IShiftService
{
    private readonly SchedulingDbContext _dbContext;
    private readonly HangfireHelper.HangfireHelper _schedulingHelper;

    public ShiftService(SchedulingDbContext dbContext, HangfireHelper.HangfireHelper schedulingHelper)
    {
        _dbContext = dbContext;
        this._schedulingHelper = schedulingHelper;
    }


    public async Task<IEnumerable<Shift>> GetShiftsAsync()
    {
        return await _dbContext.Shifts.ToListAsync();
    }

    public async Task<Shift> GetShiftAsync(Guid id)
    {
        return await GetShiftOrThrowException(id);
    }

    public async Task<ShiftPreference?> GetShiftPreferenceAsync(Guid doctorId)
    {
        return await _dbContext
            .ShiftPreferences
            .FirstOrDefaultAsync(x => x.DoctorId == doctorId);
    }

    // TODO Does it make sense to return GUID that is never used?
    public async Task<Guid> SetShiftPreferenceAsync(Guid doctorId, Guid shiftId)
    {
        var savedPreference = await GetShiftPreferenceAsync(doctorId);
        var scheduleGenerationRequest = new ScheduleGenerationRequest(doctorId, DateTime.Today);
        if (savedPreference is not null)
        {
            savedPreference.ShiftId = shiftId;
            await _dbContext.SaveChangesAsync();
            _schedulingHelper.GenerateScheduleForDoctor(scheduleGenerationRequest);
            return savedPreference.ShiftPreferenceId;
        }

        var shiftPreference = new ShiftPreference()
        {
            DoctorId = doctorId,
            ShiftId = shiftId,
        };

        _dbContext.ShiftPreferences.Add(shiftPreference);
        await _dbContext.SaveChangesAsync();
        _schedulingHelper.GenerateScheduleForDoctor(scheduleGenerationRequest);
        return shiftPreference.ShiftPreferenceId;
    }


    public async Task<Guid> CreateShiftAsync(ShiftDto newShiftInfo)
    {
        var shiftToSave = new Shift
        {
            ShiftName = newShiftInfo.ShiftName,
            ShiftStart = newShiftInfo.ShiftStart,
            LunchStart = newShiftInfo.LunchStart,
            WorkingHours = newShiftInfo.WorkingHours
        };

        _dbContext.Shifts.Add(shiftToSave);
        await _dbContext.SaveChangesAsync();
        return shiftToSave.ShiftId;
    }

    public async Task UpdateShiftAsync(Guid id, ShiftDto updatedShiftInfo)
    {
        var savedShift = await GetShiftOrThrowException(id);

        savedShift.ShiftName = updatedShiftInfo.ShiftName;
        savedShift.ShiftStart = updatedShiftInfo.ShiftStart;
        savedShift.LunchStart = updatedShiftInfo.LunchStart;
        savedShift.WorkingHours = updatedShiftInfo.WorkingHours;

        _dbContext.Update(savedShift);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteShiftAsync(Guid id)
    {
        var savedShift = await GetShiftOrThrowException(id);
        _dbContext.Remove(savedShift);
        await _dbContext.SaveChangesAsync();
    }

    private async Task<Shift> GetShiftOrThrowException(Guid id)
    {
        return await _dbContext.Shifts.FirstOrDefaultAsync(x => x.ShiftId == id) ??
               throw new MissingEntryException($"The shift with id {id} does not exist.");
    }
}