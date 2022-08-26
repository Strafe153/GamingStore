using Core.Entities;
using Core.Interfaces.Services;
using Core.Models;
using Core.ViewModels;
using Core.ViewModels.DeviceViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Mappers.Interfaces;

namespace WebApi.Controllers
{
    [Route("api/devices")]
    [ApiController]
    [Authorize(Policy = "AdminOnly")]
    public class DevicesController : ControllerBase
    {
        private readonly IDeviceService _deviceService;
        private readonly IMapper<Device, DeviceReadViewModel> _readMapper;
        private readonly IMapper<PaginatedList<Device>, PageViewModel<DeviceReadViewModel>> _paginatedMapper;
        private readonly IMapper<DeviceBaseViewModel, Device> _createMapper;
        private readonly IUpdateMapper<DeviceBaseViewModel, Device> _updateMapper;

        public DevicesController(
            IDeviceService deviceService, 
            IMapper<Device, DeviceReadViewModel> readMapper,
            IMapper<PaginatedList<Device>, PageViewModel<DeviceReadViewModel>> paginatedMapper,
            IMapper<DeviceBaseViewModel, Device> createMapper,
            IUpdateMapper<DeviceBaseViewModel, Device> updateMapper)
        {
            _deviceService = deviceService;
            _readMapper = readMapper;
            _paginatedMapper = paginatedMapper;
            _createMapper = createMapper;
            _updateMapper = updateMapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<PageViewModel<DeviceReadViewModel>>> GetAsync([FromQuery] PageParameters pageParams)
        {
            var devices = await _deviceService.GetAllAsync(pageParams.PageNumber, pageParams.PageSize);
            var readModels = _paginatedMapper.Map(devices);

            return Ok(readModels);
        }

        [HttpGet("{id:int:min(1)}")]
        [AllowAnonymous]
        public async Task<ActionResult<DeviceReadViewModel>> GetAsync([FromRoute] int id)
        {
            var device = await _deviceService.GetByIdAsync(id);
            var readModel = _readMapper.Map(device);

            return Ok(readModel);
        }

        [HttpPost]
        public async Task<ActionResult<DeviceReadViewModel>> CreateAsync([FromBody] DeviceBaseViewModel createModel)
        {
            var device = _createMapper.Map(createModel);
            await _deviceService.CreateAsync(device);

            var readModel = _readMapper.Map(device);

            return CreatedAtAction(nameof(GetAsync), new { Id = readModel.Id }, readModel);
        }

        [HttpPut("{id:int:min(1)}")]
        public async Task<ActionResult> UpdateAsync([FromRoute] int id, [FromBody] DeviceBaseViewModel updateModel)
        {
            var device = await _deviceService.GetByIdAsync(id);

            _updateMapper.Map(updateModel, device);
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
