using Application.Abstractions.MediatR;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Domain.Entities;
using Domain.Shared.Paging;
using Microsoft.Extensions.Logging;

namespace Application.Devices.Queries.GetAll;

public sealed class GetAllDevicesQueryHandler : IQueryHandler<GetAllDevicesQuery, PagedList<Device>>
{
    private readonly IRepository<Device> _deviceRepository;
    private readonly ICacheService _cacheService;
    private readonly ILogger<GetAllDevicesQueryHandler> _logger;

    public GetAllDevicesQueryHandler(
        IRepository<Device> deviceRepository, 
        ICacheService cacheService,
        ILogger<GetAllDevicesQueryHandler> logger)
    {
        _deviceRepository = deviceRepository;
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<PagedList<Device>> Handle(GetAllDevicesQuery query, CancellationToken cancellationToken)
    {
        var key = $"devices:{query.PageNumber}:{query.PageSize}:{query.CompanyName ?? "all"}";
        var cachedDevices = await _cacheService.GetAsync<List<Device>>(key, cancellationToken);
        PagedList<Device> devices;

        if (cachedDevices is null)
        {
            if (query.CompanyName is null)
            {
                devices = await _deviceRepository.GetAllAsync(query.PageNumber, query.PageSize, cancellationToken);
                _logger.LogInformation("Successfully retrieved all devices");
            }
            else
            {
                devices = await _deviceRepository.GetAllAsync(
                    query.PageNumber, 
                    query.PageSize, 
                    cancellationToken, 
                    d => d.Company!.Name == query.CompanyName);
                _logger.LogInformation("Successfully retrieved all devices of the '{CompanyName}' company", query.CompanyName);
            }

            await _cacheService.SetAsync(key, devices, cancellationToken);
        }
        else
        {
            devices = new PagedList<Device>(cachedDevices, cachedDevices.Count, query.PageNumber, query.PageSize);
        }

        return devices;
    }
}
