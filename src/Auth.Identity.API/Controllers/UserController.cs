using System.ComponentModel.DataAnnotations;
using Auth.Identity.Domain.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Identity.API.Controllers;

[ApiController]
[Route("User")]
public class UserController(IMediator mediator) : ControllerBase
{
   [HttpPost]
   public async Task<IActionResult> CreateUser([Required, FromBody] CreateUserCommand command)
   {
      return Created("/User", await mediator.Send(command));
   }
}