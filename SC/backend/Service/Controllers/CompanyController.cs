using backend.Business.Company.AddJobCompany;
using backend.Business.Company.GetCompanyDetail;
using backend.Business.Company.GetJobsCompany;
using backend.Business.Company.UpdateCompanyProfile;
using backend.Service.Contracts.Company;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
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

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile([FromQuery] string Id)
    {
        var response = await _mediator.Send(new GetCompanyDetailQuery(Id));

        return Ok(response);
    }
    
    [HttpPost("update-profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateCompanyProfileDto dto)
    {
        var response = await _mediator.Send(new UpdateCompanyProfileCommand(dto));

        return Ok(response);
    }

    [HttpGet("jobs")]
    public async Task<IActionResult> GetJobs([FromQuery] string Id)
    {
        
        //TODO: its possible to define a dto also for the parameter of the query
        var response = await _mediator.Send(new GetJobsCompanyQuery(Id));

        return Ok(response);
    }
    
    [HttpPost("add-job")]
    public async Task<IActionResult> AddJob([FromBody] AddJobCompanyDto dto)
    {
        var response = await _mediator.Send(new AddJobCommand(dto));

        return Ok(response);
    }
}