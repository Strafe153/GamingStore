namespace Domain.Exceptions;

public class IncorrectExtensionException : ApplicationException
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
