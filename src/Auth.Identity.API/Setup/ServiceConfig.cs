using System.Reflection;
using Auth.Identity.Application.Users;
using Auth.Identity.Domain.Users.Commands;
using Auth.Identity.Infrastructure;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;

namespace Auth.Identity.API.Setup;

public static class ServiceConfig
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        
        builder.Services.AddSwaggerGen();
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly(), typeof(CreateUserCommandHandler).Assembly));

        builder.Services.AddCors();
        
        builder.Services.AddDb(
            builder.Configuration.GetConnectionString("dbString")!);
        
        builder.Services.AddRepositories();
        
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddValidatorsFromAssembly(typeof(CreateUserCommandValidator).Assembly);

        builder.Services.AddScoped<PasswordHasher<CreateUserCommand>>();
    }
}