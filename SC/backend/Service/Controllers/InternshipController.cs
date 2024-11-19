using backend.Business.Internship.GetInternshipUseCase;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Service.Controllers;

[ApiController]
[Route("api/internship")]
public class InternshipController : ControllerBase
{
    private readonly ISender _mediator;
    
    public InternshipController(ISender mediator)
    {
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetInternships()
    {
        var internships = await _mediator.Send(new GetInternshipQuery());
        
        return Ok(internships);
    }
}