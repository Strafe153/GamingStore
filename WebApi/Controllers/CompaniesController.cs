using AutoMapper;
using Core.Entities;
using Core.Interfaces.Services;
using Core.Models;
using Core.ViewModels;
using Core.ViewModels.CompanyViewModels;
using Core.ViewModels.DeviceViewModels;
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
        private readonly IMapper _mapper;

        public CompaniesController(
            IService<Company> companyService,
            IDeviceService deviceService,
            IMapper mapper)
        {
            _companyService = companyService;
            _deviceService = deviceService;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<PageViewModel<CompanyReadViewModel>>> GetAsync([FromQuery] PageParameters pageParams)
        {
            var companies = await _companyService.GetAllAsync(pageParams.PageNumber, pageParams.PageSize);
            var readModels = _mapper.Map<PageViewModel<CompanyReadViewModel>>(companies);

            return Ok(readModels);
        }

        [HttpGet("{id:int:min(1)}")]
        [AllowAnonymous]
        public async Task<ActionResult<CompanyReadViewModel>> GetAsync([FromRoute] int id)
        {
            var company = await _companyService.GetByIdAsync(id);
            var readModel = _mapper.Map<CompanyReadViewModel>(company);

            return Ok(readModel);
        }

        [HttpGet("{id:int:min(1)}/devices")]
        [AllowAnonymous]
        public async Task<ActionResult<PageViewModel<DeviceReadViewModel>>> GetDevicesAsync(
            [FromRoute] int id,
            [FromQuery] PageParameters pageParams)
        {
            var company = await _companyService.GetByIdAsync(id);
            var devices = await _deviceService.GetByCompanyAsync(company.Id, pageParams.PageNumber, pageParams.PageSize);
            var readModels = _mapper.Map<PageViewModel<DeviceReadViewModel>>(devices);

            return Ok(readModels);
        }

        [HttpPost]
        public async Task<ActionResult<CompanyReadViewModel>> CreateAsync([FromBody] CompanyBaseViewModel createModel)
        {
            var company = _mapper.Map<Company>(createModel);
            await _companyService.CreateAsync(company);

            var readModel = _mapper.Map<CompanyReadViewModel>(company);

            return CreatedAtAction(nameof(GetAsync), new { Id = readModel.Id }, readModel);
        }

        [HttpPut("{id:int:min(1)}")]
        public async Task<ActionResult> UpdateAsync([FromRoute] int id, [FromBody] CompanyBaseViewModel updateModel)
        {
            var company = await _companyService.GetByIdAsync(id);

            _mapper.Map(updateModel, company);
            await _companyService.UpdateAsync(company);

            return NoContent();
        }

        [HttpDelete("{id:int:min(1)}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] int id)
        {
            var company = await _companyService.GetByIdAsync(id);
            await _companyService.DeleteAsync(company);

            return NoContent();
        }
    }
}
