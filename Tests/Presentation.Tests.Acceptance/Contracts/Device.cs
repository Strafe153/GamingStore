namespace Presentation.Tests.Acceptance.Contracts
{
    namespace Domain.Entities;

    public class Device
    {
        public Device(
            string name,
            DeviceCategory category,
            decimal price,
            int inStock,
            string picture,
            int companyId)
        {
            Name = name;
            Category = category;
            Price = price;
            InStock = inStock;
            Picture = picture;
            CompanyId = companyId;
        }

        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public DeviceCategory Category { get; set; }
        public decimal Price { get; set; }
        public int InStock { get; set; }
        public string Picture { get; set; }

        public int CompanyId { get; set; }
        public Company? Company { get; set; }
    }

}
