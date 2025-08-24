using Auth.Identity.API.Setup;
using Auth.Identity.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Auth.Identity.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Server Starting......");
            var builder = WebApplication.CreateBuilder(args);
            builder.ConfigureServices();

            var app = builder.Build();

            app.ConfigureMiddleWare();

            Console.WriteLine("Server Started serving at http://localhost:3000");

            await app.RunAsync();
            Console.WriteLine("Server Stopped.");
        }
    }
}