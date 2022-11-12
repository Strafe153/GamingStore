using Application.Abstractions.MediatR;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Application.Users.Commands.Register;

public sealed class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, User>
{
    private readonly IUserRepository _userRepository;
    private readonly IPictureService _pictureService;
    private readonly ILogger<RegisterUserCommandHandler> _logger;

    public RegisterUserCommandHandler(
        IUserRepository userRepository,
        IPictureService pictureService,
        ILogger<RegisterUserCommandHandler> logger)
    {
        _userRepository = userRepository;
        _pictureService = pictureService;
        _logger = logger;
    }

    public async Task<User> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var profilePicture = await _pictureService.UploadAsync(command.ProfilePicture, "user-profile-pictures", command.Email);
        var user = new User(
            command.FirstName, 
            command.LastName, 
            command.Email, 
            command.UserName, 
            command.PhoneNumber, 
            profilePicture);

        var result = await _userRepository.CreateAsync(user, command.Password);

        if (!result.Succeeded)
        {
            _logger.LogWarning("Failed to register a user. The performerEmail '{Email}' is already taken", command.Email);
            throw new ValueNotUniqueException($"Email '{command.Email}' is already taken");
        }

        result = await _userRepository.AssignRoleAsync(user, "User");

        if (!result.Succeeded)
        {
            _logger.LogWarning("Failed to remove roles from user with id {Id}", user.Id);
            throw new OperationFailedException($"Failed to remove roles from user with id {user.Id}");
        }

        _logger.LogInformation("Successfully registered a user");

        return user;
    }
}
