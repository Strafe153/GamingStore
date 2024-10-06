using Application.Abstractions.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Nest;

namespace Presentation.Controllers.V1;

[ApiController]
[Route("api/v{version:apiVersion}/logs")]
[Authorize(Policy = "AdminOnly")]
[ApiVersion("1.0")]
[EnableRateLimiting("tokenBucket")]
public class LogsController : ControllerBase
{
    private readonly IElasticSearchService _elasticService;

    public LogsController(IElasticSearchService elasticService)
    {
        _elasticService = elasticService;
    }

    [HttpGet]
    public ActionResult<IReadOnlyCollection<Log>> Get([FromQuery] string query) =>
        Ok(_elasticService.Get(query));
    //[HttpGet]
    //public ActionResult<IReadOnlyCollection<object>> Get([FromQuery] string query) =>
    //    Ok(_elasticService.Get(query));
}
