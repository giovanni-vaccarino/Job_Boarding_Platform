using backend.Business.Company.AddJobCompany;
using backend.Business.Company.GetCompanyDetailUseCase;
using backend.Business.Company.GetJobsCompany;
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

    [HttpGet("jobs")]
    public async Task<IActionResult> GetJobs([FromQuery] int id)
    {
        //TODO: its possible to define a dto also for the parameter of the query
        var response = await _mediator.Send(new GetJobsCompanyQuery(id));

        return Ok(response);
    }
    
    [HttpPost("add-job")]
    public async Task<IActionResult> AddJob([FromBody] AddJobCompanyDto dto)
    {
        var response = await _mediator.Send(new AddJobCommand(dto));

        return Ok(response);
    }
}