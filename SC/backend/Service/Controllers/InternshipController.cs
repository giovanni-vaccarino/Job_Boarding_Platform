using backend.Business.Internship.ApplyToInternshipUseCase;
using backend.Business.Internship.GetInternshipDetailsUseCase;
using backend.Business.Internship.GetInternshipUseCase;
using backend.Business.Internship.UpdateStatusApplicationUseCase;
using backend.Service.Contracts.Internship;
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
        var response = await _mediator.Send(new GetInternshipQuery());
        
        return Ok(response);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetInternshipDetails([FromRoute] int id)
    {
        var response = await _mediator.Send(new GetInternshipDetailsQuery(id));
        
        return Ok(response);
    }
    
    [Authorize(Policy = "StudentAccessPolicy")]
    [HttpPost("apply-internship/{id}")]
    public async Task<IActionResult> ApplyToInternship([FromRoute] int id, [FromQuery] int internshipId)
    {
        var response = await _mediator.Send(new ApplyToInternshipCommand(id, internshipId));
        
        return Ok(response);
    }
    
    [HttpGet("{internshipId}/applications")]
    public async Task<IActionResult> GetInternshipApplicants([FromRoute] int internshipId)
    {
        var response = await _mediator.Send(new GetInternshipDetailsQuery(internshipId));
        
        return Ok(response);
    }
    
    [HttpPatch("applications/{applicationId}")]
    public async Task<IActionResult> UpdateStatusApplication([FromRoute] int applicationId, [FromBody] UpdateStatusApplicationDto dto)
    {
        var response = await _mediator.Send(new UpdateStatusApplicationCommand(applicationId, dto));
        
        return Ok(response);
    }
    
    [HttpPost("applications/{applicationId}")]
    public async Task<IActionResult> AnswerApplicationQuestions([FromRoute] int applicationId, [FromBody] UpdateStatusApplicationDto dto)
    {
        var response = await _mediator.Send(new UpdateStatusApplicationCommand(applicationId, dto));
        
        return Ok(response);
    }
}