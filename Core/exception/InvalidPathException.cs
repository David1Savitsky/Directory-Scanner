namespace Core.exception;

public class InvalidPathException : Exception
{
    public InvalidPathException(string? message) : base(message)
    {
    }
}