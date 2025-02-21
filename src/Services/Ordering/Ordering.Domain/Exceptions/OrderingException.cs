namespace Ordering.Domain.Exceptions;

public class OrderingException : Exception
{
    public OrderingException(string message)
        : base(message)
    { }
}