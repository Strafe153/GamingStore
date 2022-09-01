using Core.Entities;

namespace Core.Dtos.CompanyDtos
{
    public record CompanyReadDto : CompanyBaseDto
    {
        public int Id { get; init; }
        public ICollection<Device>? Devices { get; init; }
    }
}
