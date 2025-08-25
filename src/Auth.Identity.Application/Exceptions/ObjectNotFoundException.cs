using Microsoft.AspNetCore.Http;

namespace Auth.Identity.Application.Exceptions;

public class ObjectNotFoundException :Exception
{
    public int StatusCode { get; private set; }
    public string ErrorMessage { get; private set; }

    public ObjectNotFoundException(string message) : base(message)
    {
        StatusCode = StatusCodes.Status404NotFound;
        ErrorMessage = message;
    }
}