using Innowise.Clinic.Scheduling.Api.Controllers.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Innowise.Clinic.Scheduling.Api.Controllers;

/// <summary>
///  Manages timeslots necessary for making appointments.
/// </summary>
public class TimeSlotsController : ApiControllerBase
{
    [HttpGet]
    public IActionResult GetFreeTimeSlots()
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public IActionResult ReserveTimeSlot()
    {
        throw new NotImplementedException();
    }
    
    [HttpPut("appointments/{id:guid}")]
    public IActionResult EditTimeSlot()
    {
        throw new NotImplementedException();
    }
    
}