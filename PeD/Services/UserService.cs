using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PeD.Core.ApiModels;
using PeD.Core.Models;
using PeD.Data;

namespace PeD.Services
{
    public class UserService
    {
        private GestorDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private AccessManager accessManager;
        private IConfiguration Configuration;

        public UserService(
            GestorDbContext context,
            UserManager<ApplicationUser> userManager,
            AccessManager accessManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            this.accessManager = accessManager;
            Configuration = configuration;
        }

        protected List<string> GetRoles(ApplicationUser user) => _userManager.GetRolesAsync(user).Result.ToList();

        public ApplicationUser Obter(string userId)
        {
            if (!string.IsNullOrWhiteSpace(userId))
            {
                var user = _context.Users
                    .Where(p => p.Id == userId).Include("Empresa").FirstOrDefault();
                if (user != null)
                {
                    user.Roles = GetRoles(user);
                }

                return user;
            }

            return null;
        }

        public IEnumerable<ApplicationUser> ListarTodos()
        {
            var Users = _context.Users
                .Include("Empresa")
                .OrderBy(p => p.Id)
                .ToList();
            return Users;
        }

        public async Task<bool> Incluir(ApplicationUser dadosUser)
        {
            dadosUser.Id = Guid.NewGuid().ToString();
            dadosUser.EmailConfirmed = true;
            dadosUser.DataCadastro = DateTime.Now;
            await CreateUser(dadosUser, dadosUser.Role);
            return await accessManager.SendRecoverAccountEmail(dadosUser.Email, true,
                "Seja bem-vindo ao Gerenciador PDI Taesa");
        }

        public Resultado Atualizar(ApplicationUser dadosUser)
        {
            var resultado = new Resultado()
            {
                Acao = "Atualização de User"
            };

            var user = _context.Users
                .Where(u => u.Id == dadosUser.Id)
                .FirstOrDefault();

            if (user == null)
            {
                resultado.Inconsistencias.Add("User não encontrado");
            }

            if (resultado.Inconsistencias.Count == 0 && user != null)
            {
                var roles = _userManager.GetRolesAsync(user).Result.ToList();
                roles.ForEach(r => _userManager.RemoveFromRoleAsync(user, r).Wait());
                _userManager.AddToRoleAsync(user, dadosUser.Role).Wait();

                user.Status = dadosUser.Status;
                user.NomeCompleto = dadosUser.NomeCompleto ?? user.NomeCompleto;
                user.EmpresaId = dadosUser.EmpresaId == 0
                    ? null
                    : dadosUser.EmpresaId;
                user.RazaoSocial = dadosUser.RazaoSocial ?? user.RazaoSocial;
                user.Role = dadosUser.Role == null ? user.Role : dadosUser.Role;
                user.Cpf = dadosUser.Cpf ?? user.Cpf;
                user.Cargo = dadosUser.Cargo ?? user.Cargo;
                user.DataAtualizacao = DateTime.Now;
                _context.SaveChanges();
            }

            return resultado;
        }

        public Resultado Excluir(string userId)
        {
            var resultado = new Resultado();
            resultado.Acao = "Exclusão de User";

            var user = Obter(userId);
            if (user == null)
            {
                resultado.Inconsistencias.Add(
                    "User não encontrado");
            }
            else
            {
                //_context.FotoPerfil.RemoveRange(_context.FotoPerfil.Where(t => t.UserId == userId));
                _context.Users.Remove(user);
                _context.SaveChanges();
            }

            return resultado;
        }

        public async Task CreateUser(ApplicationUser user, string initialRole = null, string password = null)
        {
            if (await _userManager.FindByEmailAsync(user.Email) != null)
                throw new Exception("Email já cadastrado!");
            if (_context.Users.Any(u => u.Cpf == user.Cpf))
                throw new Exception("Cpf já cadastrado!");

            user.UserName = user.Email;

            var result = password != null
                ? await _userManager.CreateAsync(user, password)
                : await _userManager.CreateAsync(user);

            if (result.Succeeded &&
                !string.IsNullOrWhiteSpace(initialRole))
            {
                _userManager.AddToRoleAsync(user, initialRole).Wait();
            }

            if (result.Errors.Count() > 0)
            {
                throw new Exception(result.ToString());
            }
        }

        public async Task Desativar(string emailOrId)
        {
            var user = emailOrId.Contains('@')
                ? await _userManager.FindByEmailAsync(emailOrId)
                : await _userManager.FindByIdAsync(emailOrId);
            if (user != null && user.Status)
            {
                await Desativar(user);
            }
        }

        public async Task Desativar(ApplicationUser user)
        {
            user.Status = false;
            await _userManager.UpdateAsync(user);
        }

        public async Task Ativar(string emailOrId)
        {
            var user = emailOrId.Contains('@')
                ? await _userManager.FindByEmailAsync(emailOrId)
                : await _userManager.FindByIdAsync(emailOrId);
            if (user != null && user.Status == true)
            {
                await Ativar(user);
            }
        }

        public async Task Ativar(ApplicationUser user)
        {
            user.Status = true;
            await _userManager.UpdateAsync(user);
        }

        public async Task UpdateAvatar(string id, IFormFile file)
        {
            var filename = id + ".jpg";
            var storagePath = Configuration.GetValue<string>("StoragePath");
            var fullname = Path.Combine(storagePath, "avatar", filename);

            if (File.Exists(fullname))
            {
                if (file is null)
                {
                    File.Delete(fullname);
                    return;
                }

                File.Move(fullname, fullname + ".old");
            }

            try
            {
                using (var stream = File.Create(fullname))
                {
                    await file.CopyToAsync(stream);
                    stream.Close();
                }
            }
            catch (Exception)
            {
                if (File.Exists(fullname + ".old"))
                {
                    File.Move(fullname + ".old", fullname);
                }
            }
            finally
            {
                if (File.Exists(fullname + ".old"))
                {
                    File.Delete(fullname + ".old");
                }
            }
        }

        public List<ApplicationUser> GetInRole(string role)
        {
            return _context.Users.AsQueryable()
                .Where(p => p.Role == role)
                .Include("Empresa")
                .ToList();
        }

        public async Task<bool> IsUserInRole(string userId, params string[] roles)
        {
            return await _context.Users.AnyAsync(u => u.Id == userId && roles.Contains(u.Role));
        }
    }
}