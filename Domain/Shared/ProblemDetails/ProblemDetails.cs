namespace Domain.Shared.ProblemDetails;

public record ProblemDetails(
	string Type,
	string Title,
	int Status,
	string Instance,
	string? Detail,
	IEnumerable<Error>? ValidationErrors);
