using Innowise.Clinic.Scheduling.Services.TimeSlotService.Interfaces;
using Innowise.Clinic.Shared.MassTransit.MessageTypes.Requests;
using MassTransit;

namespace Innowise.Clinic.Scheduling.Services.MassTransitService.Consumers;

public class TimeSlotUpdateRequestConsumer : IConsumer<UpdateAppointmentTimeslotRequest>
{
    private readonly ITimeSlotService _timeSlotService;

    public TimeSlotUpdateRequestConsumer(ITimeSlotService timeSlotService)
    {
        _timeSlotService = timeSlotService;
    }

    public async Task Consume(ConsumeContext<UpdateAppointmentTimeslotRequest> context)
    {
        try
        {
            await _timeSlotService.UpdateTimeSlotAsync(context.Message);
            await context.RespondAsync<UpdateAppointmentTimeslotResponse>(new(true, null));
        }
        catch (Exception e)
        {
            var message = e is ApplicationException
                ? e.Message
                : "Internal Server Error. Please contact out support team.";
            await context.RespondAsync<UpdateAppointmentTimeslotResponse>(new(false, message));
        }
    }
}