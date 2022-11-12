using Application.Abstractions.MediatR;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Users.Commands.ChangePassword;

public sealed class ChangeUserPasswordCommandHandler : ICommandHandler<ChangeUserPasswordCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserService _userService;
    private readonly ILogger<ChangeUserPasswordCommandHandler> _logger;

    public ChangeUserPasswordCommandHandler(
        IUserRepository userRepository, 
        IUserService userService,
        ILogger<ChangeUserPasswordCommandHandler> logger)
    {
        _userRepository = userRepository;
        _userService = userService;
        _logger = logger;
    }

    public async Task<Unit> Handle(ChangeUserPasswordCommand command, CancellationToken cancellationToken)
    {
        _userService.VerifyUserAccessRights(command.User);

        var result = await _userRepository.ChangePasswordAsync(command.User, command.CurrentPassword, command.NewPassword);

        if (!result.Succeeded)
        {
            _logger.LogWarning("Failed to change password for user with id {Id}", command.User.Id);
            throw new OperationFailedException($"Failed to change password for user with id {command.User.Id}");
        }

        _logger.LogInformation("Successfully changed password for user with id {Id}", command.User.Id);

        return Unit.Value;
    }
}
