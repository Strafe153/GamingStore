namespace Core.Exceptions;

public class UsernameNotUniqueException : ApplicationException
{
    public UsernameNotUniqueException(string message)
        : base(message)
    {
    }
}
