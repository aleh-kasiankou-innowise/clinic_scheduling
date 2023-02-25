using Innowise.Clinic.Scheduling.Persistence;
using Innowise.Clinic.Scheduling.Persistence.Models;
using Innowise.Clinic.Scheduling.Services.Dto;
using Innowise.Clinic.Scheduling.Services.Options;
using Innowise.Clinic.Scheduling.Services.TimeSlotService.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Innowise.Clinic.Scheduling.Services.TimeSlotService.Implementations;

public class TimeSlotService : ITimeSlotService
{
    private readonly SchedulingDbContext _dbContext;
    private readonly WorkingDayConfigOptions _workingDayConfig;

    public TimeSlotService(SchedulingDbContext dbContext, IOptions<WorkingDayConfigOptions> workingDayConfig)
    {
        _dbContext = dbContext;
        _workingDayConfig = workingDayConfig.Value;
    }

    public async Task<List<FreeTimeSlotDto>> GetFreeTimeSlots(Guid doctorId, DateTime appointmentDay,
        TimeSpan appointmentDuration)
    {
        var reservedTimeSlots = await GetReservedTimeSlotsAsync(doctorId, appointmentDay);
        var doctorSchedule = await GetDoctorSchedule(doctorId, appointmentDay);
        var shiftStart = appointmentDay + doctorSchedule.ShiftStart;
        var shiftEnd = shiftStart + doctorSchedule.WorkingHours +
                       new TimeSpan(0, _workingDayConfig.LunchDurationInMinutes, 0);
        var lunchStart = appointmentDay + doctorSchedule.LunchStart;
        var lunchEnd = lunchStart + new TimeSpan(0, _workingDayConfig.LunchDurationInMinutes, 0);

        var freeTimeSlots = new List<FreeTimeSlotDto>();
        var currentTimeSlotStartTime = shiftStart;
        while (currentTimeSlotStartTime <= shiftEnd - appointmentDuration)
        {
            var currentTimeSlotEndTime = currentTimeSlotStartTime + appointmentDuration;

            if (!IsOverlapsWithDifferentAppointment(currentTimeSlotStartTime, currentTimeSlotEndTime) &&
                !IsOverlapsWithLunch(currentTimeSlotStartTime, currentTimeSlotEndTime))
            {
                freeTimeSlots.Add(new(currentTimeSlotStartTime, currentTimeSlotEndTime));
            }

            currentTimeSlotStartTime =
                currentTimeSlotEndTime.AddMinutes(_workingDayConfig.BreaksBetweenAppointmentsInMinutes);
        }

        return freeTimeSlots;

        bool IsOverlapsWithDifferentAppointment(DateTime timeSlotStartTime, DateTime timeSlotEndTime)
        {
            return (reservedTimeSlots ?? throw new NotImplementedException()).Any(x =>
                x.AppointmentStart <= timeSlotEndTime &&
                x.AppointmentFinish >= timeSlotStartTime);
        }

        bool IsOverlapsWithLunch(DateTime timeSlotStartTime, DateTime timeSlotEndTime)
        {
            return lunchStart <= timeSlotEndTime &&
                   lunchEnd >= timeSlotStartTime;
        }
    }

    public async Task<ReservedTimeSlot> GetReservedTimeSlot(Guid id)
    {
        return await _dbContext.ReservedTimeSlots.FirstOrDefaultAsync(x => x.ReservedTimeSlotId == id) ??
               throw new NotImplementedException();
    }

    public async Task<Guid> ReserveSlot(TimeSlotReservationDto timeSlotReservationDto)
    {
        
        // todo check whether slot is free
        
        var timeSlotReservation = new ReservedTimeSlot
        {
            AppointmentId = timeSlotReservationDto.AppointmentId,
            AppointmentStart = timeSlotReservationDto.AppointmentStart,
            AppointmentFinish = timeSlotReservationDto.AppointmentFinish,
            DoctorId = timeSlotReservationDto.DoctorId
        };

        _dbContext.ReservedTimeSlots.Add(timeSlotReservation);
        await _dbContext.SaveChangesAsync();
        return timeSlotReservation.ReservedTimeSlotId;
    }

    public async Task UpdateTimeSlot(Guid id, TimeSlotReservationDto timeSlotReservationDto)
    {
        var timeSlot = await _dbContext.ReservedTimeSlots.FirstOrDefaultAsync(x => x.AppointmentId == id) ??
                       throw new NotImplementedException();
        timeSlot.AppointmentStart = timeSlotReservationDto.AppointmentStart;
        timeSlot.AppointmentFinish = timeSlot.AppointmentFinish;
        _dbContext.Update(timeSlot);
        await _dbContext.SaveChangesAsync();
    }

    private async Task<List<ReservedTimeSlot>> GetReservedTimeSlotsAsync(Guid doctorId, DateTime appointmentDay)
    {
        return await _dbContext.ReservedTimeSlots
            .Where(x => x.DoctorId == doctorId && x.AppointmentStart.Day == appointmentDay.Day)
            .ToListAsync();
    }

    private async Task<Shift> GetDoctorSchedule(Guid doctorId, DateTime appointmentDay)
    {
        return (await _dbContext.Schedules.Include(x => x.Shift)
                   .FirstOrDefaultAsync(x => x.DoctorId == doctorId && x.Day == appointmentDay.Date))?.Shift ??
               throw new NotImplementedException();
    }
}