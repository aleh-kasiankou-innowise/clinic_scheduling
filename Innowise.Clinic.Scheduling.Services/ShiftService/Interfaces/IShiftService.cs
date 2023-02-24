using Innowise.Clinic.Scheduling.Persistence.Models;
using Innowise.Clinic.Scheduling.Services.Dto;

namespace Innowise.Clinic.Scheduling.Services.ShiftService.Interfaces;

public interface IShiftService
{
    Task<IEnumerable<Shift>> GetShifts();
    Task<Shift> GetShift(Guid id);
    Task<Guid> CreateShift(ShiftDto newShiftInfo);
    Task UpdateShift(Guid id, ShiftDto updatedShiftInfo);
    Task DeleteShift(Guid id);
}