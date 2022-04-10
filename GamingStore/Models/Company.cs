namespace GamingStore.Models
{
    public class Company
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public byte[]? Icon { get; set; }

        public ICollection<Device> Devices { get; set; } = new List<Device>();
    }
}
