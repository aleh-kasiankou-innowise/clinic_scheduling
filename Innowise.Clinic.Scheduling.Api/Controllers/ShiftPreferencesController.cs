using Innowise.Clinic.Scheduling.Persistence.Models;
using Innowise.Clinic.Scheduling.Services.ShiftService.Interfaces;
using Innowise.Clinic.Shared.BaseClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Innowise.Clinic.Scheduling.Api.Controllers;

/// <summary>
/// Manages doctors shift preferences
/// </summary>
public class ShiftPreferencesController : ApiControllerBase
{
    private readonly IShiftService _shiftService;

    public ShiftPreferencesController(IShiftService shiftService)
    {
        _shiftService = shiftService;
    }

    /// <summary>
    /// Displays doctor's shift preference.
    /// </summary>
    /// <returns>Info about preferred shift.</returns>
    [HttpGet("preferences/doctors/{id:guid}")]
    [Authorize(Roles = "Doctor,Receptionist")]
    public async Task<ActionResult<ShiftPreference>> GetShiftPreference([FromRoute] Guid id)
    {
        return Ok(await _shiftService.GetShiftPreferenceAsync(id));
    }
    
    /// <summary>
    /// Sets doctor's shift preference.
    /// </summary>
    /// <returns>The id of the created preference.</returns>
    [HttpPost("preferences/doctors/{id:guid}")]
    [Authorize(Roles = "Doctor,Receptionist")]
    public async Task<ActionResult<ShiftPreference>> SetShiftPreference([FromRoute] Guid id, [FromBody] Guid shiftId)
    {
        return Ok(await _shiftService.SetShiftPreferenceAsync(id, shiftId));
    }
}