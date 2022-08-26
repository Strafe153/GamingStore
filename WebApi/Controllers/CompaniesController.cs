using Core.Entities;
using Core.Interfaces.Services;
using Core.Models;
using Core.ViewModels;
using Core.ViewModels.CompanyViewModels;
using Core.ViewModels.DeviceViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Mappers.Interfaces;

namespace WebApi.Controllers
{
    [Route("api/companies")]
    [ApiController]
    [Authorize(Policy = "AdminOnly")]
    public class CompaniesController : ControllerBase
    {
        private readonly IService<Company> _companyService;
        private readonly IDeviceService _deviceService;
        private readonly IMapper<Company, CompanyReadViewModel> _readCompanyMapper;
        private readonly IMapper<PaginatedList<Company>, PageViewModel<CompanyReadViewModel>> _pagedCompanyMapper;
        private readonly IMapper<CompanyBaseViewModel, Company> _createCompanyMapper;
        private readonly IUpdateMapper<CompanyBaseViewModel, Company> _updateCompanyMapper;
        private readonly IMapper<PaginatedList<Device>, PageViewModel<DeviceReadViewModel>> _readDeviceEnumerableMapper;

        public CompaniesController(
            IService<Company> companyService,
            IDeviceService deviceService,
            IMapper<Company, CompanyReadViewModel> readCompanyMapper,
            IMapper<PaginatedList<Company>, PageViewModel<CompanyReadViewModel>> pagedCompanyMapper,
            IMapper<CompanyBaseViewModel, Company> createCompanyMapper,
            IUpdateMapper<CompanyBaseViewModel, Company> updateCompanyMapper,
            IMapper<PaginatedList<Device>, PageViewModel<DeviceReadViewModel>> readDeviceEnumerableMapper)
        {
            _companyService = companyService;
            _deviceService = deviceService;
            _readCompanyMapper = readCompanyMapper;
            _pagedCompanyMapper = pagedCompanyMapper;
            _createCompanyMapper = createCompanyMapper;
            _updateCompanyMapper = updateCompanyMapper;
            _readDeviceEnumerableMapper = readDeviceEnumerableMapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<PageViewModel<CompanyReadViewModel>>> GetAsync([FromQuery] PageParameters pageParams)
        {
            var companies = await _companyService.GetAllAsync(pageParams.PageNumber, pageParams.PageSize);
            var readModels = _pagedCompanyMapper.Map(companies);

            return Ok(readModels);
        }

        [HttpGet("{id:int:min(1)}")]
        [AllowAnonymous]
        public async Task<ActionResult<CompanyReadViewModel>> GetAsync([FromRoute] int id)
        {
            var company = await _companyService.GetByIdAsync(id);
            var readModel = _readCompanyMapper.Map(company);

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
            var readModels = _readDeviceEnumerableMapper.Map(devices);

            return Ok(readModels);
        }

        [HttpPost]
        public async Task<ActionResult<CompanyReadViewModel>> CreateAsync([FromBody] CompanyBaseViewModel createModel)
        {
            var company = _createCompanyMapper.Map(createModel);
            await _companyService.CreateAsync(company);

            var readModel = _readCompanyMapper.Map(company);

            return CreatedAtAction(nameof(GetAsync), new { Id = readModel.Id }, readModel);
        }

        [HttpPut("{id:int:min(1)}")]
        public async Task<ActionResult> UpdateAsync([FromRoute] int id, [FromBody] CompanyBaseViewModel updateModel)
        {
            var company = await _companyService.GetByIdAsync(id);

            _updateCompanyMapper.Map(updateModel, company);
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
