using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using APIGestor.Data;
using APIGestor.Models;
using APIGestor.Security;
using APIGestor.Business;

namespace APIGestor.Security {
    public class AccessManager {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private SigningConfigurations _signingConfigurations;
        private TokenConfigurations _tokenConfigurations;
        private IHostingEnvironment _hostingEnvironment;
        private MailService _mailService;

        public AccessManager(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            SigningConfigurations signingConfigurations,
            IHostingEnvironment hostingEnvironment,
            TokenConfigurations tokenConfigurations,
            MailService mailService ) {
            _userManager = userManager;
            _signInManager = signInManager;
            _signingConfigurations = signingConfigurations;
            _hostingEnvironment = hostingEnvironment;
            _tokenConfigurations = tokenConfigurations;
            _mailService = mailService;
        }
        public bool ValidateCredentials( Login user ) {
            bool credenciaisValidas = false;
            if(user != null && !String.IsNullOrWhiteSpace(user.Email)) {
                // Verifica a existência do usuário nas tabelas do
                // ASP.NET Core Identity
                var userIdentity = _userManager
                    .FindByEmailAsync(user.Email).Result;
                if(userIdentity != null) {
                    // Efetua o login com base no Id do usuário e sua senha
                    var resultadoLogin = _signInManager
                        .CheckPasswordSignInAsync(userIdentity, user.Password, false)
                        .Result;
                    if(resultadoLogin.Succeeded) {

                        // Verifica se o usuário em questão possui
                        // a role Acesso-APIGestor
                        // credenciaisValidas = _userManager.IsInRoleAsync(
                        //     userIdentity, Roles.ROLE_ADMIN_GESTOR).Result;
                        //Verifica se o usuário está Ativo
                        credenciaisValidas = (userIdentity.Status > 0) ? true : false;
                    }
                }
            }

            return credenciaisValidas;
        }

        public Token GenerateToken( Login user ) {
            var userIdentity = _userManager
                    .FindByEmailAsync(user.Email).Result;

            ClaimsIdentity identity = new ClaimsIdentity(
                new GenericIdentity(user.Email, "Login"),
                new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, userIdentity.Id),
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.Email),
                        new Claim(ClaimTypes.Role, userIdentity.Role),
                }
            );

            DateTime dataCriacao = DateTime.Now;
            DateTime dataExpiracao = dataCriacao +
                TimeSpan.FromSeconds(_tokenConfigurations.Seconds);

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor {
                Issuer = _tokenConfigurations.Issuer,
                Audience = _tokenConfigurations.Audience,
                SigningCredentials = _signingConfigurations.SigningCredentials,
                Subject = identity,
                NotBefore = dataCriacao,
                Expires = dataExpiracao
            });
            var token = handler.WriteToken(securityToken);

            return new Token() {
                Authenticated = true,
                Created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                Expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
                AccessToken = token,
                Message = "OK"
            };
        }
        private string createEmailBody( ApplicationUser user ) {
            string body = string.Empty;
            using(StreamReader reader = new StreamReader(Path.Combine(_hostingEnvironment.WebRootPath, "MailTemplates/redefinir-senha.html"))) {
                body = reader.ReadToEnd();
            }
            var ResetToken = _userManager.GeneratePasswordResetTokenAsync(user).Result;
            body = body.Replace("{{USER_EMAIL}}", user.Email);
            body = body.Replace("{{USER_TOKEN}}", ResetToken);
            return body;
        }
        public Resultado RecuperarSenha( Login user ) {
            Resultado resultado = new Resultado();
            resultado.Acao = "Recuperação de senha de User";

            if(String.IsNullOrWhiteSpace(user.Email)) {
                resultado.Inconsistencias.Add(
                        "Preencha o E-mail do Usuário");
            }
            var User = _userManager
                    .FindByEmailAsync(user.Email).Result;
            if(User == null) {
                resultado.Inconsistencias.Add(
                    "E-mail não cadastrado");
            }

            if(resultado.Inconsistencias.Count == 0) {
                resultado = _mailService.SendMail(User, "Redefinição de Senha - Gerenciador P&D Taesa", "redefinir-senha");
            }

            return resultado;
        }
        public Resultado NovaSenha( User user ) {
            Resultado resultado = new Resultado();
            resultado.Acao = "Recuperação de senha de User";

            if(String.IsNullOrWhiteSpace(user.Email)) {
                resultado.Inconsistencias.Add(
                        "Preencha o E-mail do Usuário");
            }
            if(String.IsNullOrWhiteSpace(user.NewPassword)) {
                resultado.Inconsistencias.Add(
                        "Preencha a nova senha do Usuário");
            }
            var User = _userManager.Users
                    .Where(u => u.Email == user.Email)
                    .FirstOrDefault();
            if(User == null) {
                resultado.Inconsistencias.Add(
                    "usuário não localizado");
            }

            if(resultado.Inconsistencias.Count == 0) {
                var result = _userManager.ResetPasswordAsync(User, user.ResetToken, user.NewPassword).Result;
                if(result.Errors.Count() > 0) {
                    foreach(var error in result.Errors) {
                        resultado.Inconsistencias.Add(error.Description);
                    }
                }
            }

            return resultado;
        }

    }
}