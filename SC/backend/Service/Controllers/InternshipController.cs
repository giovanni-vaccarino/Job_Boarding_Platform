using backend.Service.Internship.GetInternshipUseCase;
using backend.Services.Internship.UseCases.AddInternshipUseCase;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.Internship;

[ApiController]
[Route("api/[controller]")]
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