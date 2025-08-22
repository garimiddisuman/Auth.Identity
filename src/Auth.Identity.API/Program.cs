
using Auth.Identity.API.Setup;

Console.WriteLine("Server Starting......");
var builder = WebApplication.CreateBuilder(args);
builder.ConfigureServices();

var app = builder.Build();

app.ConfigureMiddleWare();

Console.WriteLine("Server Started serving at http://localhost:3000");
app.MapGet("/", () => "Hello World!");

await app.RunAsync();
Console.WriteLine("Server Stopped.");