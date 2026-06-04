namespace Application.Exceptions;

public class InvalidDateException : Exception
{
    public InvalidDateException()
    {
    }

    public InvalidDateException(string? message) : base(message)
    {
    }
}