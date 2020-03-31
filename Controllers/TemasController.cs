using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using APIGestor.Business;
using APIGestor.Models;

namespace APIGestor.Controllers
{
    [Route("api/projeto/")]
    [ApiController]
    [Authorize("Bearer")]
    public class TemasController : ControllerBase
    {
        private TemaService _service;

        public TemasController(TemaService service)
        {
            _service = service;
        }

        [HttpGet("{projetoId}/Temas")]
        public ActionResult<Tema> Get(int projetoId)
        {
            if (this._service.UserProjectCan(projetoId, User))
            {
                return _service.ListarTema(projetoId);
            }

            return Forbid();
        }

         // CONTROLLER
        [HttpPost("[controller]")]
        public ActionResult<Resultado> Post([FromBody] Tema Tema)
        {
            if (this._service.UserProjectCan(Tema.ProjetoId, User, Authorizations.ProjectPermissions.LeituraEscrita))
            {
                var resultado = _service.Incluir(Tema);
                if (resultado.Sucesso)
                {
                    this.CreateLog(this._service, Tema.ProjetoId, Tema);
                }

                return resultado;
            }

            return Forbid();
        }

         // CONTROLLER
        [HttpPut("[controller]")]
        public ActionResult<Resultado> Put([FromBody] Tema Tema)
        {
            if (this._service.UserProjectCan(Tema.ProjetoId, User, Authorizations.ProjectPermissions.LeituraEscrita))
            {
                var oldTema = _service.Obter(Tema.Id);
                _service._context.Entry(oldTema).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                var resultado = _service.Atualizar(Tema);
                if (resultado.Sucesso)
                {
                    this.CreateLog(_service, oldTema.ProjetoId, _service.Obter(Tema.Id), oldTema);
                }

                return resultado;
            }

            return Forbid();
        }

        [HttpDelete("[controller]/{Id}")]
        public ActionResult<Resultado> Delete(int id)
        {
            var tema = _service.Obter(id);
            if (this._service.UserProjectCan(tema.ProjetoId, User, Authorizations.ProjectPermissions.LeituraEscrita))
            {
                var resultado = _service.Excluir(id);
                if (resultado.Sucesso)
                {
                    this.CreateLog(_service, tema.ProjetoId, tema);
                }

                return resultado;
            }

            return Forbid();
        }
    }
}