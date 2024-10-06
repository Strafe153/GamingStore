using Nest;

namespace Domain.Entities;

public class Log
{
    [Keyword(Name = "id")]
    public string Id { get; set; } = default!;

    [Date(Name = "@timestamp")]
    public DateTime Timestamp { get; set; }

    [Text(Name = "message")]
    public string Message { get; set; } = default!;

    [Keyword]
    public string Index { get; set; } = default!;

    [Text(Name = "fields.Environment")]
    public string Environment { get; set; } = default!;

    [Text(Name = "_source.fields.Method")]
    public string Method { get; set; } = default!;

    [Number(Name = "fields.StatusCode")]
    public int StatusCode { get; set; }

    [Text(Name = "fields.RequestPath")]
    public string RequestPath { get; set; } = default!;

    [Text]
    public string Level { get; set; } = default!;
}
