using Innowise.Clinic.Scheduling.Api.Controllers.Abstractions;
using Innowise.Clinic.Scheduling.Api.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Innowise.Clinic.Scheduling.Api.Controllers;

/// <summary>
/// Lists shifts and their configurations.
/// </summary>
public class ShiftsController : ApiControllerBase
{
    /// <summary>
    /// Lists all shifts that are registered in the system.
    /// </summary>
    /// <returns>A list of shifts.</returns>
    [HttpGet]
    [Authorize(Roles = "Doctor,Receptionist")]
    public IActionResult GetShifts()
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Creates a new shift available for doctors.
    /// </summary>
    /// <returns>Id of the created shift.</returns>
    [HttpPost]
    [Authorize(Roles = "Receptionist")]
    public IActionResult CreateShift([FromBody] ShiftDto newShiftInfo)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Updates the existing shift configuration.
    /// </summary>
    /// <param name="id">Id of the shift to be updated.</param>
    /// <param name="updatedShiftInfo">New shift info.</param>
    /// <returns>Status code indicating whether request succeeded.</returns>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Receptionist")]
    public IActionResult EditShift([FromRoute] Guid id, [FromBody] ShiftDto updatedShiftInfo)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Deletes the shift by its id.
    /// </summary>
    /// <param name="id">Id of the shift to be removed</param>
    /// <returns>Status code indicating whether request succeeded.</returns>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Receptionist")]
    public IActionResult DeleteShift([FromRoute] Guid id)
    {
        throw new NotImplementedException();
    }
}