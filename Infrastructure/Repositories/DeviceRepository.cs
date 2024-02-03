using Application.Abstractions.Repositories;
using Application.Common.Extensions;
using Domain.Entities;
using Domain.Shared.Paging;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class DeviceRepository : IRepository<Device>
{
    private readonly GamingStoreContext _context;

    public DeviceRepository(GamingStoreContext context)
    {
        _context = context;
    }

    public void Create(Device entity) => _context.Devices.Add(entity);

    public void Delete(Device entity) => _context.Devices.Remove(entity);

    public Task<PaginatedList<Device>> GetAllAsync(
        int pageNumber, 
        int pageSize,
        CancellationToken token = default,
        Expression<Func<Device, bool>>? filter = null)
    {
        var query = filter is null
            ? _context.Devices
            : _context.Devices.Where(filter);

        var devicesTask = query
            .Include(d => d.Company)
            .AsNoTracking()
            .ToPaginatedList(pageNumber, pageSize, token);

        return devicesTask;
    }

    public Task<Device?> GetByIdAsync(int id, CancellationToken token = default) =>
        _context.Devices
            .Include(d => d.Company)
            .FirstOrDefaultAsync(d => d.Id == id, token);

    public void Update(Device entity) => _context.Devices.Update(entity);
}
