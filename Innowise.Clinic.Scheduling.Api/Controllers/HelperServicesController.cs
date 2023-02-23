using Innowise.Clinic.Scheduling.Api.Controllers.Abstractions;
using Innowise.Clinic.Scheduling.Api.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Innowise.Clinic.Scheduling.Api.Controllers;

/// <summary>
/// Controller available only within the internal microservice network.
/// </summary>
public class HelperServicesController : ApiControllerBase
{
    /// <summary>
    /// Generates a schedule for the created doctors.
    /// </summary>
    /// <param name="generationRequest">The object containing the id of the doctor and the date of doctor's first day.</param>
    /// <returns>Status code indicating whether request succeeded.</returns>
    [HttpPost]
    public IActionResult GenerateSchedule([FromBody] ScheduleGenerationRequest generationRequest)
    {
        throw new NotImplementedException();
    }
}