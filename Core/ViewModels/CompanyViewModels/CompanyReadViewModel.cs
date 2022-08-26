using Core.Entities;

namespace Core.ViewModels.CompanyViewModels
{
    public record CompanyReadViewModel : CompanyBaseViewModel
    {
        public int Id { get; init; }
        public ICollection<Device>? Devices { get; init; }
    }
}
