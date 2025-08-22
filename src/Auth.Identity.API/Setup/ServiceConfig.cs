using Auth.Identity.Infrastructure;

namespace Auth.Identity.API.Setup;

public static class ServiceConfig
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        
        builder.Services.AddSwaggerGen();

        builder.Services.AddCors();
        
        builder.Services.AddDb(
            builder.Configuration.GetConnectionString("dbString")!);
    }
}