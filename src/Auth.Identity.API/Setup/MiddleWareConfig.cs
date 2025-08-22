namespace Auth.Identity.API.Setup;

public static class MiddleWareConfig
{
    public static void ConfigureMiddleWare(this WebApplication app)
    {
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors();
        app.UseSwagger();
        app.UseSwaggerUI();
        app.MapControllers();
    }
}