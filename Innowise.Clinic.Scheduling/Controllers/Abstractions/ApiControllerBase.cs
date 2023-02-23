using Microsoft.AspNetCore.Mvc;

namespace Innowise.Clinic.Scheduling.Controllers.Abstractions;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class ApiControllerBase : ControllerBase
{
    
}