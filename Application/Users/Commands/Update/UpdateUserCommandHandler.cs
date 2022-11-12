using Application.Abstractions.MediatR;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Users.Commands.Update;

public sealed class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserService _userService;
    private readonly IPictureService _pictureService;
    private readonly ILogger<UpdateUserCommandHandler> _logger;

    public UpdateUserCommandHandler(
        IUserRepository userRepository,
        IUserService userService,
        IPictureService pictureService,
        ILogger<UpdateUserCommandHandler> logger)
    {
        _userRepository = userRepository;
        _userService = userService;
        _pictureService = pictureService;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        _userService.VerifyUserAccessRights(command.User);
        await _pictureService.DeleteAsync(command.User.ProfilePicture!);

        command.User.FirstName = command.FirstName;
        command.User.LastName = command.LastName;
        command.User.PhoneNumber = command.PhoneNumber;
        command.User.ProfilePicture = await _pictureService.UploadAsync(
            command.ProfilePicture, "user-profile-pictures", command.User.Email);

        var result = await _userRepository.UpdateAsync(command.User);

        if (!result.Succeeded)
        {
            _logger.LogWarning("Failed to update the user with id {Id}.", command.User.Id);
            throw new OperationFailedException($"Failed to update the user with id {command.User.Id}.");
        }

        _logger.LogInformation("Successfully updater user with id {Id}", command.User.Id);

        return Unit.Value;
    }
}
