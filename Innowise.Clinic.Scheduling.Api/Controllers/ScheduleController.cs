using Innowise.Clinic.Scheduling.Api.Controllers.Abstractions;
using Innowise.Clinic.Scheduling.Api.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Innowise.Clinic.Scheduling.Api.Controllers;

/// <summary>
/// Provides access to working schedules of the doctors who work in the clinic. 
/// </summary>
public class ScheduleController : ApiControllerBase
{
    // TODO GET SCHEDULE BY DOCTOR / RECEPTIONIST WITH DIFFERENT SETS OF FILTERS
    // TODO CREATE A FILTER TO LIMIT FILTERS BY ROLE

    /// <summary>
    /// Displays the doctors' working schedule for the specified period. 
    /// </summary>
    /// <param name="specialization">Filters schedule by doctor specialization.</param>
    /// <param name="office">Filters results by doctor office.</param>
    /// <param name="from">Displays schedule from date.</param>
    /// <param name="to">Displays schedule to date.</param>
    /// <param name="doctorId">Displays schedule for the specified doctor only.</param>
    /// <returns>Doctors' working schedule for the specified period in accordance with the filters.</returns>
    [HttpGet]
    [Authorize(Roles = "Doctor,Receptionist")]
    public Task<IActionResult> GetScheduleForMonth([FromQuery] Guid specialization, [FromQuery] Guid office,
        [FromQuery] DateOnly from, [FromQuery] DateOnly to, [FromQuery] Guid? doctorId)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Updates doctor schedule.
    /// </summary>
    /// <param name="doctorId">The id of the doctor whose schedule is to be changed.</param>
    /// <param name="editScheduleForMonthDto">An object that contains info about the schedule update.</param>
    /// <returns>Status code indicating whether request succeeded.</returns>
    [HttpPut]
    [Authorize(Roles = "Receptionist")]
    public Task<IActionResult> EditScheduleForMonth([FromQuery] Guid doctorId,
        [FromBody] EditScheduleForMonthDto editScheduleForMonthDto)
    {
        throw new NotImplementedException();
    }
}