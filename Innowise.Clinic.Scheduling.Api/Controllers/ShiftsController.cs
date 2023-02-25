using Innowise.Clinic.Scheduling.Api.Controllers.Abstractions;
using Innowise.Clinic.Scheduling.Persistence.Models;
using Innowise.Clinic.Scheduling.Services.Dto;
using Innowise.Clinic.Scheduling.Services.ShiftService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Innowise.Clinic.Scheduling.Api.Controllers;

/// <summary>
/// Lists shifts and their configurations.
/// </summary>
public class ShiftsController : ApiControllerBase
{
    private readonly IShiftService _shiftService;

    /// <inheritdoc />
    public ShiftsController(IShiftService shiftService)
    {
        _shiftService = shiftService;
    }

    /// <summary>
    /// Lists all shifts that are registered in the system.
    /// </summary>
    /// <returns>A list of shifts.</returns>
    [HttpGet]
    [Authorize(Roles = "Doctor,Receptionist")]
    public async Task<ActionResult<IEnumerable<Shift>>> GetShifts()
    {
        return Ok(await _shiftService.GetShifts());
    }
    
    /// <summary>
    /// Displays info about shift by its id.
    /// </summary>
    /// <returns>Info about shift.</returns>
    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Doctor,Receptionist")]
    public async Task<ActionResult<Shift>> GetShift(Guid id)
    {
        return Ok(await _shiftService.GetShift(id));
    }
    
    /// <summary>
    /// Displays doctors shift preference.
    /// </summary>
    /// <returns>Info about preferred shift.</returns>
    [HttpGet("preferences/doctors/{id:guid}")]
    [Authorize(Roles = "Doctor,Receptionist")]
    public async Task<ActionResult<ShiftPreference>> GetShiftPreference(Guid id)
    {
        return Ok(await _shiftService.GetShiftPreference(id));
    }
    
    /// <summary>
    /// Creates a new shift available for doctors.
    /// </summary>
    /// <returns>Id of the created shift.</returns>
    [HttpPost]
    [Authorize(Roles = "Receptionist")]
    public async Task<ActionResult<Guid>> CreateShift([FromBody] ShiftDto newShiftInfo)
    {
        return Ok((await _shiftService.CreateShift(newShiftInfo)).ToString());
    }

    /// <summary>
    /// Updates the existing shift configuration.
    /// </summary>
    /// <param name="id">Id of the shift to be updated.</param>
    /// <param name="updatedShiftInfo">New shift info.</param>
    /// <returns>Status code indicating whether request succeeded.</returns>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Receptionist")]
    public async Task<IActionResult> EditShift([FromRoute] Guid id, [FromBody] ShiftDto updatedShiftInfo)
    {
        await _shiftService.UpdateShift(id, updatedShiftInfo);
        return Ok();
    }

    /// <summary>
    /// Deletes the shift by its id.
    /// </summary>
    /// <param name="id">Id of the shift to be removed</param>
    /// <returns>Status code indicating whether request succeeded.</returns>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Receptionist")]
    public async Task<IActionResult> DeleteShift([FromRoute] Guid id)
    {
        await _shiftService.DeleteShift(id);
        return NoContent();
    }
}