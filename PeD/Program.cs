using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PeD.Data;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace PeD
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = CreateLogger();
            try
            {
                Log.Information("Iniciando servidor");
                using (var host = CreateWebHostBuilder(args).Build())
                {
                    using (var scope = host.Services.CreateScope())
                    {
                        var db = scope.ServiceProvider.GetRequiredService<GestorDbContext>();
                        try
                        {
                            Log.Information("Rodando migrations pendentes");
                            db.Database.Migrate();
                            Log.Information("Migrations concluídas");
                        }
                        catch (Exception e)
                        {
                            Log.Error(e, "Erro na migração");
                            throw;
                        }
                    }

                    host.Run();
                }
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Servidor encerrado devido a um erro fatal!");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseSerilog()
                .UseStartup<Startup>();

        private static Logger CreateLogger()
        {
            return new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Verbose)
                .MinimumLevel.Override("PeD", LogEventLevel.Verbose)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Map(evt => evt.Level,
                    (level, configuration) =>
                    {
                        configuration.File($"./logs/log-{level.ToString()}.log",
                            rollingInterval: RollingInterval.Day,
                            rollOnFileSizeLimit: true,
                            fileSizeLimitBytes: 1024 * 1024 * 10);
                    })
                .CreateLogger();
        }
    }
}