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
    
    public AssetsController(ISender mediator)
    {
        _mediator = mediator;
    }
    
    [Authorize(Policy = "StudentOrCompanyAccessPolicy")]
    [HttpGet("{studentId}")]
    public async Task<IActionResult> GetAssets([FromRoute] int studentId, [FromQuery] int? companyId)
    {
        var assets = await _mediator.Send(new GetAssetQuery(studentId, companyId));
        
        return assets;
    }
}