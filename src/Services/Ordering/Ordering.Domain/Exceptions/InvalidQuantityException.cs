namespace Ordering.Domain.Exceptions;

public class InvalidQuantityException : OrderingException
{
    public InvalidQuantityException() : base("Invalid number of quantity") { }
}