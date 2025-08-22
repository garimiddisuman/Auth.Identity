namespace Auth.Identity.API.Setup;

public static class MiddleWareConfig
{
    public static void ConfigureMiddleWare(this WebApplication app)
    {
        app.MapControllers();
        app.UseCors();
        app.UseRouting();
        app.UseHttpsRedirection();
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}