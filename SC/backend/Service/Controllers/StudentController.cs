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

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var res = await _mediator.Send(new GetStudentQuery(Id: id));
        
        return Ok(res);
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> UpdateStudent([FromBody] StudentDto student)
    {
        var res = await _mediator.Send(new UpdateStudentCommand(student));

        return Ok(res);
    }
    
    [Authorize(Policy = "StudentAccessPolicy")]
    [HttpPost("cv/{id}")]
    public async Task<IActionResult> LoadCvStudent([FromForm] UploadCvFileDto dto, int id)
    {
        var res = await _mediator.Send(new LoadCvCommand(dto, id));

        return Ok(res);
    }
}