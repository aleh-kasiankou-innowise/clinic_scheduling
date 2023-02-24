using Innowise.Clinic.Scheduling.Api.Controllers.Abstractions;
using Innowise.Clinic.Scheduling.Services.Dto;
using Innowise.Clinic.Scheduling.Services.ScheduleService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Innowise.Clinic.Scheduling.Api.Controllers;

/// <summary>
/// Controller available only within the internal microservice network.
/// </summary>
public class HelperServicesController : ApiControllerBase
{
    private readonly IScheduleService _scheduleService;

    public HelperServicesController(IScheduleService scheduleService)
    {
        _scheduleService = scheduleService;
    }

    /// <summary>
    /// Generates a schedule for the created doctors.
    /// </summary>
    /// <param name="generationRequest">The object containing the id of the doctor and the date of doctor's first day.</param>
    /// <returns>Status code indicating whether request succeeded.</returns>
    [HttpPost("schedule-generation")]
    public async Task<IActionResult> GenerateSchedule([FromBody] ScheduleGenerationRequest generationRequest)
    {
        await _scheduleService.GenerateScheduleAsync(generationRequest);
        return Ok();
    }
}