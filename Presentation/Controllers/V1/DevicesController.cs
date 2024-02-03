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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

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

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<PaginatedModel<GetDeviceResponse>>> Get(
        [FromQuery] DevicePageParameters pageParameters,
        CancellationToken cancellationToken)
    {
        var query = new GetAllDevicesQuery(pageParameters.PageNumber, pageParameters.PageSize, pageParameters.Company);

        var devices = await _sender.Send(query, cancellationToken);
        var deviceResponses = _mapper.Map<PaginatedModel<GetDeviceResponse>>(devices);

        return Ok(deviceResponses);
    }

    [HttpGet("{id:int:min(1)}")]
    [AllowAnonymous]
    public async Task<ActionResult<GetDeviceResponse>> Get([FromRoute] int id, CancellationToken cancellationToken)
    {
        var query = new GetDeviceByIdQuery(id);

        var device = await _sender.Send(query, cancellationToken);
        var deviceResponse = _mapper.Map<GetDeviceResponse>(device);

        return Ok(deviceResponse);
    }

    [HttpPost]
    public async Task<ActionResult<GetDeviceResponse>> Create(
        [FromForm] CreateDeviceRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreateDeviceCommand>(request);

        var device = await _sender.Send(command, cancellationToken);
        var deviceResponse = _mapper.Map<GetDeviceResponse>(device);

        return CreatedAtAction(nameof(Get), new { deviceResponse.Id }, deviceResponse);
    }

    [HttpPut("{id:int:min(1)}")]
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

    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var query = new GetDeviceByIdQuery(id);
        var device = await _sender.Send(query, cancellationToken);

        var command = new DeleteDeviceCommand(device);
        await _sender.Send(command, cancellationToken);

        return NoContent();
    }
}
