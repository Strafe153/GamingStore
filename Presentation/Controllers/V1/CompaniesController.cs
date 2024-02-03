using Application.Companies.Commands.Create;
using Application.Companies.Commands.Delete;
using Application.Companies.Commands.Update;
using Application.Companies.Queries;
using Application.Companies.Queries.GetAll;
using Application.Companies.Queries.GetById;
using AutoMapper;
using Domain.Shared.Constants;
using Domain.Shared.PageParameters;
using Domain.Shared.Paging;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Presentation.Controllers.V1;

[ApiController]
[Route("api/v{version:apiVersion}/companies")]
[Authorize(Policy = AuthorizationConstants.AdminOnlyPolicy)]
[ApiVersion(ApiVersioningConstants.V1)]
[EnableRateLimiting(RateLimitingConstants.TokenBucket)]
public class CompaniesController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public CompaniesController(
        ISender sender,
        IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<PaginatedModel<GetCompanyResponse>>> Get(
        [FromQuery] PageParameters pageParameters,
        CancellationToken cancellationToken)
    {
        var query = new GetAllCompaniesQuery(pageParameters.PageNumber, pageParameters.PageSize);

        var companies = await _sender.Send(query, cancellationToken);
        var companyResponses = _mapper.Map<PaginatedModel<GetCompanyResponse>>(companies);

        return Ok(companyResponses);
    }

    [HttpGet("{id:int:min(1)}")]
    [AllowAnonymous]
    public async Task<ActionResult<GetCompanyResponse>> Get([FromRoute] int id, CancellationToken cancellationToken)
    {
        var query = new GetCompanyByIdQuery(id);

        var company = await _sender.Send(query, cancellationToken);
        var companyResponse = _mapper.Map<GetCompanyResponse>(company);

        return Ok(companyResponse);
    }

    [HttpPost]
    public async Task<ActionResult<GetCompanyResponse>> Create(
        [FromForm] CreateCompanyRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreateCompanyCommand>(request);

        var company = await _sender.Send(command, cancellationToken);
        var companyResponse = _mapper.Map<GetCompanyResponse>(company);

        return CreatedAtAction(nameof(Get), new { companyResponse.Id }, companyResponse);
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<ActionResult> Update(
        [FromRoute] int id,
        [FromForm] UpdateCompanyRequest request,
        CancellationToken cancellationToken)
    {
        var query = new GetCompanyByIdQuery(id);
        var company = await _sender.Send(query, cancellationToken);

        var command = _mapper.Map<UpdateCompanyCommand>(request) with { Company = company };
        await _sender.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var query = new GetCompanyByIdQuery(id);
        var company = await _sender.Send(query, cancellationToken);

        var command = new DeleteCompanyCommand(company);
        await _sender.Send(command, cancellationToken);

        return NoContent();
    }
}
