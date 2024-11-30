using backend.Business.Student.GetStudentUseCase;
using backend.Business.Student.LoadCvUseCase;
using backend.Business.Student.UpdateStudentUseCase;
using backend.Service.Contracts.Student;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Service.Controllers;

[ApiController]
[Route("api/student")]
public class StudentController : ControllerBase
{
    private readonly ISender _mediator;
    
    public StudentController(ISender mediator)
    {
        _mediator = mediator;
    }

    [Authorize(Policy = "StudentAccessPolicy")]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var res = await _mediator.Send(new GetStudentQuery(Id: id));
        
        return Ok(res);
    }

    [Authorize(Policy = "StudentAccessPolicy")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStudent([FromRoute] int id, [FromBody] UpdateStudentDto updateStudentDto)
    {
        var res = await _mediator.Send(new UpdateStudentCommand(id, updateStudentDto));

        return Ok(res);
    }
    
    [Authorize(Policy = "StudentAccessPolicy")]
    [HttpPost("cv/{id}")]
    public async Task<IActionResult> LoadCvStudent([FromForm] LoadCvFileDto dto, int id)
    {
        var res = await _mediator.Send(new LoadCvCommand(dto, id));

        return Ok(res);
    }
}