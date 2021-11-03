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
using PeD.Services;

namespace PeD.Middlewares
{
    public class Installer
    {
        private readonly RequestDelegate _next;

        public Installer(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, InstallService service, IHostApplicationLifetime lifetime)
        {
            if (service.Installed)
                await _next(context);
        }
    }
}