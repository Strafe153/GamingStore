using GamingDevicesStore.Data;

namespace GamingDevicesStore.Models
{
    public class Device
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DeviceCategory Category { get; set; }
        public decimal Price { get; set; }

        public Guid CompanyId { get; set; }
        public Company? Company { get; set; }
    }
}
