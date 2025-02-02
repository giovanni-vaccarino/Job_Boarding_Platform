using backend.Business.Match.AcceptMatchUseCase;
using backend.Business.Match.GetMatchesUseCase;
using backend.Business.Match.InviteStudentUseCase;
using backend.Shared.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Service.Controllers;

[ApiController]
[Route("api/matches")]
public class MatchController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public MatchController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [Authorize(Policy = "StudentAccessPolicy")]
    [HttpGet("student/{studentId}")]
    public async Task<IActionResult> GetStudentMatches([FromRoute] int studentId)
    {
        var response = await _mediator.Send(new GetMatchesQuery(studentId, ProfileType.Student));
        
        return Ok(response);
    }
    
    [Authorize(Policy = "CompanyAccessPolicy")]
    [HttpGet("company/{companyId}")]
    public async Task<IActionResult> GetCompanyMatches([FromRoute] int companyId)
    {
        var response = await _mediator.Send(new GetMatchesQuery(companyId, ProfileType.Company));
        
        return Ok(response);
    }
    
    [Authorize(Policy = "CompanyAccessPolicy")]
    [HttpPatch("{matchId}")]
    public async Task<IActionResult> InviteStudent([FromRoute] int matchId, [FromQuery] int companyId)
    {
        await _mediator.Send(new InviteStudentCommand(matchId));
        
        return Ok();
    }
    
    [Authorize(Policy = "StudentAccessPolicy")]
    [HttpPost("{matchId}")]
    public async Task<IActionResult> AcceptMatch([FromRoute] int matchId, [FromQuery] int studentId)
    {
        await _mediator.Send(new AcceptMatchCommand(matchId));
        
        return Ok();
    }
}