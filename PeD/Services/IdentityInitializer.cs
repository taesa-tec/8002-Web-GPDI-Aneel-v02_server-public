using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace PeD.Services
{
    public class IdentityInitializer : IStartupFilter
    {
        private InstallService installService;
        private ILogger<IdentityInitializer> _logger;

        public IConfiguration Configuration { get; }

        public IdentityInitializer(ILogger<IdentityInitializer> logger, InstallService installService)
        {
            this._logger = logger;
            this.installService = installService;
        }

        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            Initialize();
            return next;
        }

        public void Initialize()
        {
            installService.PreInstall().Wait();
        }
    }
}