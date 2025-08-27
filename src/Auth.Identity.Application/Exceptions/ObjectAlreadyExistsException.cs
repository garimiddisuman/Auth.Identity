using Microsoft.AspNetCore.Http;

namespace Auth.Identity.Application.Exceptions;

public class ObjectAlreadyExistsException : MyException
{
    public ObjectAlreadyExistsException(string message)
    {
        StatusCode = StatusCodes.Status409Conflict;
        ErrorMessage = message;
    }
}
