using backend.Business.Company.GetJobsCompany;
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
    
    [HttpGet("jobs")]
    public async Task<IActionResult> GetJobs([FromQuery] string Id)
    {
        
        //TODO: its possible to define a dto also for the parameter of the query
        var response = await _mediator.Send(new GetJobsCompanyQuery(Id));

        return Ok(response);
    }
}