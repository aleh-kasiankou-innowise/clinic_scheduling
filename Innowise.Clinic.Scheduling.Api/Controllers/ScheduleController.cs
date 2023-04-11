using Innowise.Clinic.Scheduling.Persistence.Models;
using Innowise.Clinic.Scheduling.Services.Dto;
using Innowise.Clinic.Scheduling.Services.ScheduleService.Interfaces;
using Innowise.Clinic.Shared.BaseClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Innowise.Clinic.Scheduling.Api.Controllers;

/// <summary>
/// Provides access to working schedules of the doctors who work in the clinic. 
/// </summary>
public class ScheduleController : ApiControllerBase
{
    private readonly IScheduleService _scheduleService;

    /// <inheritdoc />
    public ScheduleController(IScheduleService scheduleService)
    {
        _scheduleService = scheduleService;
    }

    /// <summary>
    /// Displays the doctors' working schedule for the specified period. 
    /// </summary>
    /// <param name="specialization">Filters schedule by doctor specialization.</param>
    /// <param name="office">Filters results by doctor office.</param>
    /// <param name="from">Displays schedule from date.</param>
    /// <param name="to">Displays schedule to date.</param>
    /// <returns>Doctors' working schedule for the specified period in accordance with the filters.</returns>
    [HttpGet]
    [Authorize(Roles = "Doctor,Receptionist")]
    public async Task<ActionResult<IEnumerable<Schedule>>> GetScheduleForMonth([FromQuery] Guid specialization, [FromQuery] Guid office,
        [FromQuery] DateTime from, [FromQuery] DateTime to)
    {
        return Ok(await _scheduleService.GetScheduleAsync(specialization, office, from, to));
    }

    /// <summary>
    /// Updates doctor schedule.
    /// </summary>
    /// <param name="doctorId">The id of the doctor whose schedule is to be changed.</param>
    /// <param name="editScheduleForMonthDto">An object that contains info about the schedule update.</param>
    /// <returns>Status code indicating whether request succeeded.</returns>
    [HttpPut]
    [Authorize(Roles = "Receptionist")]
    public async Task<IActionResult> EditScheduleForMonth([FromQuery] Guid doctorId,
        [FromBody] EditScheduleForMonthDto editScheduleForMonthDto)
    {
        await _scheduleService.UpdateScheduleAsync(doctorId, editScheduleForMonthDto);
        return Ok();
    }
}