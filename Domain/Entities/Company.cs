namespace Domain.Entities;

public class Company
{
    public Company(
        string name, 
        string picture)
    {
        Name = name;
        Picture = picture;
    }

    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Picture { get; set; } = default!;

    public ICollection<Device> Devices { get; set; } = new List<Device>();
}
