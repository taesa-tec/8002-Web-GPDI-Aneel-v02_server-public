using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PeD.Data;
using PeD.Data.Views;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using BindingFlags = System.Reflection.BindingFlags;

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
                            var pendingMigrations = db.Database.GetPendingMigrations();
                            if (pendingMigrations.Count() > 0)
                            {
                                Log.Information("Rodando migrations pendentes");
                                db.Database.Migrate();
                                Log.Information("Migrations concluídas");
                            }
                        }
                        catch (Exception e)
                        {
                            Log.Error(e, "Erro na migração");
                            throw;
                        }

                        try
                        {
                            Log.Information("Atualizando Views");
                            var viewType = typeof(IView);
                            var views = AppDomain.CurrentDomain.GetAssemblies()
                                .SelectMany(a => a.GetTypes())
                                .Where(t => viewType.IsAssignableFrom(t) && !t.IsInterface);
                            foreach (var view in views)
                            {
                                var viewObj = Activator.CreateInstance(view) as IView;
                                var sql = viewObj?.CreateView ?? "";
                                if (!string.IsNullOrWhiteSpace(sql))
                                {
                                    db.Database.ExecuteSqlRaw(sql);
                                }
                                else
                                {
                                    Log.Warning("View {View} sql não executado", view.ToString());
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Log.Error(e, "Erro no update das views: {Erro}", e.Message);
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