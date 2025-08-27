namespace Auth.Identity.Application.Exceptions;

public class MyException : Exception
{
    public int StatusCode { get; protected init;  }
    public string ErrorMessage { get; protected init; } = string.Empty;
}