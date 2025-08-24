using Microsoft.AspNetCore.Http;

namespace Auth.Identity.Application.Exceptions;

public class ObjectAlreadyExistsException : Exception
{
    public int StatusCode { get; private set; }
    public string ErrorMessage { get; private set; }

    public ObjectAlreadyExistsException(string message) : base(message)
    {
        StatusCode = StatusCodes.Status409Conflict;
        ErrorMessage = message;
    }
}
