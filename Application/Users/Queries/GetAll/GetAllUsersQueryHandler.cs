using Application.Abstractions.MediatR;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Domain.Entities;
using Domain.Shared;
using Microsoft.Extensions.Logging;

namespace Application.Users.Queries.GetAll;

public sealed class GetAllUsersQueryHandler : IQueryHandler<GetAllUsersQuery, PaginatedList<User>>
{
    private readonly IUserRepository _userRepository;
    private readonly ICacheService _cacheService;
    private readonly ILogger<GetAllUsersQueryHandler> _logger;

    public GetAllUsersQueryHandler(
        IUserRepository userRepository, 
        ICacheService cacheService,
        ILogger<GetAllUsersQueryHandler> logger)
    {
        _userRepository = userRepository;
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<PaginatedList<User>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken)
    {
        var key = $"users:{query.PageNumber}:{query.PageSize}";
        var cachedUsers = await _cacheService.GetAsync<List<User>>(key, cancellationToken);
        PaginatedList<User> users;

        if (cachedUsers is null)
        {
            users = await _userRepository.GetAllAsync(query.PageNumber, query.PageSize, cancellationToken);
            await _cacheService.SetAsync(key, users, cancellationToken);
        }
        else
        {
            users = new(cachedUsers, cachedUsers.Count, query.PageNumber, query.PageSize);
        }

        _logger.LogInformation("Successfully retrieved all users");

        return users;
    }
}
