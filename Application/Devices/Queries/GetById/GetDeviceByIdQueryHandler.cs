using Application.Abstractions.MediatR;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Devices.Queries.GetById;

public sealed class GetDeviceByIdQueryHandler : IQueryHandler<GetDeviceByIdQuery, Device>
{
    private readonly IRepository<Device> _deviceRepository;
    private readonly ICacheService _cacheService;
    private readonly ILogger<GetDeviceByIdQueryHandler> _logger;

    public GetDeviceByIdQueryHandler(
        IRepository<Device> deviceRepository, 
        ICacheService cacheService,
        ILogger<GetDeviceByIdQueryHandler> logger)
    {
        _deviceRepository = deviceRepository;
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<Device> Handle(GetDeviceByIdQuery query, CancellationToken cancellationToken)
    {
        var key = $"devices:{query.Id}";
        var device = await _cacheService.GetAsync<Device>(key, cancellationToken);

        if (device is null)
        {
            device = await _deviceRepository.GetByIdAsync(query.Id, cancellationToken);

            if (device is null)
            {
                _logger.LogWarning("Failed to retrieve a Device with id {Id}", query.Id);
                throw new NullReferenceException($"Device with id {query.Id} not found");
            }

            await _cacheService.SetAsync(key, device, cancellationToken);
        }

        _logger.LogInformation("Successfully retrieved a Device with id {Id}", query.Id);

        return device;
    }
}
