using Core.Entities;
using Core.Exceptions;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class DeviceService : IDeviceService
{
    private readonly IRepository<Device> _repository;
    private readonly ICacheService _cacheService;
    private readonly ILogger<DeviceService> _logger;

    public DeviceService(
        IRepository<Device> repository,
        ICacheService cacheService,
        ILogger<DeviceService> logger)
    {
        _repository = repository;
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task CreateAsync(Device entity)
    {
        try
        {
            _repository.Create(entity);
            await _repository.SaveChangesAsync();

            _logger.LogInformation("Succesfully created a device");
        }
        catch (DbUpdateException)
        {
            _logger.LogWarning("Failed to create a device. The name '{Name}' is already taken", entity.Name);
            throw new NameNotUniqueException($"Name '{entity.Name}' is already taken");
        }
    }

    public async Task DeleteAsync(Device entity)
    {
        _repository.Delete(entity);
        await _repository.SaveChangesAsync();

        _logger.LogInformation("Succesfully deleted a device with id {Id}", entity.Id);
    }

    public async Task<PaginatedList<Device>> GetAllAsync(
        int pageNumber, int pageSize, string? companyName, CancellationToken token = default)
    {
        string key = $"devices:{pageNumber}:{pageSize}:{companyName ?? "all"}";
        var cachedDevices = await _cacheService.GetAsync<List<Device>>(key);
        PaginatedList<Device> devices;

        if (cachedDevices is null)
        {
            if (companyName is null)
            {
                devices = await _repository.GetAllAsync(pageNumber, pageSize, token);
                _logger.LogInformation("Successfully retrieved all devices");
            }
            else
            {
                devices = await _repository.GetAllAsync(pageNumber, pageSize, token, d => d.Company!.Name == companyName);
                _logger.LogInformation("Successfully retrieved all devices of the '{CompanyName}' company", companyName);
            }

            await _cacheService.SetAsync(key, devices);
        }
        else
        {
            devices = new(cachedDevices, cachedDevices.Count, pageNumber, pageSize);
        }

        return devices;
    }

    public async Task<PaginatedList<Device>> GetAllAsync(int pageNumber, int pageSize, CancellationToken token = default)
    {
        return await GetAllAsync(pageNumber, pageSize, null, token);
    }

    public async Task<Device> GetByIdAsync(int id, CancellationToken token = default)
    {
        string key = $"devices:{id}";
        var device = await _cacheService.GetAsync<Device>(key);

        if (device is null)
        {
            device = await _repository.GetByIdAsync(id, token);

            if (device is null)
            {
                _logger.LogWarning("Failed to retrieve a device with id {Id}", id);
                throw new NullReferenceException($"Device with id {id} not found");
            }

            await _cacheService.SetAsync(key, device);
        }

        _logger.LogInformation("Successfully retrieved a device with id {Id}", id);

        return device;
    }

    public async Task UpdateAsync(Device entity)
    {
        try
        {
            _repository.Update(entity);
            await _repository.SaveChangesAsync();

            _logger.LogInformation("Successfully updated a device with id {Id}", entity.Id);
        }
        catch (DbUpdateException)
        {
            _logger.LogWarning("Failed to update the device wiht id {Id}. The name '{Name}' is already taken", 
                entity.Id, entity.Name);
            throw new NameNotUniqueException($"Name '{entity.Name}' is already taken");
        }
    }
}
