using backend.Business.Feedback.AddInternshipFeedbackUseCase;
using backend.Business.Feedback.AddPlatformFeedbackUseCase;
using backend.Service.Contracts.Feedback;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace backend.Service.Controllers;

[ApiController]
[Route("api/feedback")]
public class FeedbackController : ControllerBase
{
    private readonly ISender _mediator;
    
    public FeedbackController(ISender mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("platform")]
    public async Task<IActionResult> AddPlatformFeedback([FromBody] AddPlatformFeedbackDto dto)
    {
        var response = await _mediator.Send(new AddPlatformFeedbackCommand(dto));
        
        return Ok(response);
    }
    
    [HttpPost("internship")]
    public async Task<IActionResult> AddInternshipFeedback([FromBody] AddInternshipFeedbackDto dto)
    {
        var response = await _mediator.Send(new AddInternshipFeedbackCommand(dto));
        
        return Ok(response);
    }
}