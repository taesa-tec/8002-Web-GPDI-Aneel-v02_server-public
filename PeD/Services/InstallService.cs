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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private ILogger<InstallService> logger;
        public bool Installed { get; set; } = true;

        public InstallService(IServiceProvider services, ILogger<InstallService> logger)
        {
            var scope = services.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<GestorDbContext>();
            _roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            this.logger = logger;
            _userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
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
                        logger.LogInformation("Cargo {Cargo} Cadastrado", role);
                    }
                    else
                        result.Errors.ToList().ForEach(error =>
                        {
                            logger.LogError("Erro ao cadastrar o cargo {Cargo}: {Error}", role, error.Description);
                        });
                }
            }
        }

        public async Task Install(InstallRequest request)
        {
            if (Installed)
                return;

            Installed = true;
            var result = await _userManager.CreateAsync(new ApplicationUser()
            {
                Cargo = request.Cargo,
                Email = request.Email,
                Role = Roles.Administrador,
                DataCadastro = DateTime.Now,
                Status = true,
                NomeCompleto = request.NomeCompleto,
                UserName = request.Email
            }, request.Password);
            if (!result.Succeeded)
            {
                foreach (var identityError in result.Errors)
                {
                    logger.LogError("Erro ao cadastrar usuário: {Error}", identityError.Description);
                }

                throw new Exception("Erro ao criar o usuário administrativo");
            }
        }
    }
}