using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GamingDevicesStore.Models;
using GamingDevicesStore.Dtos.Device;
using GamingDevicesStore.Repositories.Interfaces;
using AutoMapper;

namespace GamingDevicesStore.Controllers
{
    [Route("api/devices")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly IControllable<Device> _repo;
        private readonly IMapper _mapper;

        public DevicesController(IControllable<Device> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeviceReadDto>>> GetAllDevicesAsync()
        {
            IEnumerable<Device> devices = await _repo.GetAllAsync();
            var readDtos = _mapper.Map<IEnumerable<DeviceReadDto>>(devices);

            return Ok(readDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DeviceReadDto>> GetDeviceAsync(Guid id)
        {
            Device? device = await _repo.GetByIdAsync(id);

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
            var device = _mapper.Map<Device>(createDto);

            _repo.Add(device);
            await _repo.SaveChangesAsync();

            var readDto = _mapper.Map<DeviceReadDto>(device);

            return CreatedAtAction(nameof(GetDeviceAsync), new { Id = readDto.Id }, readDto);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult> UpdateDeviceAsync(Guid id, DeviceUpdateDto updateDto)
        {
            Device? device = await _repo.GetByIdAsync(id);

            if (device is null)
            {
                return NotFound("Device not found");
            }

            _mapper.Map(updateDto, device);
            _repo.Update(device);
            await _repo.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult> DeleteDeviceAsync(Guid id)
        {
            Device? device = await _repo.GetByIdAsync(id);

            if (device is null)
            {
                return NotFound("Device not found");
            }

            _repo.Remove(device);
            await _repo.SaveChangesAsync();

            return NoContent();
        }
    }
}
