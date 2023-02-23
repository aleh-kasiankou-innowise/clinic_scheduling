using Innowise.Clinic.Scheduling.Services.Dto;

namespace Innowise.Clinic.Scheduling.Services.TimeSlotService.Interfaces;

public interface ITimeSlotService
{
    Task<IEnumerable<TimeSlotDto>> GetFreeTimeSlots(Guid doctorId, int appointmentDuration);
    Task<Guid> ReserveSlot(TimeSlotReservationDto timeSlotReservationDto);
    Task UpdateTimeSlot(Guid id, TimeSlotReservationDto timeSlotReservationDto);
}