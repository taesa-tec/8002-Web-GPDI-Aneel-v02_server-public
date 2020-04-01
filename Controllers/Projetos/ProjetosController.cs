using System.Collections.Generic;
using System.Linq;
using APIGestor.Authorizations;
using APIGestor.Data;
using APIGestor.Models.Catalogs;
using APIGestor.Models.Projetos;
using APIGestor.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIGestor.Controllers.Projetos
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("Bearer")]
    public class ProjetosController : ControllerBase
    {
        public ProjetoService _service;
        private UserProjetoService _userprojeto_service;
        protected GestorDbContext _context;


        public ProjetosController(ProjetoService service, GestorDbContext context, UserProjetoService userprojeto_service)
        {
            _service = service;
            _context = context;
            _userprojeto_service = userprojeto_service;
        }

        [HttpGet]
        public IEnumerable<Projeto> Get()
        {

            CatalogStatus status = null;
            int statusId = 0;


            if (Request.Query.ContainsKey("status"))
            {
                status = _context.CatalogStatus.FirstOrDefault(s => s.Status == Request.Query["status"]);
                statusId = status != null ? status.Id : 0;
            }

            if (this.isAdmin())
            {
                return _service.ListarTodos(statusId);
            }

            return _userprojeto_service.ListarTodos(this.userId(), statusId)
                .Select(item => item.Projeto);
        }

        [HttpGet("{id}")]
        public ActionResult<Projeto> Get(int id)
        {
            var Projeto = _service.Obter(id);


            if (Projeto != null)
            {
                if (_service.UserProjectCan(id, User))
                {
                    return Projeto;
                }
                return Forbid();
            }
            return NotFound();
        }
        [HttpGet("{id}/me")]
        public ActionResult<CatalogUserPermissao> myAccess(int id)
        {
            if (this.isAdmin())
            {
                return new CatalogUserPermissao { Nome = "Administrador", Valor = "admin" };
            }
            var userProjeto = _context.UserProjetos.Include("CatalogUserPermissao").Where(up => up.ProjetoId == id && up.UserId == this.userId()).FirstOrDefault();
            if (userProjeto != null)
            {
                return userProjeto.CatalogUserPermissao;
            }
            return null;
        }

        [HttpGet("{id}/Usuarios")]
        public ActionResult<List<UserProjeto>> GetA(int id)
        {
            if (this._service.UserProjectCan(id, User, ProjectPermissions.Leitura))
                return _service.ObterUsuarios(id);
            return Forbid();
        }

        [HttpPost] // Criar
        public ActionResult<Resultado> Post([FromBody]Projeto Projeto)
        {
            if (this.isAdmin())
            {
                var resultado = _service.Incluir(Projeto, this.userId());
                if (resultado.Sucesso)
                {
                    this.CreateLog(this._service, int.Parse(resultado.Id), Projeto);
                }
                return resultado;
            }
            return Forbid();
        }

        [HttpPut] // Editar
        public ActionResult<Resultado> Put([FromBody]Projeto Projeto)
        {
            if (this._service.UserProjectCan(Projeto.Id, User, ProjectPermissions.Administrator))
            {
                var ProjetoOld = _service.Obter(Projeto.Id);
                _service._context.Entry(ProjetoOld).State = EntityState.Detached;
                var result = _service.Atualizar(Projeto);
                if (result.Sucesso)
                {
                    this.CreateLog(this._service, Projeto.Id, _service.Obter(Projeto.Id), ProjetoOld);
                }
                return result;
            }
            return Forbid();
        }

        [HttpDelete("{id}")] // Apagar
        public ActionResult<Resultado> Delete(int id)
        {
            if (this._service.UserProjectCan(id, User, ProjectPermissions.Administrator))
            {
                var resultado = _service.Excluir(id);
            }
            return Forbid();
        }

        [HttpPut("dataInicio")]
        public ActionResult<Resultado> PutA([FromBody]Projeto Projeto)
        {
            if (this._service.UserProjectCan(Projeto.Id, User, ProjectPermissions.Administrator))
            {
                var ProjetoOld = _service.Obter(Projeto.Id);
                _service._context.Entry(ProjetoOld).State = EntityState.Detached;
                var result = _service.AtualizaDataInicio(Projeto);
                if (result.Sucesso)
                {
                    this.CreateLog(this._service, Projeto.Id, _service.Obter(Projeto.Id), ProjetoOld);
                }
                return result;
            }
            return Forbid();
        }

        [HttpPost("prorrogar")]
        public ActionResult<Resultado> PostA([FromBody]Projeto Projeto)
        {
            if (this._service.UserProjectCan(Projeto.Id, User, ProjectPermissions.Administrator))
            {
                Etapa etapa;
                var result = _service.ProrrogarProjeto(Projeto, out etapa);
                if (result.Sucesso)
                {
                    this.CreateLog(this._service, Projeto.Id, etapa);
                }
                return result;
            }

            return Forbid();
        }
    }
}