using backend.Business.Student.GetApplicationsUseCase;
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
    [HttpGet("{studentId}")]
    public async Task<IActionResult> Get(int studentId)
    {
        var res = await _mediator.Send(new GetStudentQuery(Id: studentId));
        
        return Ok(res);
    }

    [Authorize(Policy = "StudentAccessPolicy")]
    [HttpPut("{studentId}")]
    public async Task<IActionResult> UpdateStudent([FromRoute] int studentId, [FromBody] UpdateStudentDto updateStudentDto)
    {
        var res = await _mediator.Send(new UpdateStudentCommand(studentId, updateStudentDto));

        return Ok(res);
    }
    
    [Authorize(Policy = "StudentAccessPolicy")]
    [HttpPost("cv/{studentId}")]
    public async Task<IActionResult> LoadCvStudent([FromForm] LoadCvFileDto dto, [FromRoute] int studentId)
    {
        var res = await _mediator.Send(new LoadCvCommand(dto, studentId));

        return Ok(res);
    }
    
    //[Authorize(Policy = "StudentAccessPolicy")]
    [HttpGet("{studentId}/applications")]
    public async Task<IActionResult> GetApplications(int studentId)
    {
        var response = await _mediator.Send(new GetApplicationsQuery(studentId));
        
        return Ok(response);
    }
}