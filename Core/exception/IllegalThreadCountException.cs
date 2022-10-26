namespace Core.exception;

public class IllegalThreadCountException : Exception
{
    public IllegalThreadCountException(string? message) : base(message)
    {
    }
}