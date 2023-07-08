using Application.Devices.Commands.Create;
using Application.Devices.Commands.Delete;
using Application.Devices.Commands.Update;
using Application.Devices.Queries;
using Application.Devices.Queries.GetAll;
using Application.Devices.Queries.GetById;
using AutoMapper;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Presentation.Controllers.V1;

[ApiController]
[Route("api/v{version:apiVersion}/devices")]
[Authorize(Policy = "AdminOnly")]
[ApiVersion("1.0")]
[EnableRateLimiting("tokenBucket")]
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
    public async Task<ActionResult<PaginatedModel<GetDeviceResponse>>> GetAsync(
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
    public async Task<ActionResult<GetDeviceResponse>> GetAsync([FromRoute] int id, CancellationToken cancellationToken)
    {
        var query = new GetDeviceByIdQuery(id);

        var device = await _sender.Send(query, cancellationToken);
        var deviceResponse = _mapper.Map<GetDeviceResponse>(device);

        return Ok(deviceResponse);
    }

    [HttpPost]
    public async Task<ActionResult<GetDeviceResponse>> CreateAsync(
        [FromForm] CreateDeviceRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreateDeviceCommand>(request);

        var device = await _sender.Send(command, cancellationToken);
        var deviceResponse = _mapper.Map<GetDeviceResponse>(device);

        return CreatedAtAction(nameof(GetAsync), new { deviceResponse.Id }, deviceResponse);
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<ActionResult> UpdateAsync(
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
    public async Task<ActionResult> DeleteAsync([FromRoute] int id, CancellationToken cancellationToken)
    {
        var query = new GetDeviceByIdQuery(id);
        var device = await _sender.Send(query, cancellationToken);

        var command = new DeleteDeviceCommand(device);
        await _sender.Send(command, cancellationToken);

        return NoContent();
    }
}
