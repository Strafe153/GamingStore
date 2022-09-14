namespace Core.Dtos.DeviceDtos
{
    public record DeviceReadDto : DeviceBaseDto
    {
        public int Id { get; init; }
        public string? Picture { get; init; }
    }
}
