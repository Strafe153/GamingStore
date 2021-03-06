using System.ComponentModel.DataAnnotations.Schema;
using GamingStore.Data;

namespace GamingStore.Models
{
    public class Device
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DeviceCategory Category { get; set; }

        [Column(TypeName = "decimal(8, 2)")]
        public decimal Price { get; set; }

        public int InStock { get; set; }
        public byte[]? Icon { get; set; }

        public Guid CompanyId { get; set; }
        public Company? Company { get; set; }
    }
}
