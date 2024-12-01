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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Net.Mime;

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

	/// <summary>
	/// Fetches a page of companies
	/// </summary>
	/// <param name="pageParameters">Page parameters containing number and size</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>A page of companies</returns>
	/// <response code="200">Returns a page of companies</response>
	[HttpGet]
	[AllowAnonymous]
	[Consumes(MediaTypeNames.Application.Json)]
	[Produces(MediaTypeNames.Application.Json)]
	[ProducesResponseType(typeof(PagedModel<GetCompanyResponse>), StatusCodes.Status200OK)]
	public async Task<ActionResult<PagedModel<GetCompanyResponse>>> Get(
		[FromQuery] PageParameters pageParameters,
		CancellationToken cancellationToken)
	{
		var query = new GetAllCompaniesQuery(pageParameters.PageNumber, pageParameters.PageSize);

		var companies = await _sender.Send(query, cancellationToken);
		var companyResponses = _mapper.Map<PagedModel<GetCompanyResponse>>(companies);

		return Ok(companyResponses);
	}

	/// <summary>
	/// Fetches a company by the specified identifier
	/// </summary>
	/// <param name="id">A company's identifier</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>A company by the specified identifier</returns>
	/// <response code="200">Returns a company by the specified identifier</response>
	/// <response code="404">Returns if a company does not exist</response>
	[HttpGet("{id:int:min(1)}")]
	[AllowAnonymous]
	[Consumes(MediaTypeNames.Application.Json)]
	[Produces(MediaTypeNames.Application.Json)]
	[ProducesResponseType(typeof(GetCompanyResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetCompanyResponse>> Get([FromRoute] int id, CancellationToken cancellationToken)
	{
		var query = new GetCompanyByIdQuery(id);

		var company = await _sender.Send(query, cancellationToken);
		var companyResponse = _mapper.Map<GetCompanyResponse>(company);

		return Ok(companyResponse);
	}

	/// <summary>
	/// Creates a company
	/// </summary>
	/// <param name="request">A request to create a company</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>The newly created company</returns>
	/// <response code="201">Returns if a company is created successfully</response>
	/// <response code="400">Returns if the validations are not passed</response>
	/// <response code="401">Returns if a user is unauthorized</response>
	[HttpPost]
	[Consumes(MediaTypeNames.Multipart.FormData)]
	[Produces(MediaTypeNames.Application.Json)]
	[ProducesResponseType(typeof(GetCompanyResponse), StatusCodes.Status201Created)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
	public async Task<ActionResult<GetCompanyResponse>> Create(
		[FromForm] CreateCompanyRequest request,
		CancellationToken cancellationToken)
	{
		var command = _mapper.Map<CreateCompanyCommand>(request);

        var company = await _sender.Send(command, cancellationToken);
		var companyResponse = _mapper.Map<GetCompanyResponse>(company);

		return CreatedAtAction(nameof(Get), new { companyResponse.Id }, companyResponse);
	}

	/// <summary>
	/// Updates a company by the specified identifier
	/// </summary>
	/// <param name="id">A company's identifier</param>
	/// <param name="request">A request to update a company</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>No content</returns>
	/// <response code="204">Returns if a company is updated successfully</response>
	/// <response code="400">Returns if the validations are not passed</response>
	/// <response code="401">Returns if a user is unauthorized</response>
	/// <response code="404">Returns if a company does not exist</response>
	[HttpPut("{id:int:min(1)}")]
	[Consumes(MediaTypeNames.Multipart.FormData)]
	[Produces(MediaTypeNames.Application.Json)]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
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

	/// <summary>
	/// Deletes a company by the specified identifier
	/// </summary>
	/// <param name="id">A company's identifier</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>No content</returns>
	/// <response code="204">Returns if a company is updated successfully</response>
	/// <response code="401">Returns if a user is unauthorized</response>
	/// <response code="404">Returns if a company does not exist</response>
	[HttpDelete("{id:int:min(1)}")]
	[Consumes(MediaTypeNames.Application.Json)]
	[Produces(MediaTypeNames.Application.Json)]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
	public async Task<ActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
	{
		var query = new GetCompanyByIdQuery(id);
		var company = await _sender.Send(query, cancellationToken);

		var command = new DeleteCompanyCommand(company);
		await _sender.Send(command, cancellationToken);

		return NoContent();
	}
}
