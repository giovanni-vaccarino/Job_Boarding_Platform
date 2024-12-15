using backend.Business.Assets.GetAssetUseCase;
using backend.Data;
using backend.Shared.MatchingBackgroundService;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Service.Controllers;

[ApiController]
[Route("api/assets")]
public class AssetsController : ControllerBase
{
    private readonly ISender _mediator;
    private readonly IJobQueue _jobQueue;
    private readonly IInternshipMatchingTaskFactory _taskFactory;
    
    public AssetsController(ISender mediator, IJobQueue jobQueue, IInternshipMatchingTaskFactory taskFactory)
    {
        _mediator = mediator;
        _taskFactory = taskFactory;
        _jobQueue = jobQueue;
    }
    
    [Authorize(Policy = "StudentOrCompanyAccessPolicy")]
    [HttpGet("{studentId}")]
    public async Task<IActionResult> GetAssets([FromRoute] int studentId, [FromQuery] int? companyId)
    {
        var assets = await _mediator.Send(new GetAssetQuery(studentId, companyId));
        
        return assets;
    }
    
    [HttpGet("test")]
    public async Task<IActionResult> Test()
    {
        //TODO endpoint just for matching testing reasons. Remove once matching is implemented
        var task = _taskFactory.Create(4);
        _jobQueue.EnqueueJob(task);

        return Ok();
    }
}