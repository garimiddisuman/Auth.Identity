using Microsoft.AspNetCore.Http;

namespace Auth.Identity.Application.Exceptions;

public class ObjectNotFoundException : MyException
{
    public ObjectNotFoundException(string message)
    {
        StatusCode = StatusCodes.Status404NotFound;
        ErrorMessage = message;
    }
}