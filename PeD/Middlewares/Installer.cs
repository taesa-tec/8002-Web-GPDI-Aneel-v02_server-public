using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using PeD.Core.Requests;

namespace PeD.Middlewares
{
    public class Installer
    {
        private readonly RequestDelegate _next;

        public Installer(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IConfiguration configuration, IHostApplicationLifetime lifetime)
        {
            if (configuration.GetValue<string>("AppName") is null)
            {
                if (context.Request.Method == "POST")
                {
                    if (await SaveSettings(context.Request, context))
                        lifetime.StopApplication();
                    return;
                }
            }

            await _next(context);
        }

        public async Task<bool> SaveSettings(HttpRequest request, HttpContext context)
        {
            request.EnableBuffering();
            using (var reader = new StreamReader(request.Body, Encoding.UTF8))
            {
                var body = await reader.ReadToEndAsync();
                var install = JsonConvert.DeserializeObject<InstallRequest>(body, new JsonSerializerSettings());
                var validator = new InstallRequestValidator();
                var result = validator.Validate(install);
                if (result.IsValid)
                {
                    File.WriteAllText("appsettings.json", body);
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
                    return true;
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
                }

                return false;
            }
        }
    }
}