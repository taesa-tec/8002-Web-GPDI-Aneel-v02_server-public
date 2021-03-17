using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeD.Auth;
using PeD.Core.ApiModels.Fornecedores;
using PeD.Core.Models;
using PeD.Core.Requests.Sistema.Fornecedores;
using PeD.Services;
using Swashbuckle.AspNetCore.Annotations;
using TaesaCore.Controllers;
using TaesaCore.Interfaces;

namespace PeD.Controllers.Sistema
{
    [SwaggerTag("Fornecedores")]
    [Route("api/Sistema/Fornecedores")]
    [ApiController]
    [Authorize("Bearer")]
    public class FornecedoresController : ControllerCrudBase<Core.Models.Fornecedores.Fornecedor, FornecedorDto,
        FornecedorCreateRequest,
        FornecedorEditRequest>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private UserService _userService;
        protected AccessManager AccessManager;

        public FornecedoresController(IService<Core.Models.Fornecedores.Fornecedor> service, IMapper mapper,
            UserManager<ApplicationUser> userManager, AccessManager accessManager, UserService userService) : base(
            service, mapper)
        {
            _userManager = userManager;
            AccessManager = accessManager;
            _userService = userService;
        }


        protected async Task UpdateResponsavelFornecedor(Core.Models.Fornecedores.Fornecedor fornecedor, string email,
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
                    Status = true,
                    EmpresaId = fornecedor.Id == 0 ? (int?) null : fornecedor.Id
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
                    await AccessManager.SendNewFornecedorAccountEmail(responsavel.Email);
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

        protected async Task DesativarFonecedor(Core.Models.Fornecedores.Fornecedor fornecedor)
        {
            fornecedor.Ativo = false;
            if (fornecedor.ResponsavelId != null)
            {
                await _userService.Desativar(fornecedor.ResponsavelId);
            }

            Service.Put(fornecedor);
        }

        protected async Task AtivarFonecedor(Core.Models.Fornecedores.Fornecedor fornecedor)
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
            var fornecedor = new Core.Models.Fornecedores.Fornecedor()
            {
                Ativo = true,
                Nome = model.Nome,
                Cnpj = model.Cnpj,
                Categoria = Empresa.CategoriaEmpresa.Fornecedor,
            };

            Service.Post(fornecedor);
            UpdateResponsavelFornecedor(fornecedor, model.ResponsavelEmail, model.ResponsavelNome).Wait();
            Service.Put(fornecedor);

            return Ok(fornecedor);
        }

        [HttpPut]
        public override IActionResult Put(FornecedorEditRequest model)
        {
            if (!Service.Exist(model.Id))
                return NotFound();
            var fornecedor = Service.Get(model.Id);

            fornecedor.Nome = model.Nome;
            fornecedor.Cnpj = model.Cnpj;
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