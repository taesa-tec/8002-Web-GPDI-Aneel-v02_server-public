using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PeD.Auth;
using PeD.Core.ApiModels;
using PeD.Core.ApiModels.Auth;
using PeD.Core.Models;
using PeD.Core.Models.Fornecedores;
using PeD.Core.Requests;
using PeD.Data;
using PeD.Views.Email;

namespace PeD.Services
{
    public class AccessManager
    {
        private UserManager<ApplicationUser> _userManager;
        private GestorDbContext GestorDbContext;
        private SignInManager<ApplicationUser> _signInManager;
        private SigningConfigurations _signingConfigurations;
        private TokenConfigurations _tokenConfigurations;
        private IWebHostEnvironment _hostingEnvironment;
        private MailService _mailService;
        protected SendGridService SendGridService;
        protected IMapper Mapper;

        public AccessManager(
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            SigningConfigurations signingConfigurations, IWebHostEnvironment hostingEnvironment,
            TokenConfigurations tokenConfigurations, MailService mailService,
            SendGridService sendGridService, IMapper mapper, GestorDbContext gestorDbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _signingConfigurations = signingConfigurations;
            _hostingEnvironment = hostingEnvironment;
            _tokenConfigurations = tokenConfigurations;
            _mailService = mailService;
            SendGridService = sendGridService;
            Mapper = mapper;
            GestorDbContext = gestorDbContext;
        }

        // @todo retornar o status do usuário
        public bool ValidateCredentials(Login user)
        {
            if (user != null && !String.IsNullOrWhiteSpace(user.Email))
            {
                // Verifica a existência do usuário nas tabelas do
                // ASP.NET Core Identity
                var userIdentity = _userManager
                    .FindByEmailAsync(user.Email).Result;
                if (userIdentity != null)
                {
                    // Efetua o login com base no Id do usuário e sua senha
                    var resultadoLogin = _signInManager.CheckPasswordSignInAsync(userIdentity, user.Password, false)
                        .Result;
                    if (resultadoLogin.Succeeded)
                    {
                        return userIdentity.Status;
                    }
                }
            }

            return false;
        }

        public Token GenerateToken(Login user)
        {
            var userIdentity = _userManager
                .FindByEmailAsync(user.Email).Result;
            if (userIdentity.EmpresaId != null)
            {
                userIdentity.Empresa =
                    GestorDbContext.Empresas.FirstOrDefault(ce => ce.Id == userIdentity.EmpresaId);
            }

            var roles = _userManager.GetRolesAsync(userIdentity).Result.ToList();
            // Correção de funções do usuário
            if (roles.Count == 0 && !string.IsNullOrWhiteSpace(userIdentity.Role))
            {
                _userManager.AddToRoleAsync(userIdentity, userIdentity.Role).Wait();
                roles.Add(userIdentity.Role);
            }

            userIdentity.Roles = roles;

            ClaimsIdentity identity = new ClaimsIdentity(
                new GenericIdentity(user.Email, "Login"),
                new[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti, userIdentity.Id),
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.Email),
                    new Claim(ClaimTypes.Role,
                        userIdentity.Role ?? ""), // @todo Remover do sistema o uso do Role da tabela de usuários
                }.Concat(roles.Select(r => new Claim(ClaimTypes.Role, r)))
            );

            DateTime dataCriacao = DateTime.Now;
            DateTime dataExpiracao = dataCriacao +
                                     TimeSpan.FromSeconds(_tokenConfigurations.Seconds);

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _tokenConfigurations.Issuer,
                Audience = _tokenConfigurations.Audience,
                SigningCredentials = _signingConfigurations.SigningCredentials,
                Subject = identity,
                NotBefore = dataCriacao,
                Expires = dataExpiracao
            });
            var token = handler.WriteToken(securityToken);

            return new Token()
            {
                AccessToken = token,
                User = Mapper.Map<ApplicationUserDto>(userIdentity)
            };
        }

        public bool RecuperarSenha(Login user)
        {
            if (String.IsNullOrWhiteSpace(user.Email))
            {
                throw new Exception("Preencha o E-mail do Usuário");
            }

            try
            {
                SendRecoverAccountEmail(user.Email).Wait(10000);
            }
            catch (Exception)
            {
                return false;
            }

            return false;
        }

        public bool NovaSenha(User user)
        {
            if (String.IsNullOrWhiteSpace(user.Email))
            {
                throw new Exception("Preencha o E-mail do Usuário");
            }

            if (String.IsNullOrWhiteSpace(user.NewPassword))
            {
                throw new Exception("Preencha a nova senha do Usuário");
            }

            var User = _userManager.Users
                .Where(u => u.Email == user.Email)
                .FirstOrDefault();
            if (User == null)
            {
                throw new Exception("usuário não localizado");
            }

            var result = _userManager.ResetPasswordAsync(User, user.ResetToken, user.NewPassword).Result;
            if (result.Errors.Count() > 0)
            {
                foreach (var error in result.Errors)
                {
                    throw new Exception(error.Description);
                }
            }

            return true;
        }

        public async Task SendRecoverAccountEmail(string email, bool newAccount = false,
            string subject = "Redefinição de Senha - Gerenciador P&D Taesa")
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) throw new Exception("Email não encontrado");
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            // Console.WriteLine(token);
            await SendGridService.Send(email, subject, newAccount ? "Email/RegisterAccount" : "Email/RecoverAccount",
                new RecoverAccount()
                {
                    Email = email,
                    Token = token
                });
        }

        public async Task SendNewFornecedorAccountEmail(string email, Fornecedor fornecedor)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) throw new Exception("Email não encontrado");

            if (fornecedor == null) throw new Exception("Email não encontrado");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            // Console.WriteLine(token);
            await SendGridService.Send(email,
                "Você foi convidado para participar do Gestor P&D da Taesa como Fornecedor Cadastrado",
                "Email/FornecedorAccount",
                new FornecedorAccount()
                {
                    Email = email,
                    Token = token,
                    Fornecedor = fornecedor.Nome
                });
        }
    }
}