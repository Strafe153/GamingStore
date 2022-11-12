namespace Domain.Exceptions;

public class ValueNotUniqueException : ApplicationException
{
    public ValueNotUniqueException(string message)
        : base(message)
    {
    }
}
