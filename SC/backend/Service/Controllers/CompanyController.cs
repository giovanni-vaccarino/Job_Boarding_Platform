using backend.Business.Company.AddInternshipUseCase;
using backend.Business.Company.AddQuestionUseCase;
using backend.Business.Company.GetCompanyDetailUseCase;
using backend.Business.Company.GetInternshipsUseCase;
using backend.Business.Company.GetQuestionsUseCase;
using backend.Business.Company.UpdateCompanyProfileUseCase;
using backend.Business.Company.UpdateInternshipUseCase;
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
    [HttpGet("{companyId}")]
    public async Task<IActionResult> GetProfile([FromRoute] int companyId)
    {
        var response = await _mediator.Send(new GetCompanyDetailQuery(companyId));

        return Ok(response);
    }
    
    [Authorize("CompanyAccessPolicy")]
    [HttpPut("{companyId}")]
    public async Task<IActionResult> UpdateProfile([FromRoute] int companyId, [FromBody] UpdateCompanyProfileDto dto)
    {
        var response = await _mediator.Send(new UpdateCompanyProfileCommand(companyId, dto));

        return Ok(response);
    }

    [Authorize("CompanyAccessPolicy")]
    [HttpGet("{companyId}/internships")]
    public async Task<IActionResult> GetInternships([FromRoute] int companyId, [FromQuery] int? internshipId)
    {
        var response = await _mediator.Send(new GetInternshipsQuery(companyId, internshipId));

        return Ok(response);
    }
    
    [Authorize("CompanyAccessPolicy")]
    [HttpPost("{companyId}/internships")]
    public async Task<IActionResult> AddInternship([FromRoute] int companyId,[FromBody] AddInternshipDto dto)
    {
        var response = await _mediator.Send(new AddInternshipCommand(companyId, dto));

        return Ok(response);
    }
    
    [Authorize("CompanyAccessPolicy")]
    [HttpPut("{companyId}/internships/{internshipId}")]
    public async Task<IActionResult> UpdateInternship([FromRoute] int companyId, [FromRoute] int internshipId, [FromBody] UpdateInternshipDto dto)
    {
        var response = await _mediator.Send(new UpdateInternshipCommand(companyId, internshipId, dto));
        
        return Ok(response);
    }
    
    [Authorize("CompanyAccessPolicy")]
    [HttpGet("{companyId}/questions")]
    public async Task<IActionResult> GetQuestions([FromRoute] int companyId)
    {
        var response = await _mediator.Send(new GetQuestionsQuery(companyId));
        
        return Ok(response);
    }
    
    [Authorize("CompanyAccessPolicy")]
    [HttpPost("{companyId}/questions")]
    public async Task<IActionResult> AddQuestion([FromRoute] int companyId, [FromBody] AddQuestionDto dto)
    {
        var response = await _mediator.Send(new AddQuestionCommand(companyId, dto));
        
        return Ok(response);
    }
}