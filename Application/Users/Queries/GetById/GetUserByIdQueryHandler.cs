using Application.Abstractions.MediatR;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Users.Queries.GetById;

public sealed class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, User>
{
    private readonly IUserRepository _userRepository;
    private readonly ICacheService _cacheService;
    private readonly ILogger<GetUserByIdQueryHandler> _logger;

    public GetUserByIdQueryHandler(
        IUserRepository userRepository,
        ICacheService cacheService,
        ILogger<GetUserByIdQueryHandler> logger)
    {
        _userRepository = userRepository;
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<User> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        var key = $"users:{query.Id}";
        var user = await _cacheService.GetAsync<User>(key);

        if (user is null)
        {
            user = await _userRepository.GetByIdAsync(query.Id, cancellationToken);

            if (user is null)
            {
                _logger.LogWarning("Failed to retrieve a user with id {Id}", query.Id);
                throw new NullReferenceException($"User with id {query.Id} not found");
            }

            await _cacheService.SetAsync(key, user);
        }

        _logger.LogInformation("Successfully retrieved a user with id {Id}", query.Id);

        return user;
    }
}
