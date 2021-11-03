using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PeD.Core.Models;
using PeD.Core.Requests;
using PeD.Data;

namespace PeD.Services
{
    public class InstallService
    {
        private readonly GestorDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private ILogger<IdentityInitializer> logger;
        public bool Installed { get; set; } = true;

        public InstallService(IServiceProvider services, IConfiguration Configuration,
            ILogger<IdentityInitializer> logger
        )
        {
            var scope = services.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<GestorDbContext>();
            _roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            this.logger = logger;
        }

        public async Task PreInstall()
        {
            if (_context is null)
                return;
            if (!_context.Database.EnsureCreated())
            {
                await CreateRoles();
                Installed = _context.Users.Any(user => user.Role == Roles.Administrador);
            }
        }

        protected async Task CreateRoles()
        {
            foreach (var role in Roles.AllRoles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    var result = await _roleManager.CreateAsync(new IdentityRole(role));
                    if (result.Succeeded)
                    {
                        Console.WriteLine($"{role} Criado");
                    }
                    else
                        result.Errors.ToList().ForEach(error => { logger.LogError(error.Description); });
                }
            }
        }

        public async Task Install(InstallRequest request)
        {
            if (Installed)
                return;
        }
    }
}