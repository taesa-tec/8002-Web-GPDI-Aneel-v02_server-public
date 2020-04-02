using System;
using System.Security.Cryptography;
using System.Text;
using APIGestor.Models;
using APIGestor.Models.Fornecedores;
using APIGestor.Requests.Sistema.Fornecedores;
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

        public FornecedoresController(IService<Fornecedor> service, IMapper mapper,
            UserManager<ApplicationUser> userManager) : base(
            service, mapper)
        {
            _userManager = userManager;
        }

        protected void UpdateResponsavelFornecedor(Fornecedor fornecedor, string email, string nome)
        {
            var responsavel = _userManager.FindByEmailAsync(email).Result;

            if (responsavel == null)
            {
                responsavel = new ApplicationUser()
                {
                    Email = email,
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
                _userManager.CreateAsync(responsavel, "Pass@123");
                // @todo Mandar email
                // @todo Atualizar o Role 
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


            UpdateResponsavelFornecedor(fornecedor, model.ResponsavelEmail, model.ResponsavelNome);
            Service.Post(fornecedor);

            return Ok(fornecedor);
        }

        protected IActionResult Put(FornecedorEditRequest model)
        {
            return base.Put(model);
        }
    }
}