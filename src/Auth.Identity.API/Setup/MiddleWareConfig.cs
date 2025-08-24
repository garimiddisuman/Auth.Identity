using Auth.Identity.Application.Exceptions;
using Auth.Identity.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Auth.Identity.API.Setup;

public static class MiddleWareConfig
{
    public static void ConfigureMiddleWare(this WebApplication app)
    {
        app.UseExceptionMiddleWare();
        app.MapControllers();
        app.UseCors();
        app.UseRouting();
        app.UseHttpsRedirection();
        app.UseSwagger();
        app.UseSwaggerUI();
        app.ConfigureDb();
    }

    private static void UseExceptionMiddleWare(this WebApplication app)
    {
        app.Use(async (context, next) =>
        {
            try
            {
                await next(context);
            }
            catch (ObjectAlreadyExistsException ex)
            {
                context.Response.StatusCode = ex.StatusCode;
                await context.Response.WriteAsync(ex.ErrorMessage);
            }
        });
    }

    private static void ConfigureDb(this WebApplication app)
    {
        if (app.Environment.EnvironmentName != "IntegrationTests")
        {
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.Migrate();
            }
        }
    }
}