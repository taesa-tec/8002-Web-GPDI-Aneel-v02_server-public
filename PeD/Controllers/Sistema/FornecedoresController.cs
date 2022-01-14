using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PeD.Authorizations;
using PeD.Core.ApiModels.Fornecedores;
using PeD.Core.Models;
using PeD.Core.Models.Fornecedores;
using PeD.Core.Requests.Sistema.Fornecedores;
using PeD.Data;
using PeD.Services;
using PeD.Services.Captacoes;
using Swashbuckle.AspNetCore.Annotations;
using TaesaCore.Controllers;
using TaesaCore.Interfaces;

namespace PeD.Controllers.Sistema
{
    [SwaggerTag("Fornecedores")]
    [Route("api/Sistema/Fornecedores")]
    [ApiController]
    [Authorize("Bearer")]
    public class FornecedoresController : ControllerCrudBase<Fornecedor, FornecedorDto,
        FornecedorCreateRequest,
        FornecedorEditRequest>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private UserService _userService;
        private PropostaService _propostaService;
        protected AccessManager AccessManager;
        private GestorDbContext _context;
        private ILogger<FornecedoresController> _logger;

        public FornecedoresController(IService<Fornecedor> service, IMapper mapper,
            UserManager<ApplicationUser> userManager, AccessManager accessManager, UserService userService,
            PropostaService propostaService, GestorDbContext context, ILogger<FornecedoresController> logger) : base(
            service, mapper)
        {
            _userManager = userManager;
            AccessManager = accessManager;
            _userService = userService;
            _propostaService = propostaService;
            _context = context;
            _logger = logger;
        }


        protected async Task<bool> UpdateResponsavelFornecedor(Fornecedor fornecedor,
            string email,
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
                    EmpresaId = fornecedor.Id == 0 ? (int?)null : fornecedor.Id
                };

                var userResult = await _userManager.CreateAsync(responsavel);
                if (userResult.Succeeded)
                {
                    await _userManager.AddToRoleAsync(responsavel, Roles.Fornecedor);
                    await AccessManager.SendNewFornecedorAccountEmail(responsavel.Email, fornecedor);
                }
                else
                {
                    foreach (var userResultError in userResult.Errors)
                    {
                        _logger.LogError("Erro na criação do fornecedor: {Error}", userResultError.Description);
                    }

                    throw new Exception("Erros na criação do usuário do responsável");
                }
            }
            else
            {
                await _userService.Ativar(responsavel.Id);
            }

            if (fornecedor.ResponsavelId != null && fornecedor.ResponsavelId != responsavel.Id)
                await _userService.Desativar(fornecedor.ResponsavelId);

            fornecedor.ResponsavelId = responsavel.Id;
            return true;
        }

        protected void UpdateResponsavelPropostaFornecedor(Fornecedor fornecedor)
        {
            var propostas = _propostaService.Filter(q =>
                q.Where(p => p.FornecedorId == fornecedor.Id && p.ResponsavelId != fornecedor.ResponsavelId));

            if (propostas.Count == 0)
                return;

            foreach (var proposta in propostas)
            {
                proposta.ResponsavelId = fornecedor.ResponsavelId;
            }

            _propostaService.Put(propostas);
        }


        protected async Task DesativarFonecedor(Fornecedor fornecedor)
        {
            fornecedor.Ativo = false;
            if (fornecedor.ResponsavelId != null)
            {
                await _userService.Desativar(fornecedor.ResponsavelId);
            }

            Service.Put(fornecedor);
        }

        protected async Task AtivarFonecedor(Fornecedor fornecedor)
        {
            fornecedor.Ativo = true;
            await _userService.Ativar(fornecedor.ResponsavelId);
            Service.Put(fornecedor);
        }

        [Authorize(Policy = Policies.IsUserTaesa)]
        public override ActionResult<FornecedorDto> Get(int id)
        {
            if (!Service.Exist(id))
                return NotFound();
            var fornecedor = Service.Filter(q => q.Include(f => f.Responsavel).Where(f => f.Id == id)).FirstOrDefault();
            return Mapper.Map<FornecedorDto>(fornecedor);
        }

        [Authorize(Policy = Policies.IsUserTaesa)]
        public override ActionResult<List<FornecedorDto>> Get()
        {
            return Mapper.Map<List<FornecedorDto>>(Service.Filter(q => q.Include(f => f.Responsavel)).ToList());
        }

        [Authorize(Policy = Policies.IsAdmin)]
        [HttpPost]
        public override IActionResult Post(FornecedorCreateRequest model)
        {
            var fornecedor = new Fornecedor()
            {
                Ativo = true,
                Nome = model.Nome,
                Cnpj = model.Cnpj,
                UF = model.Uf,
                Categoria = Empresa.CategoriaEmpresa.Fornecedor
            };

            var responsavel = _userManager.FindByEmailAsync(model.ResponsavelEmail).Result;
            if (responsavel != null && (responsavel.Role != Roles.Fornecedor ||
                                        _context.Set<Fornecedor>()
                                            .Any(f => f.ResponsavelId == responsavel.Id)))
            {
                return Problem("O responsável não pode ser cadastrado", statusCode: StatusCodes.Status400BadRequest);
            }

            Service.Post(fornecedor);
            try
            {
                UpdateResponsavelFornecedor(fornecedor, model.ResponsavelEmail, model.ResponsavelNome).Wait();
                Service.Put(fornecedor);
            }
            catch (Exception e)
            {
                _logger.LogError("Não foi possível atualizar o responsável:{Error}", e.Message);
                return Problem("Não foi possível atualizar o responsável");
            }


            return Ok(fornecedor);
        }

        [Authorize(Policy = Policies.IsAdmin)]
        [HttpPut]
        public override IActionResult Put(FornecedorEditRequest model)
        {
            if (!Service.Exist(model.Id))
                return NotFound();
            var fornecedor = Service.Get(model.Id);

            fornecedor.Nome = model.Nome;
            fornecedor.Cnpj = model.Cnpj;

            if (model.TrocarResponsavel)
            {
                var responsavel = _userManager.FindByEmailAsync(model.ResponsavelEmail).Result;
                if (responsavel != null && (responsavel.Role != Roles.Fornecedor ||
                                            _context.Set<Fornecedor>()
                                                .Any(f => f.ResponsavelId == responsavel.Id)))
                {
                    return Problem("O responsável não pode ser cadastrado",
                        statusCode: StatusCodes.Status400BadRequest);
                }

                UpdateResponsavelFornecedor(fornecedor, model.ResponsavelEmail, model.ResponsavelNome).Wait();
            }

            if (!model.Ativo && fornecedor.Ativo)
            {
                DesativarFonecedor(fornecedor).Wait();
                return Ok(fornecedor);
            }

            if (model.Ativo && !fornecedor.Ativo)
            {
                AtivarFonecedor(fornecedor).Wait();
                return Ok(fornecedor);
            }

            Service.Put(fornecedor);
            UpdateResponsavelPropostaFornecedor(fornecedor);
            return Ok(fornecedor);
        }
    }
}