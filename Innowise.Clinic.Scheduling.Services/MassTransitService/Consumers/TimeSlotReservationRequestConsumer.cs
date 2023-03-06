using Innowise.Clinic.Scheduling.Services.Dto;
using Innowise.Clinic.Scheduling.Services.TimeSlotService.Interfaces;
using Innowise.Clinic.Shared.MassTransit.MessageTypes;
using MassTransit;

namespace Innowise.Clinic.Scheduling.Services.MassTransitService.Consumers;

public class TimeSlotReservationRequestConsumer : IConsumer<TimeSlotReservationRequest>
{
    private readonly ITimeSlotService _timeSlotService;

    public TimeSlotReservationRequestConsumer(ITimeSlotService timeSlotService)
    {
        _timeSlotService = timeSlotService;
    }

    public async Task Consume(ConsumeContext<TimeSlotReservationRequest> context)
    {
        var reservation = new TimeSlotReservationDto(context.Message.DoctorId, context.Message.AppointmentStart,
            context.Message.AppointmentEnd);
        try
        {
            var timeSlotId = await _timeSlotService.ReserveSlotAsync(reservation);
            await context.RespondAsync<TimeSlotReservationResponse>(new(true, timeSlotId, null));
        }
        catch (Exception e)
        {
            var message = e is ApplicationException
                ? e.Message
                : "Internal Server Error. Please contact out support team.";
            await context.RespondAsync<TimeSlotReservationResponse>(new(false, null, message));
        }
    }
}