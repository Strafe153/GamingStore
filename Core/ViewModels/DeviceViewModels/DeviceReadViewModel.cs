namespace Core.ViewModels.DeviceViewModels
{
    public record DeviceReadViewModel : DeviceBaseViewModel
    {
        public int Id { get; init; }
        public string? CompanyName { get; init; }
    }
}
