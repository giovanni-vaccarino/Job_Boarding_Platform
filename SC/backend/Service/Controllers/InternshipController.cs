using backend.Business.Internship.ApplyToInternshipUseCase;
using backend.Business.Internship.GetInternshipDetailsUseCase;
using backend.Business.Internship.GetInternshipUseCase;
using MediatR;
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
        var response = await _mediator.Send(new GetInternshipQuery());
        
        return Ok(response);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetInternshipDetails([FromRoute] int id)
    {
        var response = await _mediator.Send(new GetInternshipDetailsQuery(id));
        
        return Ok(response);
    }
    
    [HttpPost("apply-internship/{id}/{internshipId}")]
    public async Task<IActionResult> ApplyToInternship([FromRoute] int id, [FromRoute] int internshipId)
    {
        var response = await _mediator.Send(new ApplyToInternshipCommand(id, internshipId));
        
        return Ok(response);
    }
}