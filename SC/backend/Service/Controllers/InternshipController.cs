using backend.Business.Internship.GetInternshipDetails;
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
    
    [HttpGet]
    public async Task<IActionResult> GetInternships()
    {
        var internships = await _mediator.Send(new GetInternshipQuery());
        
        return Ok(internships);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetInternshipDetails(int id)
    {
        var internship = await _mediator.Send(new InternshipDetailsCommand(id));
        
        return Ok(internship);
    }
    
}