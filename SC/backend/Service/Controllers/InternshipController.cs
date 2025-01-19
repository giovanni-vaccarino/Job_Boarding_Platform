using backend.Business.Internship.AnswerQuestionsUseCase;
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
    [HttpPost("apply-internship/{studentId}?")]
    public async Task<IActionResult> ApplyToInternship([FromRoute] int studentId, [FromQuery] int internshipId)
    {
        var response = await _mediator.Send(new ApplyToInternshipCommand(studentId, internshipId));
        
        return Ok(response);
    }
    
    [Authorize(Policy = "CompanyAccessPolicy")]
    [HttpGet("{internshipId}/applications")]
    public async Task<IActionResult> GetInternshipApplicants([FromRoute] int internshipId, [FromQuery] int companyId)
    {
        var response = await _mediator.Send(new GetInternshipDetailsQuery(internshipId));
        
        return Ok(response);
    }
    
    [Authorize(Policy = "CompanyAccessPolicy")]
    [HttpPatch("applications/{applicationId}")]
    public async Task<IActionResult> UpdateStatusApplication([FromRoute] int applicationId, 
        [FromBody] UpdateStatusApplicationDto dto, [FromQuery] int companyId)
    {
        var response = await _mediator.Send(new UpdateStatusApplicationCommand(applicationId, dto));
        
        return Ok(response);
    }
    
    [Authorize(Policy = "StudentAccessPolicy")]
    [HttpPost("applications/{applicationId}")]
    public async Task<IActionResult> AnswerApplicationQuestions([FromRoute] int applicationId, 
        [FromBody] AnswerQuestionsDto dto, [FromQuery] int studentId)
    {
        var response = await _mediator.Send(new AnswerQuestionsCommand(applicationId, dto));
        
        return Ok(response);
    }
}