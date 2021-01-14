using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using APIGestor.Dtos.FornecedoresDtos;
using APIGestor.Models;
using APIGestor.Requests.Sistema.Fornecedores;
using APIGestor.Security;
using APIGestor.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using TaesaCore.Controllers;
using TaesaCore.Interfaces;

namespace APIGestor.Controllers.Sistema
{
    [SwaggerTag("Fornecedores")]
    [Route("api/Sistema/Fornecedores")]
    [ApiController]
    [Authorize("Bearer")]
    public class FornecedoresController : ControllerCrudBase<Models.Fornecedores.Fornecedor, FornecedorDto,
        FornecedorCreateRequest,
        FornecedorEditRequest>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private UserService _userService;
        protected AccessManager AccessManager;

        public FornecedoresController(IService<Models.Fornecedores.Fornecedor> service, IMapper mapper,
            UserManager<ApplicationUser> userManager, AccessManager accessManager, UserService userService) : base(
            service, mapper)
        {
            _userManager = userManager;
            AccessManager = accessManager;
            _userService = userService;
        }


        protected async Task UpdateResponsavelFornecedor(Models.Fornecedores.Fornecedor fornecedor, string email,
            string nome)
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
                    Role = Roles.Fornecedor,
                    RazaoSocial = fornecedor.Nome,
                    Status = UserStatus.Ativo,
                    CatalogEmpresaId = 0
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
            else
            {
                await _userService.Ativar(responsavel.Id);
            }

            fornecedor.ResponsavelId = responsavel.Id;
        }

        protected async Task DesativarFonecedor(Models.Fornecedores.Fornecedor fornecedor)
        {
            fornecedor.Ativo = false;
            await _userService.Desativar(fornecedor.ResponsavelId);
            Service.Put(fornecedor);
        }

        protected async Task AtivarFonecedor(Models.Fornecedores.Fornecedor fornecedor)
        {
            fornecedor.Ativo = true;
            await _userService.Ativar(fornecedor.ResponsavelId);
            Service.Put(fornecedor);
        }

        public override ActionResult<FornecedorDto> Get(int id)
        {
            if (!Service.Exist(id))
                return NotFound();
            var fornecedor = Service.Filter(q => q.Include(f => f.Responsavel).Where(f => f.Id == id)).FirstOrDefault();
            return Mapper.Map<FornecedorDto>(fornecedor);
        }

        public override ActionResult<List<FornecedorDto>> Get()
        {
            return Mapper.Map<List<FornecedorDto>>(Service.Filter(q => q.Include(f => f.Responsavel)).ToList());
        }

        [HttpPost]
        public override IActionResult Post(FornecedorCreateRequest model)
        {
            var fornecedor = new Models.Fornecedores.Fornecedor()
            {
                Ativo = true,
                Nome = model.Nome,
                CNPJ = model.Cnpj
            };


            UpdateResponsavelFornecedor(fornecedor, model.ResponsavelEmail, model.ResponsavelNome).Wait();
            Service.Post(fornecedor);

            return Ok(fornecedor);
        }

        [HttpPut]
        public override IActionResult Put(FornecedorEditRequest model)
        {
            if (!Service.Exist(model.Id))
                return NotFound();
            var fornecedor = Service.Get(model.Id);

            fornecedor.Nome = model.Nome;
            fornecedor.CNPJ = model.Cnpj;
            if (!model.Ativo && fornecedor.Ativo)
            {
                DesativarFonecedor(fornecedor).Wait();
                return Ok(fornecedor);
            }

            if (model.TrocarResponsavel)
            {
                _userService.Desativar(fornecedor.ResponsavelId).Wait();
                UpdateResponsavelFornecedor(fornecedor, model.ResponsavelEmail, model.ResponsavelNome).Wait();
            }

            if (model.Ativo && !fornecedor.Ativo)
            {
                AtivarFonecedor(fornecedor).Wait();
            }
            else
            {
                Service.Put(fornecedor);
            }

            return Ok(fornecedor);
        }
    }
}