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
using PeD.Data;

namespace PeD.Services
{
    public class IdentityInitializer : IStartupFilter
    {
        private readonly GestorDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private UserService _userService;
        private MailService _mailService;
        private ILogger<IdentityInitializer> logger;

        public IConfiguration Configuration { get; }

        public IdentityInitializer(IServiceProvider services, IConfiguration Configuration,
            ILogger<IdentityInitializer> logger
        )
        {
            var scope = services.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<GestorDbContext>();
            _userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            _roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            _mailService = scope.ServiceProvider.GetRequiredService<MailService>();
            _userService = new UserService(_context, _userManager, _roleManager, _mailService,
                scope.ServiceProvider.GetService<AccessManager>());
            this.Configuration = Configuration;
            this.logger = logger;
        }

        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            Initialize();
            return next;
        }

        public void Initialize()
        {
            if (!_context.Database.EnsureCreated())
            {
                CreateRoles().Wait();
                CreateAdminUser();
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Skip Database seed");
                Console.WriteLine();
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

        protected void CreateAdminUser()
        {
            // Verifica se já termos um administrador cadastrado
            if (_context.Users.Any(User => User.Role == Roles.Administrador))
                return;

            var adminSection = Configuration.GetSection("AdminUser");
            var adminEmail = adminSection.GetValue<string>("Email");
            var adminPass = adminSection.GetValue<string>("Password");

            if (adminEmail != null && adminPass != null)
            {
                _userService.CreateUser(
                    new ApplicationUser()
                    {
                        Email = adminEmail,
                        EmailConfirmed = true,
                        Status = true,
                        Role = Roles.Administrador,
                        EmpresaId = 1,
                        DataCadastro = DateTime.Now
                    }, adminPass, Roles.Administrador);
            }
        }
    }
}