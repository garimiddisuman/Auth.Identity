using Microsoft.AspNetCore.Http;

namespace Auth.Identity.Application.Exceptions;

public class ObjectAlreadyExistsException(string message) : Exception(message)
{
    public int StatusCode { get; set; } = StatusCodes.Status409Conflict;
    public string ErrorMessage { get; set; } = message;
}
