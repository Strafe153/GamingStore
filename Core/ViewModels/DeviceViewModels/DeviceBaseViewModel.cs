using Core.Enums;

namespace Core.ViewModels.DeviceViewModels
{
    public record DeviceBaseViewModel
    {
        public string? Name { get; init; }
        public DeviceCategory Category { get; init; }
        public decimal Price { get; init; }
        public int InStock { get; init; }
        public int CompanyId { get; init; }
    }
}
