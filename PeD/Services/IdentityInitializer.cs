using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace PeD.Services
{
    public class IdentityInitializer : IStartupFilter
    {
        private InstallService installService;

        public IdentityInitializer(InstallService installService)
        {
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