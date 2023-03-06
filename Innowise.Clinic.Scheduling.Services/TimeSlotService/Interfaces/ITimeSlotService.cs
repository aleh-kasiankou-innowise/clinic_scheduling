using Innowise.Clinic.Scheduling.Services.Dto;
using Innowise.Clinic.Shared.MassTransit.MessageTypes;

namespace Innowise.Clinic.Scheduling.Services.TimeSlotService.Interfaces;

public interface ITimeSlotService
{
    Task<IEnumerable<FreeTimeSlotDto>> GetFreeTimeSlots(Guid doctorId, DateTime appointmentDay,
        TimeSpan appointmentDuration);
    Task<Guid> ReserveSlotAsync(TimeSlotReservationRequest timeSlotReservationRequest);
    Task<Guid> UpdateTimeSlotAsync(Guid id, TimeSlotReservationRequest timeSlotReservationRequest);
}