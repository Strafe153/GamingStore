using Microsoft.AspNetCore.Http;

namespace Core.Dtos.DeviceDtos
{
    public record DeviceCreateUpdateDto : DeviceBaseDto
    {
        public IFormFile? Picture { get; init; }
    }
}
