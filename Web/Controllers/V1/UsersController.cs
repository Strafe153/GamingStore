using System.Net.Mime;
using Application.Users.Commands.ChangePassword;
using Application.Users.Commands.ChangeRole;
using Application.Users.Commands.Delete;
using Application.Users.Commands.Register;
using Application.Users.Commands.Update;
using Application.Users.Queries;
using Application.Users.Queries.GetAll;
using Application.Users.Queries.GetById;
using Asp.Versioning;
using AutoMapper;
using Domain.Shared.Constants;
using Domain.Shared.PageParameters;
using Domain.Shared.Paging;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Web.Controllers.V1;

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

    /// <summary>
    /// Fetches a page of users
    /// </summary>
    /// <param name="pageParameters">Page parameters containing number and size</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A page of users</returns>
    /// <response code="200">Returns a page of users</response>
    [HttpGet]
    [Authorize(Policy = AuthorizationConstants.AdminOnlyPolicy)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(PagedModel<GetUserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<PagedModel<GetUserResponse>>> Get(
        [FromQuery] PageParameters pageParameters,
        CancellationToken cancellationToken)
    {
        var query = new GetAllUsersQuery(pageParameters.PageNumber, pageParameters.PageSize);

        var users = await _sender.Send(query, cancellationToken);
        var userResponses = _mapper.Map<PagedModel<GetUserResponse>>(users);

        return Ok(userResponses);
    }

    /// <summary>
    /// Fetches a user by the specified identifier
    /// </summary>
    /// <param name="id">A user's identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A user by the specified identifier</returns>
    /// <response code="200">Returns a user by the specified identifier</response>
    /// <response code="404">Returns if a user does not exist</response>
    [HttpGet("{id:int:min(1)}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(GetUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetUserResponse>> Get([FromRoute] int id, CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(id);

        var user = await _sender.Send(query, cancellationToken);
        var userResponse = _mapper.Map<GetUserResponse>(user);

        return Ok(userResponse);
    }

    /// <summary>
    /// Creates a user
    /// </summary>
    /// <param name="request">A request to register a user</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The newly created user</returns>
    /// <response code="201">Returns if a user is registered successfully</response>
    /// <response code="400">Returns if the validations are not passed</response>
    /// <response code="401">Returns if a user is unauthorized</response>
    [HttpPost("register")]
    [AllowAnonymous]
    [Consumes(MediaTypeNames.Multipart.FormData)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(GetUserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<GetUserResponse>> Register(
        [FromForm] RegisterUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<RegisterUserCommand>(request);

        var user = await _sender.Send(command, cancellationToken);
        var userResponse = _mapper.Map<GetUserResponse>(user);

        return CreatedAtAction(nameof(Get), new { userResponse.Id }, userResponse);
    }

    /// <summary>
    /// Updates a user by the specified identifier
    /// </summary>
    /// <param name="id">A user's identifier</param>
    /// <param name="request">A request to update a user</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>No content</returns>
    /// <response code="204">Returns if a user is updated successfully</response>
    /// <response code="400">Returns if the validations are not passed</response>
    /// <response code="401">Returns if a user is unauthorized</response>
    /// <response code="404">Returns if a user does not exist</response>
    [HttpPut("{id:int:min(1)}")]
    [Consumes(MediaTypeNames.Multipart.FormData)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
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

    /// <summary>
    /// Changes a password for a user by the specified identifier
    /// </summary>
    /// <param name="id">A user's identifier</param>
    /// <param name="request">A request to change a user's password</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>No content</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     PUT /api/v1/users/2/password
    ///     {
    ///        "currentPassword": "01DP4ssw0_rd",
    ///        "newPassword": "N3w-pa$$"
    ///     }
    ///
    /// </remarks>
    /// <response code="204">Returns if a user's password is updated successfully</response>
    /// <response code="400">Returns if the validations are not passed</response>
    /// <response code="401">Returns if a user is unauthorized</response>
    /// <response code="404">Returns if a user does not exist</response>
    [HttpPut("{id:int:min(1)}/password")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
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

    /// <summary>
    /// Changes a role for a user by the specified identifier
    /// </summary>
    /// <param name="id">A user's identifier</param>
    /// <param name="request">A request to change a user's role</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>No content</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     PUT /api/v1/users/9/role
    ///     {
    ///        "currentPassword": "01DP4ssw0_rd",
    ///        "newPassword": "N3w-pa$$"
    ///     }
    ///
    /// </remarks>
    /// <response code="204">Returns if a user's role is changed successfully'</response>
    /// <response code="400">Returns if the validations are not passed</response>
    /// <response code="401">Returns if a user is unauthorized</response>
    /// <response code="404">Returns if a user does not exist</response>
    [HttpPut("{id:int:min(1)}/role")]
    [Authorize(Policy = AuthorizationConstants.AdminOnlyPolicy)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
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

    /// <summary>
    /// Deletes a user by the specified identifier
    /// </summary>
    /// <param name="id">A user's identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>No content</returns>
    /// <response code="204">Returns if a user is updated successfully</response>
    /// <response code="401">Returns if a user is unauthorized</response>
    /// <response code="404">Returns if a user does not exist</response>
    [HttpDelete("{id:int:min(1)}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(id);
        var user = await _sender.Send(query, cancellationToken);

        var command = new DeleteUserCommand(user);
        await _sender.Send(command, cancellationToken);

        return NoContent();
    }
}

