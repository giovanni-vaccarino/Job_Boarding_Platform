using backend.Business.Auth.LoginUseCase;
using backend.Business.Auth.LogoutUseCase;
using backend.Business.Auth.RefreshUseCase;
using backend.Business.Auth.RegisterUseCase;
using backend.Business.Auth.SendForgotPasswordUseCase;
using backend.Business.Auth.SendVerificationMailUseCase;
using backend.Business.Auth.UpdatePasswordUseCase;
using backend.Business.Auth.VerifyMailUseCase;
using backend.Service.Contracts.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Service.Controllers;

[ApiController]
[Route("api/authentication")]
public class AuthenticationController : ControllerBase
{
    private readonly ISender _mediator;
    
    public AuthenticationController(ISender mediator)
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
    
    [Authorize]
    [HttpPost("send-verification-email")]
    public async Task<IActionResult> SendVerificationMail(SendVerificationMailDto dto)
    {
        await _mediator.Send(new SendVerificationMailCommand(dto.Email));

        return Ok();
    }
    
    [HttpPost("verify-email")]
    public async Task<IActionResult> VerifyMail(VerifyMailDto dto)
    {
        await _mediator.Send(new VerifyMailCommand(dto.VerificationToken));

        return Ok();
    }
    
    [HttpPost("send-reset-password")]
    public async Task<IActionResult> SendResetPassword(SendVerificationMailDto dto)
    {
        await _mediator.Send(new SendForgotPasswordCommand(dto.Email));

        return Ok();
    }
    
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(UpdatePasswordDto dto)
    {
        await _mediator.Send(new UpdatePasswordCommand(dto.Token, dto.Password));

        return Ok();
    }
}