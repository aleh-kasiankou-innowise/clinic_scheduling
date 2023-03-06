using Innowise.Clinic.Scheduling.Services.Dto;
using Innowise.Clinic.Shared.MassTransit.MessageTypes.Requests;

namespace Innowise.Clinic.Scheduling.Services.TimeSlotService.Interfaces;

public interface ITimeSlotService
{
    Task<IEnumerable<FreeTimeSlotDto>> GetFreeTimeSlots(Guid doctorId, DateTime appointmentDay,
        TimeSpan appointmentDuration);
    Task<Guid> ReserveSlotAsync(TimeSlotReservationRequest timeSlotReservationRequest, Guid? reservedTimeslotId = null);
    Task UpdateTimeSlotAsync(UpdateAppointmentTimeslotRequest timeslotUpdateRequest);
}