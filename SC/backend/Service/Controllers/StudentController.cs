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
    [HttpPost]
    public async Task<IActionResult> LoadCvStudent([FromBody] UploadCvFileDto dto)
    {
        var res = await _mediator.Send(new LoadCvCommand(dto));

        return Ok(res);
    }
    
    [Authorize(Policy = "StudentAccessPolicy")]
    [HttpGet("prova/{id}")]
    public async Task<IActionResult> Test()
    {
        return Ok();
    }
    
    [Authorize(Policy = "CompanyAccessPolicy")]
    [HttpGet("prova-company/{id}")]
    public async Task<IActionResult> TestCompany()
    {
        return Ok();
    }
}