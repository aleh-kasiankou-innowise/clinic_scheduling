using Innowise.Clinic.Scheduling.Services.Dto;
using Innowise.Clinic.Scheduling.Services.TimeSlotService.Interfaces;
using Innowise.Clinic.Shared.BaseClasses;
using Microsoft.AspNetCore.Mvc;

namespace Innowise.Clinic.Scheduling.Api.Controllers;

/// <summary>
///  Manages timeslots necessary for making appointments.
/// </summary>
public class TimeSlotsController : ApiControllerBase
{
    private readonly ITimeSlotService _timeSlotService;

    /// <inheritdoc />
    public TimeSlotsController(ITimeSlotService timeSlotService)
    {
        _timeSlotService = timeSlotService;
    }

    /// <summary>
    /// Lists available timeslots for the specified doctor.
    /// </summary>
    /// <param name="doctorId">Id of the doctor whose schedule is to be checked.</param>
    /// <param name="appointmentDay">Day of the appointment.</param>
    /// <param name="duration">Duration of appointment in minutes.</param>
    /// <returns>List of available timeslots for an appointment.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TimeSlotDto>>> GetFreeTimeSlots([FromQuery] Guid doctorId, [FromQuery] DateTime appointmentDay,
        [FromQuery] int duration)
    {
        return Ok(await _timeSlotService.GetFreeTimeSlots(doctorId, appointmentDay, new TimeSpan(0, duration, 0)));
    }
}