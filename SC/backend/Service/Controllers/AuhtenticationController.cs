using backend.Dtos.Auth;
using backend.Service.Auth.LoginUseCase;
using backend.Service.Auth.RegisterUseCase;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.Internship;

[ApiController]
[Route("api/[controller]")]
public class AuhtenticationController : ControllerBase
{
    private readonly ISender _mediator;
    
    public AuhtenticationController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
    {
        var response = await _mediator.Send(new RegisterCommand(dto));

        return Ok(response);
    }
    
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
    {
        var response = await _mediator.Send(new LoginCommand(dto));

        return Ok(response);
    }
}