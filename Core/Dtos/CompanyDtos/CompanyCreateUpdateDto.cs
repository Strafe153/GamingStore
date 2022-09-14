using Microsoft.AspNetCore.Http;

namespace Core.Dtos.CompanyDtos
{
    public record CompanyCreateUpdateDto : CompanyBaseDto
    {
        public IFormFile? Picture { get; init; }
    }
}
