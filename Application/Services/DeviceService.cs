using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;

namespace Application.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly IRepository<Device> _repository;

        public DeviceService(IRepository<Device> repository)
        {
            _repository = repository;
        }

        public async Task CreateAsync(Device entity)
        {
            _repository.Create(entity);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(Device entity)
        {
            _repository.Delete(entity);
            await _repository.SaveChangesAsync();
        }

        public async Task<PaginatedList<Device>> GetAllAsync(int pageNumber, int pageSize)
        {
            var devices = await _repository.GetAllAsync(pageNumber, pageSize);
            return devices;
        }

        public async Task<PaginatedList<Device>> GetByCompanyAsync(int companyId, int pageNumber, int pageSize)
        {
            var devices = await _repository.GetAllAsync(pageNumber, pageSize, q => q.CompanyId == companyId);
            return devices;
        }

        public async Task<Device> GetByIdAsync(int id)
        {
            var device = await _repository.GetByIdAsync(id);

            if (device is null)
            {
                throw new NullReferenceException($"Device with id {id} not found");
            }

            return device;
        }

        public async Task UpdateAsync(Device entity)
        {
            _repository.Update(entity);
            await _repository.SaveChangesAsync();
        }
    }
}
