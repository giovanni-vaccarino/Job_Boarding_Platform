using backend.Business.Auth.LoginUseCase;
using backend.Business.Auth.LogoutUseCase;
using backend.Business.Auth.RefreshUseCase;
using backend.Business.Auth.RegisterUseCase;
using backend.Service.Contracts.Auth;
using backend.Shared.EmailService;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Service.Controllers;

[ApiController]
[Route("api/authentication")]
public class AuthenticationController : ControllerBase
{
    private readonly ISender _mediator;
    private readonly EmailService _emailService;
    
    public AuthenticationController(ISender mediator, EmailService emailService)
    {
        _mediator = mediator;
        _emailService = emailService;
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
    
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto dto)
    {
        var response = await _mediator.Send(new RefreshCommand(dto));

        return Ok(response);
    }
    
    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _mediator.Send(new LogoutCommand());

        return Ok();
    }
}