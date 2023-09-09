namespace Domain.Exceptions;

public class ValueNotUniqueException : ApplicationException
{
    public ValueNotUniqueException()
    {
    }

    public ValueNotUniqueException(string message)
        : base(message)
    {
    }

    public ValueNotUniqueException(string message, Exception innerException)
        : base(message, innerException)
    { 
    }
}
