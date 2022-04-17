using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using GamingStore.Data;
using GamingStore.Models;
using GamingStore.Dtos.Device;
using GamingStore.Repositories.Interfaces;
using AutoMapper;

namespace GamingStore.Controllers
{
    [Route("api/devices")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly IControllable<Device> _devicesRepo;
        private readonly IControllable<Company> _companiesRepo;
        private readonly IMapper _mapper;
        private static readonly JsonSerializerOptions serializerOptions = new()
        {
            WriteIndented = true,
            Converters =
            {
                new ByteArrayConverter()
            }
        };

        public DevicesController(IControllable<Device> devicesRepo,
            IControllable<Company> companiesRepo, IMapper mapper)
        {
            _devicesRepo = devicesRepo;
            _companiesRepo = companiesRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeviceReadDto>>> GetAllDevicesAsync()
        {
            IEnumerable<Device> devices = await _devicesRepo.GetAllAsync();
            var readDtos = _mapper.Map<IEnumerable<DeviceReadDto>>(devices);

            return Ok(readDtos);
        }

        [HttpGet("company/{id}")]
        public async Task<ActionResult<IEnumerable<DeviceReadDto>>> GetDevicesByCompanyAsync(Guid id)
        {
            Company? company = await _companiesRepo.GetByIdAsync(id);

            if (company is null)
            {
                return NotFound("Company not found");
            }

            IEnumerable<Device?> devices = company.Devices.Where(d => d.CompanyId == id);
            var readDtos = _mapper.Map<IEnumerable<DeviceReadDto>>(devices);

            return Ok(readDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DeviceReadDto>> GetDeviceAsync(Guid id)
        {
            Device? device = await _devicesRepo.GetByIdAsync(id);

            if (device is null)
            {
                return NotFound("Device not found");
            }

            var readDto = _mapper.Map<DeviceReadDto>(device);

            return Ok(readDto);
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<DeviceReadDto>> CreateDeviceAsync(DeviceCreateDto createDto)
        {
            var icon = JsonSerializer.Deserialize<byte[]>(createDto.Icon, serializerOptions);
            var device = _mapper.Map<Device>(createDto with { Icon = null! });
            device.Icon = icon;

            _devicesRepo.Add(device);
            await _devicesRepo.SaveChangesAsync();

            var readDto = _mapper.Map<DeviceReadDto>(device);

            return CreatedAtAction(nameof(GetDeviceAsync), new { Id = readDto.Id }, readDto);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult> UpdateDeviceAsync(Guid id, DeviceUpdateDto updateDto)
        {
            Device? device = await _devicesRepo.GetByIdAsync(id);

            if (device is null)
            {
                return NotFound("Device not found");
            }

            _mapper.Map(updateDto with { Icon = null! }, device);
            var icon = JsonSerializer.Deserialize<byte[]>(updateDto.Icon, serializerOptions);
            device.Icon = icon;

            _devicesRepo.Update(device);
            await _devicesRepo.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult> DeleteDeviceAsync(Guid id)
        {
            Device? device = await _devicesRepo.GetByIdAsync(id);

            if (device is null)
            {
                return NotFound("Device not found");
            }

            _devicesRepo.Remove(device);
            await _devicesRepo.SaveChangesAsync();

            return NoContent();
        }
    }
}
