using Microsoft.EntityFrameworkCore;
using GamingStore.Data;
using GamingStore.Models;
using GamingStore.Repositories.Interfaces;

namespace GamingStore.Repositories.Implementations
{
    public class DevicesRepository : IControllable<Device>
    {
        private readonly DataContext _context;

        public DevicesRepository(DataContext context)
        {
            _context = context;
        }

        public void Add(Device entity)
        {
            _context.Devices.Add(entity);
        }

        public async Task<IEnumerable<Device>> GetAllAsync()
        {
            return await _context.Devices.ToListAsync();
        }

        public async Task<Device?> GetByIdAsync(Guid id)
        {
            return await _context.Devices.SingleOrDefaultAsync(d => d.Id == id);
        }

        public void Remove(Device entity)
        {
            _context.Devices.Remove(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(Device entity)
        {
            _context.Devices.Update(entity);
        }
    }
}
