namespace GamingDevicesStore.Models
{
    public class Company
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<Device>? Devices { get; set; }
    }
}
