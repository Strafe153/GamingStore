using AutoMapper;
using Core.Dtos;
using Core.Dtos.CompanyDtos;
using Core.Dtos.DeviceDtos;
using Core.Entities;
using Core.Interfaces.Services;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/companies")]
    [ApiController]
    [Authorize(Policy = "AdminOnly")]
    public class CompaniesController : ControllerBase
    {
        private readonly IService<Company> _companyService;
        private readonly IDeviceService _deviceService;
        private readonly IPictureService _pictureService;
        private readonly IMapper _mapper;
        private readonly string _blobFolder;

        public CompaniesController(
            IService<Company> companyService,
            IDeviceService deviceService,
            IPictureService pictureService,
            IMapper mapper)
        {
            _companyService = companyService;
            _deviceService = deviceService;
            _pictureService = pictureService;
            _mapper = mapper;
            _blobFolder = "company-pictures";
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<PageDto<CompanyReadDto>>> GetAsync([FromQuery] PageParameters pageParams)
        {
            var companies = await _companyService.GetAllAsync(pageParams.PageNumber, pageParams.PageSize);
            var pageDto = _mapper.Map<PageDto<CompanyReadDto>>(companies);

            return Ok(pageDto);
        }

        [HttpGet("{id:int:min(1)}")]
        [AllowAnonymous]
        public async Task<ActionResult<CompanyReadDto>> GetAsync([FromRoute] int id)
        {
            var company = await _companyService.GetByIdAsync(id);
            var readDto = _mapper.Map<CompanyReadDto>(company);

            return Ok(readDto);
        }

        [HttpGet("{id:int:min(1)}/devices")]
        [AllowAnonymous]
        public async Task<ActionResult<PageDto<DeviceReadDto>>> GetDevicesAsync(
            [FromRoute] int id,
            [FromQuery] PageParameters pageParams)
        {
            var company = await _companyService.GetByIdAsync(id);
            var devices = await _deviceService.GetByCompanyAsync(company.Id, pageParams.PageNumber, pageParams.PageSize);
            var pageDto = _mapper.Map<PageDto<DeviceReadDto>>(devices);

            return Ok(pageDto);
        }

        [HttpPost]
        public async Task<ActionResult<CompanyReadDto>> CreateAsync([FromBody] CompanyBaseDto createDto)
        {
            var company = _mapper.Map<Company>(createDto);

            company.Picture = await _pictureService.UploadAsync(createDto.Picture, _blobFolder, createDto.Name!);
            await _companyService.CreateAsync(company);

            var readDto = _mapper.Map<CompanyReadDto>(company);

            return CreatedAtAction(nameof(GetAsync), new { Id = readDto.Id }, readDto);
        }

        [HttpPut("{id:int:min(1)}")]
        public async Task<ActionResult> UpdateAsync([FromRoute] int id, [FromBody] CompanyBaseDto updateDto)
        {
            var company = await _companyService.GetByIdAsync(id);

            _mapper.Map(updateDto, company);
            company.Picture = await _pictureService.UploadAsync(updateDto.Picture, _blobFolder, updateDto.Name!);
            await _companyService.UpdateAsync(company);

            return NoContent();
        }

        [HttpDelete("{id:int:min(1)}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] int id)
        {
            var company = await _companyService.GetByIdAsync(id);

            await _companyService.DeleteAsync(company);
            await _pictureService.DeleteAsync(company.Picture);

            return NoContent();
        }
    }
}
