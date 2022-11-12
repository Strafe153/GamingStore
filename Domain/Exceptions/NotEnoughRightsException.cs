namespace Domain.Exceptions;

public class NotEnoughRightsException : ApplicationException
{
    public NotEnoughRightsException(string message)
        : base(message)
    {
    }
}
