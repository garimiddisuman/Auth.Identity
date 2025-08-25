using Microsoft.AspNetCore.Http;

namespace Auth.Identity.Application.Exceptions;

public class InvalidPasswordException : Exception
{
    public int StatusCode { get; private set; }
    public string ErrorMessage { get; private set; }

    public InvalidPasswordException(string errorMessage)
    {
        StatusCode = StatusCodes.Status401Unauthorized;
        ErrorMessage = errorMessage;
    }
}