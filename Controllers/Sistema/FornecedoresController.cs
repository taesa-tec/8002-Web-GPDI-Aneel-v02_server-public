using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using APIGestor.Models;
using APIGestor.Models.Fornecedores;
using APIGestor.Requests.Sistema.Fornecedores;
using APIGestor.Security;
using APIGestor.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TaesaCore.Controllers;
using TaesaCore.Interfaces;

namespace APIGestor.Controllers.Sistema
{
    [SwaggerTag("Fornecedores")]
    [Route("api/Sistema/Fornedores")]
    [ApiController]
    [Authorize("Bearer")]
    public class FornecedoresController : ControllerServiceBase<Fornecedor, Fornecedor, FornecedorCreateRequest>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        protected AccessManager AccessManager;

        public FornecedoresController(IService<Fornecedor> service, IMapper mapper,
            UserManager<ApplicationUser> userManager, AccessManager accessManager) : base(
            service, mapper)
        {
            _userManager = userManager;
            AccessManager = accessManager;
        }

        protected async Task UpdateResponsavelFornecedor(Fornecedor fornecedor, string email, string nome)
        {
            var responsavel = _userManager.FindByEmailAsync(email).Result;

            if (responsavel == null)
            {
                responsavel = new ApplicationUser()
                {
                    Email = email,
                    UserName = email,
                    EmailConfirmed = true,
                    DataCadastro = DateTime.Now,
                    NomeCompleto = nome,
                };

                var md5Hash = MD5.Create();
                var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(DateTime.Now.Ticks.ToString()));
                var sBuilder = new StringBuilder();
                for (var i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                //_userManager.CreateAsync(responsavel, sBuilder.ToString());
                var userResult = await _userManager.CreateAsync(responsavel, "Pass@123");
                if (userResult.Succeeded)
                {
                    await _userManager.AddToRoleAsync(responsavel, Roles.Fornecedor);
                    await AccessManager.SendRecoverAccountEmail(responsavel.Email, true,
                        "Seja bem-vindo ao Gerenciador P&D Taesa");
                }
                else
                {
                    foreach (var userResultError in userResult.Errors)
                    {
                        Console.WriteLine(userResultError.Description);
                    }

                    throw new Exception("Erros na criação do usuário do responsável");
                }
            }

            fornecedor.ResponsavelId = responsavel.Id;
        }

        public override IActionResult Post(FornecedorCreateRequest model)
        {
            var fornecedor = new Fornecedor()
            {
                Ativo = true,
                Nome = model.Nome,
                CNPJ = model.Cnpj
            };


            UpdateResponsavelFornecedor(fornecedor, model.ResponsavelEmail, model.ResponsavelNome).Wait();
            Service.Post(fornecedor);

            return Ok(fornecedor);
        }

        protected IActionResult Put(FornecedorEditRequest model)
        {
            return base.Put(model);
        }
    }
}