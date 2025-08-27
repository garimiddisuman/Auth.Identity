using System.Reflection;
using System.Text;
using Auth.Identity.Application.Services;
using Auth.Identity.Application.Users;
using Auth.Identity.Domain.Users.Commands;
using Auth.Identity.Domain.Users;
using Auth.Identity.Infrastructure;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Identity.API.Setup;

public static class ServiceConfig
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        
        builder.Services.AddSwaggerGen();
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly(), typeof(CreateUserCommandHandler).Assembly));
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly(), typeof(UserLoginHandler).Assembly));

        builder.Services.AddCors();
        builder.Services.AddSingleton<TokenService>(provider =>
            new TokenService(builder.Configuration.GetConnectionString("jwt_secret")!));
        
        builder.Services.AddDb(
            builder.Configuration.GetConnectionString("dbString")!);
        
        builder.Services.AddRepositories();
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var jwtSecret = builder.Configuration.GetConnectionString("jwt_secret")!; // get from appsettings.json

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,   // set true if you want issuer validation
                    ValidateAudience = false, // set true if you want audience validation
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
                    ClockSkew = TimeSpan.Zero // no tolerance for expiry
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        // Read the token from the "jwt" cookie
                        if (context.Request.Cookies.ContainsKey("jwt"))
                        {
                            context.Token = context.Request.Cookies["jwt"];
                        }

                        return Task.CompletedTask;
                    }
                };
            });
        
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddValidatorsFromAssembly(typeof(CreateUserCommandValidator).Assembly);

        builder.Services.AddScoped<PasswordHasher<CreateUserCommand>>();
        builder.Services.AddScoped<PasswordHasher<User>>();
    }
}