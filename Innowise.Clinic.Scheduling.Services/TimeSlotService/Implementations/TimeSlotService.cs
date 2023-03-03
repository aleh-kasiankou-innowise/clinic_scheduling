using Innowise.Clinic.Scheduling.Persistence;
using Innowise.Clinic.Scheduling.Persistence.Models;
using Innowise.Clinic.Scheduling.Services.Dto;
using Innowise.Clinic.Scheduling.Services.Exceptions;
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

    public async Task<IEnumerable<FreeTimeSlotDto>> GetFreeTimeSlots(Guid doctorId, DateTime appointmentDay,
        TimeSpan appointmentDuration)
    {
        var reservedTimeSlots = await GetReservedTimeSlotsAsync(doctorId, appointmentDay);
        var doctorSchedule = await GetDoctorSchedule(doctorId, appointmentDay);
        var lunchStart = appointmentDay + doctorSchedule.LunchStart;
        var shiftStart = appointmentDay + doctorSchedule.ShiftStart;
        var shiftEnd = (shiftStart + doctorSchedule.WorkingHours).AddMinutes(_workingDayConfig.LunchDurationInMinutes);
        return CalculateFreeTimeSlots(shiftStart, shiftEnd, lunchStart, appointmentDuration, reservedTimeSlots);
    }

    public async Task<ReservedTimeSlot> GetReservedTimeSlotAsync(Guid id)
    {
        return await _dbContext.ReservedTimeSlots.FirstOrDefaultAsync(x => x.ReservedTimeSlotId == id) ??
               throw new MissingEntryException($"There is no booking with such id: {id}.");
    }

    public async Task<Guid> ReserveSlotAsync(TimeSlotReservationDto timeSlotReservationDto)
    {
        var appointmentDuration = timeSlotReservationDto.AppointmentFinish - timeSlotReservationDto.AppointmentStart;
        var freeTimeSlots = await GetFreeTimeSlots(timeSlotReservationDto.DoctorId,
            timeSlotReservationDto.AppointmentStart.Date, appointmentDuration);

        if (freeTimeSlots.Any(x =>
                x.AppointmentStart == timeSlotReservationDto.AppointmentStart &&
                x.AppointmentEnd == timeSlotReservationDto.AppointmentFinish))
        {
            var timeSlotReservation = new ReservedTimeSlot
            {
                AppointmentStart = timeSlotReservationDto.AppointmentStart,
                AppointmentFinish = timeSlotReservationDto.AppointmentFinish,
                DoctorId = timeSlotReservationDto.DoctorId
            };

            _dbContext.ReservedTimeSlots.Add(timeSlotReservation);
            await _dbContext.SaveChangesAsync();
            return timeSlotReservation.ReservedTimeSlotId;
        }

        throw new InvalidTimeslotException(
            $"There is no timeslot with the requested timeframe: {timeSlotReservationDto.AppointmentStart} - {timeSlotReservationDto.AppointmentFinish}");
    }

    public async Task<Guid> UpdateTimeSlotAsync(Guid id, TimeSlotReservationDto timeSlotReservationDto)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            await DeleteTimeSlotAsync(id);
            var reservedTimeSlotId = await ReserveSlotAsync(timeSlotReservationDto);
            await transaction.CommitAsync();
            return reservedTimeSlotId;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task DeleteTimeSlotAsync(Guid id)
    {
        var timeSlotToRemove = await GetReservedTimeSlotAsync(id);
        _dbContext.ReservedTimeSlots.Remove(timeSlotToRemove);
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
               throw new MissingEntryException("The schedule for the day is not generated.");
    }

    private bool IsOverlapsWithDifferentAppointment(DateTime timeSlotStartTime, DateTime timeSlotEndTime,
        IList<ReservedTimeSlot> reservedTimeSlots)
    {
        return reservedTimeSlots.Any(x =>
            x.AppointmentStart <= timeSlotEndTime &&
            x.AppointmentFinish >= timeSlotStartTime);
    }

    private bool IsOverlapsWithLunch(DateTime timeSlotStartTime, DateTime timeSlotEndTime, DateTime lunchStart)
    {
        var lunchEnd = lunchStart + new TimeSpan(0, _workingDayConfig.LunchDurationInMinutes, 0);
        return lunchStart <= timeSlotEndTime &&
               lunchEnd >= timeSlotStartTime;
    }

    private IEnumerable<FreeTimeSlotDto> CalculateFreeTimeSlots(
        DateTime shiftStart,
        DateTime shiftEnd,
        DateTime lunchStart,
        TimeSpan appointmentDuration,
        IList<ReservedTimeSlot> reservedTimeSlots)
    {
        var freeTimeSlots = new List<FreeTimeSlotDto>();
        var currentTimeSlotStartTime = shiftStart;
        while (currentTimeSlotStartTime <= shiftEnd - appointmentDuration)
        {
            var currentTimeSlotEndTime = currentTimeSlotStartTime + appointmentDuration;
            if (!IsOverlapsWithDifferentAppointment(currentTimeSlotStartTime, currentTimeSlotEndTime,
                    reservedTimeSlots) &&
                !IsOverlapsWithLunch(currentTimeSlotStartTime, currentTimeSlotEndTime, lunchStart))
            {
                freeTimeSlots.Add(new(currentTimeSlotStartTime, currentTimeSlotEndTime));
            }

            currentTimeSlotStartTime =
                currentTimeSlotEndTime.AddMinutes(_workingDayConfig.BreaksBetweenAppointmentsInMinutes);
        }

        return freeTimeSlots;
    }
}