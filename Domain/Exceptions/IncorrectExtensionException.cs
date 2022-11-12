namespace Domain.Exceptions;

public class IncorrectExtensionException : ApplicationException
{
    public IncorrectExtensionException(string message)
        : base(message)
    {
    }
}
