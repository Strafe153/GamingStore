using AutoMapper;
using Core.Entities;
using Core.Interfaces.Services;
using Core.Models;
using Core.ViewModels;
using Core.ViewModels.DeviceViewModels;
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
        public async Task<ActionResult<PageViewModel<DeviceReadViewModel>>> GetAsync([FromQuery] PageParameters pageParams)
        {
            var devices = await _deviceService.GetAllAsync(pageParams.PageNumber, pageParams.PageSize);
            var readModels = _mapper.Map<PageViewModel<DeviceReadViewModel>>(devices);

            return Ok(readModels);
        }

        [HttpGet("{id:int:min(1)}")]
        [AllowAnonymous]
        public async Task<ActionResult<DeviceReadViewModel>> GetAsync([FromRoute] int id)
        {
            var device = await _deviceService.GetByIdAsync(id);
            var readModel = _mapper.Map<DeviceReadViewModel>(device);

            return Ok(readModel);
        }

        [HttpPost]
        public async Task<ActionResult<DeviceReadViewModel>> CreateAsync([FromBody] DeviceBaseViewModel createModel)
        {
            var device = _mapper.Map<Device>(createModel);
            await _deviceService.CreateAsync(device);

            var readModel = _mapper.Map<DeviceReadViewModel>(device);

            return CreatedAtAction(nameof(GetAsync), new { Id = readModel.Id }, readModel);
        }

        [HttpPut("{id:int:min(1)}")]
        public async Task<ActionResult> UpdateAsync([FromRoute] int id, [FromBody] DeviceBaseViewModel updateModel)
        {
            var device = await _deviceService.GetByIdAsync(id);

            _mapper.Map(updateModel, device);
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
