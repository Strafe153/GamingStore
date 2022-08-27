﻿using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Models;
using DataAccess.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.Repositories
{
    public class DeviceRepository : IRepository<Device>
    {
        private readonly GamingStoreContext _context;

        public DeviceRepository(GamingStoreContext context)
        {
            _context = context;
        }

        public void Create(Device entity)
        {
            _context.Devices.Add(entity);
        }

        public void Delete(Device entity)
        {
            _context.Devices.Remove(entity);
        }

        public async Task<PaginatedList<Device>> GetAllAsync(
            int pageNumber, 
            int pageSize, 
            Expression<Func<Device, bool>>? filter = null)
        {
            var query = filter is null
                ? _context.Devices
                : _context.Devices.Where(filter);

            var devices = await query
                .Include(d => d.Company)
                .ToPaginatedList(pageNumber, pageSize);

            return devices;
        }

        public async Task<Device?> GetByIdAsync(int id)
        {
            var device = await _context.Devices
                .Include(d => d.Company)
                .SingleOrDefaultAsync(d => d.Id == id);

            return device;
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