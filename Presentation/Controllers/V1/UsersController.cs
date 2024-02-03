using Application.Users.Commands.ChangePassword;
using Application.Users.Commands.ChangeRole;
using Application.Users.Commands.Delete;
using Application.Users.Commands.Register;
using Application.Users.Commands.Update;
using Application.Users.Queries;
using Application.Users.Queries.GetAll;
using Application.Users.Queries.GetById;
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
[Route("api/v{version:apiVersion}/users")]
[Authorize]
[ApiVersion(ApiVersioningConstants.V1)]
[EnableRateLimiting(RateLimitingConstants.TokenBucket)]
public class UsersController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public UsersController(
        ISender sender,
        IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedModel<GetUserResponse>>> Get(
        [FromQuery] PageParameters pageParameters,
        CancellationToken cancellationToken)
    {
        var query = new GetAllUsersQuery(pageParameters.PageNumber, pageParameters.PageSize);

        var users = await _sender.Send(query, cancellationToken);
        var userResponses = _mapper.Map<PaginatedModel<GetUserResponse>>(users);

        return Ok(userResponses);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult<GetUserResponse>> Get([FromRoute] int id, CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(id);

        var user = await _sender.Send(query, cancellationToken);
        var userResponse = _mapper.Map<GetUserResponse>(user);

        return Ok(userResponse);
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<GetUserResponse>> Register(
        [FromForm] RegisterUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<RegisterUserCommand>(request);

        var user = await _sender.Send(command, cancellationToken);
        var userResponse = _mapper.Map<GetUserResponse>(user);

        return CreatedAtAction(nameof(Get), new { userResponse.Id }, userResponse);
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<ActionResult> Update(
        [FromRoute] int id,
        [FromForm] UpdateUserRequest request,
        CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(id);
        var user = await _sender.Send(query, cancellationToken);

        var command = _mapper.Map<UpdateUserCommand>(request) with { User = user };
        await _sender.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpPut("{id:int:min(1)}/changePassword")]
    public async Task<ActionResult> ChangePassword(
        [FromRoute] int id,
        [FromBody] ChangeUserPasswordRequest request,
        CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(id);
        var user = await _sender.Send(query, cancellationToken);

        var command = _mapper.Map<ChangeUserPasswordCommand>(request) with { User = user };
        await _sender.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpPut("{id:int:min(1)}/changeRole")]
    [Authorize(Policy = AuthorizationConstants.AdminOnlyPolicy)]
    public async Task<ActionResult> ChangeRole(
        [FromRoute] int id,
        [FromBody] ChangeUserRoleRequest request,
        CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(id);
        var user = await _sender.Send(query, cancellationToken);

        var command = _mapper.Map<ChangeUserRoleCommand>(request) with { User = user };
        await _sender.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(id);
        var user = await _sender.Send(query, cancellationToken);

        var command = new DeleteUserCommand(user);
        await _sender.Send(command, cancellationToken);

        return NoContent();
    }
}
