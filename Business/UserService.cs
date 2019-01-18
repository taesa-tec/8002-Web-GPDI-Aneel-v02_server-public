using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using APIGestor.Data;
using APIGestor.Models;
using Microsoft.AspNetCore.Identity;
using APIGestor.Security;

namespace APIGestor.Business
{
    public class UserService
    {
        private GestorDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(
            GestorDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public ApplicationUser Obter(string userId)
        {
            if (!String.IsNullOrWhiteSpace(userId))
            {
                return _context.Users.Where(
                    p => p.Id == userId).Include("CatalogEmpresa").FirstOrDefault();
            }
            else
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

        public Resultado Incluir(User dadosUser)
        {
            Resultado resultado = DadosValidos(dadosUser);
            resultado.Acao = "Inclusão de User";

            if (resultado.Inconsistencias.Count == 0 &&
                _context.Users.Where(
                p => p.Email== dadosUser.Email).Count() > 0)
            {
                resultado.Inconsistencias.Add(
                    "E-mail já cadastrado");
            }

            if (resultado.Inconsistencias.Count == 0)
            {
                resultado = CreateUser(
                    new ApplicationUser()
                    {
                        Email = dadosUser.Email,
                        EmailConfirmed = true
                    }, dadosUser.Password, dadosUser.Role);
                
            }

            return resultado;
        }

        public Resultado Atualizar(ApplicationUser dadosUser)
        {
            var resultado = new Resultado();
            resultado.Acao = "Atualização de User";

                ApplicationUser User = _context.Users.Where(
                    u => u.Id == dadosUser.Id).FirstOrDefault();

                if (User == null)
                {
                    resultado.Inconsistencias.Add(
                        "User não encontrado");
                }

                CatalogEmpresa Empresa = _context.CatalogEmpresas.Where(
                    e => e.Id == dadosUser.CatalogEmpresaId).FirstOrDefault();

                if (Empresa == null)
                {
                    resultado.Inconsistencias.Add(
                        "Empresa não encontrada");
                }
                else
                {
                    User.Status = dadosUser.Status;
                    User.NomeCompleto = dadosUser.NomeCompleto;
                    User.CatalogEmpresaId = dadosUser.CatalogEmpresaId;
                    User.RazaoSocial = dadosUser.RazaoSocial;
                    User.FotoPerfil = dadosUser.FotoPerfil;
                    User.CPF = dadosUser.CPF;
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
                _context.Users.Remove(User);
                _context.SaveChanges();
            }

            return resultado;
        }

        private Resultado DadosValidos(User User)
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
                if (String.IsNullOrWhiteSpace(User.Password))
                {
                    resultado.Inconsistencias.Add(
                        "Preencha a senha do usuário");
                }
                if (String.IsNullOrWhiteSpace(User.Role))
                {
                    resultado.Inconsistencias.Add(
                        "Preencha a Role do usuário");
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
                    _userManager.AddToRoleAsync(user, initialRole).Wait();
                }
                if (result.Errors.Count()>0){
                    foreach(var error in result.Errors){
                        resultado.Inconsistencias.Add(error.Description);
                    }
                }
            }
            return resultado;
        }
    }
}