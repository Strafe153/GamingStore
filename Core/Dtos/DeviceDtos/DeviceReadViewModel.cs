namespace Core.Dtos.DeviceDtos
{
    public record DeviceReadDto : DeviceBaseDto
    {
        public int Id { get; init; }
        public string? CompanyName { get; init; }
    }
}
