
using Auth.Identity.API.Setup;
using Auth.Identity.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Server Starting......");
var builder = WebApplication.CreateBuilder(args);
builder.ConfigureServices();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

app.ConfigureMiddleWare();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<Auth.Identity.Infrastructure.Database.AppDbContext>();
    db.Database.Migrate();
}

Console.WriteLine("Server Started serving at http://localhost:3000");
app.MapGet("/", () => "Hello World!");

await app.RunAsync();
Console.WriteLine("Server Stopped.");