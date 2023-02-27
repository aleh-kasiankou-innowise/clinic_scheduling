using Innowise.Clinic.Scheduling.Persistence;
using Innowise.Clinic.Scheduling.Persistence.Models;
using Innowise.Clinic.Scheduling.Services.Dto;
using Innowise.Clinic.Scheduling.Services.Exceptions;
using Innowise.Clinic.Scheduling.Services.ShiftService.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Innowise.Clinic.Scheduling.Services.ShiftService.Implementations;

public class ShiftService : IShiftService
{
    private readonly SchedulingDbContext _dbContext;

    public ShiftService(SchedulingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Shift>> GetShiftsAsync()
    {
        return await _dbContext.Shifts.ToListAsync();
    }
    
    public async Task<Shift> GetShiftAsync(Guid id)
    {
        return await GetShiftOrThrowException(id);
    }

    public async Task<ShiftPreference> GetShiftPreferenceAsync(Guid doctorId)
    {
        return await GetPreferredShiftOrThrowException(doctorId);
    }

    public async Task<Guid> SetShiftPreferenceAsync(Guid doctorId, Guid shiftId)
    {
        // TODO ENSURE PREFERENCE IS AUTOMATICALLY SET AFTER PROFILE CREATION
        throw new NotImplementedException();
    }

    public async Task UpdateShiftPreferenceAsync(Guid doctorId, Guid shiftId)
    {
        throw new NotImplementedException();
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
    
    private async Task<ShiftPreference> GetPreferredShiftOrThrowException(Guid doctorId)
    {
        var doctorShiftPreference = await _dbContext.ShiftPreferences.FirstOrDefaultAsync(x => x.DoctorId == doctorId);
        if (doctorShiftPreference == null)
        {
            // TODO IF NULL, SET DEFAULT SHIFT
        }
        return doctorShiftPreference;
    }
}