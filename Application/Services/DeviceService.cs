using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly IRepository<Device> _repository;
        private readonly ILogger<DeviceService> _logger;

        public DeviceService(
            IRepository<Device> repository,
            IPictureService pictureService,
            ILogger<DeviceService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task CreateAsync(Device entity)
        {
            _repository.Create(entity);
            await _repository.SaveChangesAsync();

            _logger.LogInformation("Succesfully created a device");
        }

        public async Task DeleteAsync(Device entity)
        {
            _repository.Delete(entity);
            await _repository.SaveChangesAsync();

            _logger.LogInformation($"Succesfully deleted a device with id {entity.Id}");
        }

        public async Task<PaginatedList<Device>> GetAllAsync(int pageNumber, int pageSize)
        {
            var devices = await _repository.GetAllAsync(pageNumber, pageSize);
            _logger.LogInformation("Successfully retrieved all devices");

            return devices;
        }

        public async Task<PaginatedList<Device>> GetByCompanyAsync(int companyId, int pageNumber, int pageSize)
        {
            var devices = await _repository.GetAllAsync(pageNumber, pageSize, q => q.CompanyId == companyId);
            _logger.LogInformation($"Successfully retrieved all devices of the company with id {companyId}");

            return devices;
        }

        public async Task<Device> GetByIdAsync(int id)
        {
            var device = await _repository.GetByIdAsync(id);

            if (device is null)
            {
                _logger.LogWarning($"Failed to retrieve a device with id {id}");
                throw new NullReferenceException($"Device with id {id} not found");
            }

            _logger.LogInformation($"Successfully retrieved a device with id {id}");

            return device;
        }

        public async Task UpdateAsync(Device entity)
        {
            _repository.Update(entity);
            await _repository.SaveChangesAsync();

            _logger.LogInformation($"Successfully updated a device with id {entity.Id}");
        }
    }
}
