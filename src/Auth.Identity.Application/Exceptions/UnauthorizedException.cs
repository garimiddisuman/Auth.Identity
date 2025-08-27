using Microsoft.AspNetCore.Http;

namespace Auth.Identity.Application.Exceptions;

public class UnauthorizedException : MyException
{
    public UnauthorizedException(string errorMessage)
    {
        StatusCode = StatusCodes.Status401Unauthorized;
        ErrorMessage = errorMessage;
    }
}