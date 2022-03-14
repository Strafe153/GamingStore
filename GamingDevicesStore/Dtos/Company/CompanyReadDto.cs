namespace GamingDevicesStore.Dtos.Company
{
    public record CompanyReadDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
    }
}
