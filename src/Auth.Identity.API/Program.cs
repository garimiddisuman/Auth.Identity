
Console.WriteLine("Server Starting......");
var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

Console.WriteLine("Server Started serving at http://localhost:3000");
app.MapGet("/", () => "Hello World!");

await app.RunAsync();
Console.WriteLine("Server Stopped.");