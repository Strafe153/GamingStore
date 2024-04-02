namespace Domain.Exceptions;

public class IncorrectExtensionException : Exception
{
	public IncorrectExtensionException()
	{
	}

	public IncorrectExtensionException(string message)
		: base(message)
	{
	}

	public IncorrectExtensionException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
