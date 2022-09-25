using AutoMapper;
using Core.Dtos;
using Core.Dtos.DeviceDtos;
using Core.Entities;
using Core.Interfaces.Services;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/devices")]
[ApiController]
[Authorize(Policy = "AdminOnly")]
public class DevicesController : ControllerBase
{
    private readonly IDeviceService _deviceService;
    private readonly IPictureService _pictureService;
    private readonly IMapper _mapper;
    private readonly string _blobFolder;

    public DevicesController(
        IDeviceService deviceService,
        IPictureService pictureService,
        IMapper mapper)
    {
        _deviceService = deviceService;
        _pictureService = pictureService;
        _mapper = mapper;
        _blobFolder = "device-pictures";
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<PageDto<DeviceReadDto>>> GetAsync([FromQuery] DevicePageParameters pageParams)
    {
        var devices = await _deviceService.GetAllAsync(pageParams.PageNumber, pageParams.PageSize, pageParams.Company);
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
    public async Task<ActionResult<DeviceReadDto>> CreateAsync([FromForm] DeviceCreateUpdateDto createDto)
    {
        var device = _mapper.Map<Device>(createDto);

        device.Picture = await _pictureService.UploadAsync(createDto.Picture, _blobFolder, createDto.Name!);
        await _deviceService.CreateAsync(device);

        var readDto = _mapper.Map<DeviceReadDto>(device);

        return CreatedAtAction(nameof(GetAsync), new { Id = readDto.Id }, readDto);
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<ActionResult> UpdateAsync([FromRoute] int id, [FromForm] DeviceCreateUpdateDto updateDto)
    {
        var device = await _deviceService.GetByIdAsync(id);

        await _pictureService.DeleteAsync(device.Picture!);

        _mapper.Map(updateDto, device);
        device.Picture = await _pictureService.UploadAsync(updateDto.Picture, _blobFolder, updateDto.Name!);

        await _deviceService.UpdateAsync(device);

        return NoContent();
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult> DeleteAsync([FromRoute] int id)
    {
        var device = await _deviceService.GetByIdAsync(id);

        await _deviceService.DeleteAsync(device);
        await _pictureService.DeleteAsync(device.Picture!);

        return NoContent();
    }
}
