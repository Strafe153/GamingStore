using AutoMapper;
using Core.Dtos;
using Core.Dtos.DeviceDtos;
using Core.Entities;
using Core.Interfaces.Services;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/devices")]
    [ApiController]
    [Authorize(Policy = "AdminOnly")]
    public class DevicesController : ControllerBase
    {
        private readonly IDeviceService _deviceService;
        private readonly IMapper _mapper;

        public DevicesController(
            IDeviceService deviceService,
            IMapper mapper)
        {
            _deviceService = deviceService;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<PageDto<DeviceReadDto>>> GetAsync([FromQuery] PageParameters pageParams)
        {
            var devices = await _deviceService.GetAllAsync(pageParams.PageNumber, pageParams.PageSize);
            var pageDto = _mapper.Map<PageDto<DeviceReadDto>>(devices);

            return Ok(pageDto);
        }

        [HttpGet("{id:int:min(1)}")]
        [AllowAnonymous]
        public async Task<ActionResult<DeviceReadDto>> GetAsync([FromRoute] int id)
        {
            var device = await _deviceService.GetByIdAsync(id);
            var readDto = _mapper.Map<DeviceReadDto>(device);

            return Ok(readDto);
        }

        [HttpPost]
        public async Task<ActionResult<DeviceReadDto>> CreateAsync([FromBody] DeviceBaseDto createDto)
        {
            var device = _mapper.Map<Device>(createDto);
            await _deviceService.CreateAsync(device);

            var readDto = _mapper.Map<DeviceReadDto>(device);

            return CreatedAtAction(nameof(GetAsync), new { Id = readDto.Id }, readDto);
        }

        [HttpPut("{id:int:min(1)}")]
        public async Task<ActionResult> UpdateAsync([FromRoute] int id, [FromBody] DeviceBaseDto updateDto)
        {
            var device = await _deviceService.GetByIdAsync(id);

            _mapper.Map(updateDto, device);
            await _deviceService.UpdateAsync(device);

            return NoContent();
        }

        [HttpDelete("{id:int:min(1)}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] int id)
        {
            var device = await _deviceService.GetByIdAsync(id);
            await _deviceService.DeleteAsync(device);

            return NoContent();
        }
    }
}
