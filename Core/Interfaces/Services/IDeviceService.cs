using Core.Entities;
using Core.Models;

namespace Core.Interfaces.Services
{
    public interface IDeviceService : IService<Device>
    {
        Task<PaginatedList<Device>> GetByCompanyAsync(int companyId, int pageNumber, int pageSize);
    }
}
