using Innowise.Clinic.Scheduling.Api.Controllers.Abstractions;
using Innowise.Clinic.Scheduling.Services.Dto;
using Innowise.Clinic.Scheduling.Services.TimeSlotService.Interfaces;
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
    /// <param name="appointmentDuration">Duration of service in minutes</param>
    /// <returns>List of available timeslots for an appointment.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TimeSlotDto>>> GetFreeTimeSlots([FromQuery] Guid doctorId,
        [FromQuery] int appointmentDuration)
    {
        return Ok(await _timeSlotService.GetFreeTimeSlots(doctorId, appointmentDuration));
    }

    /// <summary>
    /// Reserves timeslot fot appointment.
    /// </summary>
    /// <param name="timeSlotReservationDto"></param>
    /// <returns>Id of the reserved timeslot.</returns>
    [HttpPost]
    public async Task<ActionResult<Guid>> ReserveTimeSlot([FromBody] TimeSlotReservationDto timeSlotReservationDto)
    {
        return Ok((await _timeSlotService.ReserveSlot(timeSlotReservationDto)).ToString());
    }

    /// <summary>
    /// Updates appointment time.
    /// </summary>
    /// <param name="id">Id of the appointment.</param>
    /// <param name="timeSlotReservationDto">Object with appointment start time and end time.</param>
    /// <returns>Status code indicating whether request succeeded.</returns>
    [HttpPut("appointments/{id:guid}")]
    public async Task<IActionResult> EditTimeSlot([FromRoute] Guid id,
        [FromBody] TimeSlotReservationDto timeSlotReservationDto)
    {
        await _timeSlotService.UpdateTimeSlot(id, timeSlotReservationDto);
        return Ok();
    }
}