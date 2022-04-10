namespace GamingStore.Dtos.Company
{
    public record CompanyReadDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public byte[]? Icon { get; init; }
    }
}
