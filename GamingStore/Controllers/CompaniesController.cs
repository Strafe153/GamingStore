using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using GamingStore.Data;
using GamingStore.Models;
using GamingStore.Dtos.Company;
using GamingStore.Repositories.Interfaces;
using AutoMapper;

namespace GamingStore.Controllers
{
    [Route("api/companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyControllable _repo;
        private readonly IMapper _mapper;
        private static readonly JsonSerializerOptions serializerOptions = new()
        {
            WriteIndented = true,
            Converters =
            {
                new ByteArrayConverter()
            }
        };

        public CompaniesController(ICompanyControllable repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyReadDto>>> GetAllCompaniesAsync()
        {
            IEnumerable<Company> companies = await _repo.GetAllAsync();
            var readDtos = _mapper.Map<IEnumerable<CompanyReadDto>>(companies);

            return Ok(readDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyReadDto>> GetCompanyAsync(Guid id)
        {
            Company? company = await _repo.GetByIdAsync(id);

            if (company is null)
            {
                return NotFound("Company not found");
            }

            var readDto = _mapper.Map<CompanyReadDto>(company);

            return Ok(readDto);
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<CompanyReadDto>> CreateCompanyAsync(CompanyCreateUpdateDto createDto)
        {
            var icon = JsonSerializer.Deserialize<byte[]>(createDto.Icon, serializerOptions);
            var company = _mapper.Map<Company>(createDto with { Icon = null! });
            company.Icon = icon;

            _repo.Add(company);
            await _repo.SaveChangesAsync();

            var readDto = _mapper.Map<CompanyReadDto>(company);

            return CreatedAtAction(nameof(GetAllCompaniesAsync), new { id = readDto.Id }, readDto);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult> UpdateCompanyAsync(Guid id, CompanyCreateUpdateDto updateDto)
        {
            Company? company = await _repo.GetByIdAsync(id);

            if (company is null)
            {
                return NotFound("Company not found");
            }

            _mapper.Map(updateDto with { Icon = null! }, company);
            var icon = JsonSerializer.Deserialize<byte[]>(updateDto.Icon, serializerOptions);
            company.Icon = icon;

            _repo.Update(company);
            await _repo.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult> DeleteCompanyAsync(Guid id)
        {
            Company? company = await _repo.GetByIdAsync(id);

            if (company is null)
            {
                return NotFound("Company not found");
            }

            _repo.Remove(company);
            await _repo.SaveChangesAsync();

            return NoContent();
        }
    }
}
