using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PeD.Data;

namespace PeD
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var host = CreateWebHostBuilder(args).Build())
            {
                using (var scope = host.Services.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<GestorDbContext>();
                    db.Database.Migrate();
                }

                host.Run();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}