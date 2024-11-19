using backend.Business.Auth.LoginUseCase;
using backend.Business.Auth.RegisterUseCase;
using backend.Service.Contracts.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace backend.Service.Controllers;

[ApiController]
[Route("api/authentication")]
public class AuhtenticationController : ControllerBase
{
    private readonly ISender _mediator;
    
    public AuhtenticationController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
    {
        var response = await _mediator.Send(new RegisterCommand(dto));

        return Ok(response);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
    {
        var response = await _mediator.Send(new LoginCommand(dto));

        return Ok(response);
    }
}