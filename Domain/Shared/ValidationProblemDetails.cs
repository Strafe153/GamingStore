using Microsoft.AspNetCore.Mvc;

namespace Domain.Shared;

public class FluentValidationProblemDetails : ProblemDetails
{
    public IEnumerable<Error>? ValidationErrors { get; set; }
}
