using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeD.Data;
using PeD.Models;
using PeD.Models.Catalogs;
using PeD.Models.Projetos;
using PeD.Security;
using TaesaCore.Extensions;

namespace PeD.Services
{
    //@todo Refatorar os metodos que retornam Resultado
    public class UserService
    {
        private GestorDbContext _context;
        private MailService _mailService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private AccessManager accessManager;

        public UserService(
            GestorDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            MailService mailService, AccessManager accessManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _mailService = mailService;
            this.accessManager = accessManager;
        }

        protected List<string> GetRoles(ApplicationUser user) => _userManager.GetRolesAsync(user).Result.ToList();

        public ApplicationUser Obter(string userId)
        {
            if (!String.IsNullOrWhiteSpace(userId))
            {
                var user = _context.Users
                    .Include("FotoPerfil")
                    .Where(p => p.Id == userId).Include("CatalogEmpresa").FirstOrDefault();
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
                .Include("CatalogEmpresa")
                .OrderBy(p => p.Id)
                .ToList();
            return Users;
        }

        public Resultado Incluir(ApplicationUser dadosUser)
        {
            Resultado resultado = DadosValidos(dadosUser);
            resultado.Acao = "Inclusão de User";

            if (resultado.Inconsistencias.Count == 0 &&
                _context.Users.Where(
                    p => p.Email == dadosUser.Email).Count() > 0)
            {
                resultado.Inconsistencias.Add(
                    "E-mail já cadastrado");
            }

            if (resultado.Sucesso)
            {
                string Password = DateTime.Now.ToString().ToMD5() + $"@M{DateTime.Now.Minute}";
                dadosUser.EmailConfirmed = true;
                dadosUser.DataCadastro = DateTime.Now;
                resultado = CreateUser(dadosUser, Password, dadosUser.Role);
                string UserId = resultado.Id;
                if (resultado.Sucesso)
                {
                    try
                    {
                        accessManager.SendRecoverAccountEmail(dadosUser.Email, true,
                            "Seja bem-vindo ao Gerenciador P&D Taesa").Wait(10000);
                    }
                    catch (Exception e)
                    {
                        resultado.Inconsistencias.Add(e.Message);
                    }

                    resultado.Id = UserId;
                }
            }

            return resultado;
        }

        public Resultado Atualizar(ApplicationUser dadosUser)
        {
            var resultado = new Resultado();
            resultado.Acao = "Atualização de User";

            ApplicationUser User = _context.Users
                .Where(u => u.Id == dadosUser.Id)
                .FirstOrDefault();

            if (User == null)
            {
                resultado.Inconsistencias.Add("User não encontrado");
            }

            if (resultado.Inconsistencias.Count == 0)
            {
                var roles = _userManager.GetRolesAsync(User).Result.ToList();
                roles.ForEach(r => _userManager.RemoveFromRoleAsync(User, r).Wait());
                _userManager.AddToRoleAsync(User, dadosUser.Role).Wait();

                User.Status = (dadosUser.Status != null && Enum.IsDefined(typeof(UserStatus), dadosUser.Status))
                    ? dadosUser.Status
                    : User.Status;
                User.NomeCompleto = dadosUser.NomeCompleto == null ? User.NomeCompleto : dadosUser.NomeCompleto;
                User.CatalogEmpresaId = dadosUser.CatalogEmpresaId == null
                    ? User.CatalogEmpresaId
                    : dadosUser.CatalogEmpresaId;
                User.RazaoSocial = dadosUser.RazaoSocial == null ? User.RazaoSocial : dadosUser.RazaoSocial;
                User.FotoPerfil = dadosUser.FotoPerfil == null || dadosUser.FotoPerfil.File.Length == 0
                    ? User.FotoPerfil
                    : dadosUser.FotoPerfil;


                User.Role = dadosUser.Role == null ? User.Role : dadosUser.Role;
                User.CPF = dadosUser.CPF == null ? User.CPF : dadosUser.CPF;
                User.Cargo = dadosUser.Cargo == null ? User.Cargo : dadosUser.Cargo;
                User.DataAtualizacao = DateTime.Now;
                _context.SaveChanges();
            }

            return resultado;
        }

        public Resultado Excluir(string userId)
        {
            Resultado resultado = new Resultado();
            resultado.Acao = "Exclusão de User";

            ApplicationUser User = Obter(userId);
            if (User == null)
            {
                resultado.Inconsistencias.Add(
                    "User não encontrado");
            }
            else
            {
                _context.UserProjetos.RemoveRange(_context.UserProjetos.Where(t => t.UserId == userId));
                _context.FotoPerfil.RemoveRange(_context.FotoPerfil.Where(t => t.UserId == userId));
                _context.Users.Remove(User);
                _context.SaveChanges();
            }

            return resultado;
        }

        private Resultado DadosValidos(ApplicationUser User)
        {
            var resultado = new Resultado();
            if (User == null)
            {
                resultado.Inconsistencias.Add(
                    "Preencha os Dados do User");
            }
            else
            {
                if (String.IsNullOrWhiteSpace(User.Email))
                {
                    resultado.Inconsistencias.Add(
                        "Preencha o E-mail do Usuário");
                }

                if (String.IsNullOrWhiteSpace(User.NomeCompleto))
                {
                    resultado.Inconsistencias.Add(
                        "Preencha o Nome Completo do Usuário");
                }

                if (String.IsNullOrWhiteSpace(User.Role))
                {
                    resultado.Inconsistencias.Add(
                        "Preencha a Role do usuário");
                }
                else
                {
                    if (!Roles.AllRoles.Contains(User.Role))
                    {
                        resultado.Inconsistencias.Add(
                            "Role do usuário não identificada.");
                    }
                }

                if (User.CatalogEmpresaId > 0)
                {
                    CatalogEmpresa Empresa = _context.CatalogEmpresas.Where(
                        e => e.Id == User.CatalogEmpresaId).FirstOrDefault();
                    if (Empresa == null)
                    {
                        resultado.Inconsistencias.Add("Empresa não encontrada");
                    }
                }
            }

            return resultado;
        }

        public Resultado CreateUser(
            ApplicationUser user,
            string password,
            string initialRole = null)
        {
            var resultado = new Resultado();
            resultado.Acao = "Inclusão de User";
            user.UserName = user.Email;

            if (_userManager.FindByEmailAsync(user.Email).Result == null)
            {
                var result = _userManager
                    .CreateAsync(user, password).Result;

                if (result.Succeeded &&
                    !String.IsNullOrWhiteSpace(initialRole))
                {
                    resultado.Id = user.Id;
                    _userManager.AddToRoleAsync(user, initialRole).Wait();
                }

                if (result.Errors.Count() > 0)
                {
                    foreach (var error in result.Errors)
                    {
                        resultado.Inconsistencias.Add(error.Description);
                    }
                }
            }

            return resultado;
        }

        public async Task Desativar(string emailOrId)
        {
            var user = emailOrId.Contains('@')
                ? await _userManager.FindByEmailAsync(emailOrId)
                : await _userManager.FindByIdAsync(emailOrId);
            if (user != null && user.Status == UserStatus.Ativo)
            {
                await Desativar(user);
            }
        }

        public async Task Desativar(ApplicationUser user)
        {
            user.Status = UserStatus.Inativo;
            await _userManager.UpdateAsync(user);
        }

        public async Task Ativar(string emailOrId)
        {
            var user = emailOrId.Contains('@')
                ? await _userManager.FindByEmailAsync(emailOrId)
                : await _userManager.FindByIdAsync(emailOrId);
            if (user != null && user.Status == UserStatus.Inativo)
            {
                await Ativar(user);
            }
        }

        public async Task Ativar(ApplicationUser user)
        {
            user.Status = UserStatus.Ativo;
            await _userManager.UpdateAsync(user);
        }
    }
}