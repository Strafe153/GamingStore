using Core.Enums;

namespace Core.Entities
{
    public class Device
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DeviceCategory Category { get; set; }
        public decimal Price { get; set; }
        public int InStock { get; set; }

        public int CompanyId { get; set; }
        public Company? Company { get; set; }
    }
}
