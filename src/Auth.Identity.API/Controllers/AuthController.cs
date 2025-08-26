using System.ComponentModel.DataAnnotations;
using Auth.Identity.Domain.Dto;
using Auth.Identity.Domain.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Identity.API.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(IMediator mediator) : ControllerBase
{
   [HttpPost("register")]
   public async Task<IActionResult> CreateUser([Required, FromBody] CreateUserCommand command)
   {
      return Created("/User", await mediator.Send(command));
   }

   [HttpPost("login")]
   public async Task<IActionResult> LoginUser([Required, FromBody] LoginRequest command)
   {
      var response = await mediator.Send(command);

      HttpContext.Response.Cookies.Append("jwt", response!.Token, new CookieOptions
      {
         HttpOnly = true,
         Secure = true,
         SameSite = SameSiteMode.Strict,
         Expires = DateTimeOffset.UtcNow.AddHours(1)
      });

      return Ok(response);
   }
}