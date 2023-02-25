using Innowise.Clinic.Scheduling.Persistence.Models;
using Innowise.Clinic.Scheduling.Services.Dto;

namespace Innowise.Clinic.Scheduling.Services.ScheduleService.Interfaces;

public interface IScheduleService
{
    Task<IEnumerable<Schedule>> GetScheduleAsync(Guid specialization, Guid office, DateTime from, DateTime to);
    Task UpdateScheduleAsync(Guid doctorId, EditScheduleForMonthDto editScheduleForMonthDto);
}