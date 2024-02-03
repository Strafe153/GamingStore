namespace Domain.Shared;

public class CacheOptions
{
    public const string SectionName = "Cache";

    public TimeSpan AbsoluteExpirationRelativeToNow { get; set; }
}
