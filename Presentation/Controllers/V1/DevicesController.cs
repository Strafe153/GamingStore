using Application.Devices.Commands.Create;
using Application.Devices.Commands.Delete;
using Application.Devices.Commands.Update;
using Application.Devices.Queries;
using Application.Devices.Queries.GetAll;
using Application.Devices.Queries.GetById;
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

using ProblemDetails = Domain.Shared.ProblemDetails.ProblemDetails;

namespace Presentation.Controllers.V1;

[ApiController]
[Route("api/v{version:apiVersion}/devices")]
[Authorize(Policy = AuthorizationConstants.AdminOnlyPolicy)]
[ApiVersion(ApiVersioningConstants.V1)]
[EnableRateLimiting(RateLimitingConstants.TokenBucket)]
public class DevicesController : ControllerBase
{
	private readonly ISender _sender;
	private readonly IMapper _mapper;

	public DevicesController(
		ISender sender,
		IMapper mapper)
	{
		_sender = sender;
		_mapper = mapper;
	}

	/// <summary>
	/// Gets a page of devices
	/// </summary>
	/// <param name="pageParameters">Parameters containing number, size and company name</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>A page of devices</returns>
	/// <response code="200">Returns a page of devices</response>
	[HttpGet]
	[AllowAnonymous]
	[Consumes(MediaTypeNames.Application.Json)]
	[Produces(MediaTypeNames.Application.Json)]
	[ProducesResponseType(typeof(PagedModel<GetDeviceResponse>), StatusCodes.Status200OK)]
	public async Task<ActionResult<PagedModel<GetDeviceResponse>>> Get(
		[FromQuery] DeviceParameters pageParameters,
		CancellationToken cancellationToken)
	{
		var query = new GetAllDevicesQuery(pageParameters.PageNumber, pageParameters.PageSize, pageParameters.Company);

		var devices = await _sender.Send(query, cancellationToken);
		var deviceResponses = _mapper.Map<PagedModel<GetDeviceResponse>>(devices);

		return Ok(deviceResponses);
	}

	/// <summary>
	/// Gets a device by the specified identifier
	/// </summary>
	/// <param name="id">A device's identifier</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>A device by the specified identifier</returns>
	/// <response code="200">Returns a company by the specified identifier</response>
	/// <response code="404">Returns if a company does not exist</response>
	[HttpGet("{id:int:min(1)}")]
	[AllowAnonymous]
	[Consumes(MediaTypeNames.Application.Json)]
	[Produces(MediaTypeNames.Application.Json)]
	[ProducesResponseType(typeof(GetDeviceResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetDeviceResponse>> Get([FromRoute] int id, CancellationToken cancellationToken)
	{
		var query = new GetDeviceByIdQuery(id);

		var device = await _sender.Send(query, cancellationToken);
		var deviceResponse = _mapper.Map<GetDeviceResponse>(device);

		return Ok(deviceResponse);
	}

	/// <summary>
	/// Creates a device
	/// </summary>
	/// <param name="request">A request to create a device</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>The newly created device</returns>
	/// <response code="201">Returns if a device is created successfully</response>
	/// <response code="400">Returns if the validations are not passed</response>
	/// <response code="401">Returns if a user is unauthorized</response>
	[HttpPost]
	[Consumes(MediaTypes.FormData)]
	[Produces(MediaTypeNames.Application.Json)]
	[ProducesResponseType(typeof(GetDeviceResponse), StatusCodes.Status201Created)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
	public async Task<ActionResult<GetDeviceResponse>> Create(
		[FromForm] CreateDeviceRequest request,
		CancellationToken cancellationToken)
	{
		var command = _mapper.Map<CreateDeviceCommand>(request);

		var device = await _sender.Send(command, cancellationToken);
		var deviceResponse = _mapper.Map<GetDeviceResponse>(device);

		return CreatedAtAction(nameof(Get), new { deviceResponse.Id }, deviceResponse);
	}

	/// <summary>
	/// Updates a device by the specified identifier
	/// </summary>
	/// <param name="id">A device's identifier</param>
	/// <param name="request">A request to update a device</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>No content</returns>
	/// <response code="204">Returns if a device is updated successfully</response>
	/// <response code="400">Returns if the validations are not passed</response>
	/// <response code="401">Returns if a user is unauthorized</response>
	/// <response code="404">Returns if device does not exist</response>
	[HttpPut("{id:int:min(1)}")]
	[Consumes(MediaTypes.FormData)]
	[Produces(MediaTypeNames.Application.Json)]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
	public async Task<ActionResult> Update(
		[FromRoute] int id,
		[FromForm] UpdateDeviceRequest request,
		CancellationToken cancellationToken)
	{
		var query = new GetDeviceByIdQuery(id);
		var device = await _sender.Send(query, cancellationToken);

		var command = _mapper.Map<UpdateDeviceCommand>(request) with { Device = device };
		await _sender.Send(command, cancellationToken);

		return NoContent();
	}

	/// <summary>
	/// Deletes a device by the specified identifier
	/// </summary>
	/// <param name="id">A device's identifier</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>No content</returns>
	/// <response code="204">Returns if the device is updated successfully</response>
	/// <response code="401">Returns if a user is unauthorized</response>
	/// <response code="404">Returns if device does not exist</response>
	[HttpDelete("{id:int:min(1)}")]
	[Consumes(MediaTypeNames.Application.Json)]
	[Produces(MediaTypeNames.Application.Json)]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
	public async Task<ActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
	{
		var query = new GetDeviceByIdQuery(id);
		var device = await _sender.Send(query, cancellationToken);

		var command = new DeleteDeviceCommand(device);
		await _sender.Send(command, cancellationToken);

		return NoContent();
	}
}
