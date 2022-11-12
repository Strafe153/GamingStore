using Application.Abstractions.MediatR;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Users.Commands.Delete;

public sealed class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserService _userService;
    private readonly IPictureService _pictureService;
    private readonly ILogger<DeleteUserCommandHandler> _logger;

    public DeleteUserCommandHandler(
        IUserRepository userRepository,
        IUserService userService,
        IPictureService pictureService, 
        ILogger<DeleteUserCommandHandler> logger)
    {
        _userRepository = userRepository;
        _userService = userService;
        _pictureService = pictureService;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        _userService.VerifyUserAccessRights(command.User);

        var result = await _userRepository.DeleteAsync(command.User);

        if (!result.Succeeded)
        {
            _logger.LogWarning("Failed to delete user with username {Username}", command.User.UserName);
            throw new OperationFailedException($"Failed to delete user with username {command.User.UserName}");
        }

        await _pictureService.DeleteAsync(command.User.ProfilePicture!);

        _logger.LogInformation("Succesfully deleted user with id {Id}", command.User.Id);

        return Unit.Value;
    }
}
