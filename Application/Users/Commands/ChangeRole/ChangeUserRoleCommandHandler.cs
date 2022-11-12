using Application.Abstractions.MediatR;
using Application.Abstractions.Repositories;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Users.Commands.ChangeRole;

public sealed class ChangeUserRoleCommandHandler : ICommandHandler<ChangeUserRoleCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<ChangeUserRoleCommandHandler> _logger;

    public ChangeUserRoleCommandHandler(
        IUserRepository userRepository, 
        ILogger<ChangeUserRoleCommandHandler> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<Unit> Handle(ChangeUserRoleCommand command, CancellationToken cancellationToken)
    {
        var result = await _userRepository.RemoveFromRolesAsync(command.User);

        if (!result.Succeeded)
        {
            _logger.LogWarning("Failed to remove roles from user with id {Id}", command.User.Id);
            throw new OperationFailedException($"Failed to remove roles from user with id {command.User.Id}");
        }

        result = await _userRepository.AssignRoleAsync(command.User, command.Role);

        if (!result.Succeeded)
        {
            _logger.LogWarning("Failed to assign performerRole {Role} to user with id {Id}", command.Role, command.User.Id);
            throw new OperationFailedException($"Failed to assign performerRole {command.Role} to user with id {command.User.Id}");
        }

        _logger.LogInformation("Successfully assigned performerRole {Role} to user with id {Id}", command.Role, command.User.Id);

        return Unit.Value;
    }
}
