using Innowise.Clinic.Scheduling.Persistence.Models;
using Innowise.Clinic.Scheduling.Services.Dto;

namespace Innowise.Clinic.Scheduling.Services.ShiftService.Interfaces;

public interface IShiftService
{
    Task<IEnumerable<Shift>> GetShiftsAsync();
    Task<Shift> GetShiftAsync(Guid id);
    Task<ShiftPreference> GetShiftPreferenceAsync(Guid doctorId);
    Task<Guid> SetShiftPreferenceAsync(Guid doctorId, Guid shiftId);
    Task UpdateShiftPreferenceAsync(Guid doctorId, Guid shiftId);
    Task<Guid> CreateShiftAsync(ShiftDto newShiftInfo);
    Task UpdateShiftAsync(Guid id, ShiftDto updatedShiftInfo);
    Task DeleteShiftAsync(Guid id);
}