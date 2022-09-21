using Core.Entities;
using Core.Models;

namespace Core.Interfaces.Services;

public interface IDeviceService : IService<Device>
{
    Task<PaginatedList<Device>> GetAllAsync(int pageNumber, int pageSize, string? companyName);
}
