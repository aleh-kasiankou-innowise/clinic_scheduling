using Innowise.Clinic.Scheduling.Persistence;
using Innowise.Clinic.Scheduling.Persistence.Models;
using Innowise.Clinic.Scheduling.Services.Dto;
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

    public async Task<IEnumerable<Shift>> GetShifts()
    {
        return await _dbContext.Shifts.ToListAsync();
    }
    
    public async Task<Shift> GetShift(Guid id)
    {
        return await GetShiftOrThrowException(id);
    }

    public async Task<ShiftPreference> GetShiftPreference(Guid id)
    {
        return await GetPreferredShiftOrThrowException(id);
    }

    public async Task<Guid> CreateShift(ShiftDto newShiftInfo)
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

    public async Task UpdateShift(Guid id, ShiftDto updatedShiftInfo)
    {
        var savedShift = await GetShiftOrThrowException(id);

        savedShift.ShiftName = updatedShiftInfo.ShiftName;
        savedShift.ShiftStart = updatedShiftInfo.ShiftStart;
        savedShift.LunchStart = updatedShiftInfo.LunchStart;
        savedShift.WorkingHours = updatedShiftInfo.WorkingHours;

        _dbContext.Update(savedShift);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteShift(Guid id)
    {
        var savedShift = await GetShiftOrThrowException(id);
        _dbContext.Remove(savedShift);
        await _dbContext.SaveChangesAsync();
    }

    private async Task<Shift> GetShiftOrThrowException(Guid id)
    {
        return await _dbContext.Shifts.FirstOrDefaultAsync(x => x.ShiftId == id) ??
               throw new NotImplementedException();
    }
    
    private async Task<ShiftPreference> GetPreferredShiftOrThrowException(Guid id)
    {
        return await _dbContext.ShiftPreferences.FirstOrDefaultAsync(x => x.DoctorId == id) ??
               throw new NotImplementedException();
    }
}