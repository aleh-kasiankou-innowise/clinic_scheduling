using Innowise.Clinic.Scheduling.Services.Dto;

namespace Innowise.Clinic.Scheduling.Services.TimeSlotService.Interfaces;

public interface ITimeSlotService
{
    Task<IEnumerable<FreeTimeSlotDto>> GetFreeTimeSlots(Guid doctorId, DateTime appointmentDay,
        TimeSpan appointmentDuration);
    Task<Guid> ReserveSlotAsync(TimeSlotReservationDto timeSlotReservationDto);
    Task<Guid> UpdateTimeSlotAsync(Guid id, TimeSlotReservationDto timeSlotReservationDto);
}