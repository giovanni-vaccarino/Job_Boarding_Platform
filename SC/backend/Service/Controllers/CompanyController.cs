using backend.Business.Company.AddInternshipUseCase;
using backend.Business.Company.GetCompanyDetailUseCase;
using backend.Business.Company.GetInternshipsUseCase;
using backend.Business.Company.UpdateCompanyProfileUseCase;
using backend.Service.Contracts.Company;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Service.Controllers;

[ApiController]
[Route("api/company")]
public class CompanyController : ControllerBase
{
    private readonly ISender _mediator;
    
    public CompanyController(ISender mediator)
    {
        _mediator = mediator;
    }

    [Authorize("CompanyAccessPolicy")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProfile([FromRoute] int id)
    {
        var response = await _mediator.Send(new GetCompanyDetailQuery(id));

        return Ok(response);
    }
    
    [Authorize("CompanyAccessPolicy")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProfile([FromRoute] int id, [FromBody] UpdateCompanyProfileDto dto)
    {
        var response = await _mediator.Send(new UpdateCompanyProfileCommand(id, dto));

        return Ok(response);
    }

    [Authorize("CompanyAccessPolicy")]
    [HttpGet("{id}/internships")]
    public async Task<IActionResult> GetInternships([FromRoute] int id, [FromQuery] int? internshipId)
    {
        var response = await _mediator.Send(new GetInternshipsQuery(id, internshipId));

        return Ok(response);
    }
    
    [Authorize("CompanyAccessPolicy")]
    [HttpPost("{id}/internships")]
    public async Task<IActionResult> AddInternship([FromRoute] int id,[FromBody] AddInternshipDto dto)
    {
        var response = await _mediator.Send(new AddInternshipCommand(id, dto));

        return Ok(response);
    }
    
    [Authorize("CompanyAccessPolicy")]
    [HttpPut("{id}/internships/{internshipId}")]
    public async Task<IActionResult> UpdateInternship([FromRoute] int id, [FromRoute] int internshipId, [FromBody] AddInternshipDto dto)
    {
        
        return Ok();
    }
    
    [Authorize("CompanyAccessPolicy")]
    [HttpGet("{id}/questions")]
    public async Task<IActionResult> GetQuestions([FromRoute] int id, [FromQuery] int? questionId)
    {
        
        return Ok();
    }
    
    [Authorize("CompanyAccessPolicy")]
    [HttpPost("{id}/questions")]
    public async Task<IActionResult> AddQuestion([FromRoute] int id, [FromBody] string dto)
    {
        
        return Ok();
    }
}