using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using APIGestor.Business;
using APIGestor.Models;
using System.IdentityModel.Tokens.Jwt;

namespace APIGestor.Controllers {
    [Route("api/projeto/")]
    [ApiController]
    [Authorize("Bearer")]
    public class RegistroFinanceiroController : ControllerBase {
        private RegistroFinanceiroService _service;

        public RegistroFinanceiroController( RegistroFinanceiroService service ) {
            _service = service;
        }

        // [HttpGet("{projetoId}/RegistroFinanceiro")]
        // public IEnumerable<RegistroFinanceiro> Get(int projetoId)
        // {
        //     return _service.ListarTodos(projetoId);
        // }

        [Route("[controller]")]
        [HttpPost]
        public ActionResult<Resultado> Post( [FromBody]RegistroFinanceiro RegistroFinanceiro ) {
            var Registro = _service._context.RegistrosFinanceiros.Where(r => r.Id == RegistroFinanceiro.Id).FirstOrDefault();
            if(_service.UserProjectCan((int)RegistroFinanceiro.ProjetoId, User, Authorizations.ProjectPermissions.LeituraEscrita))
                return _service.Incluir(RegistroFinanceiro, this.userId());
            return Forbid();
        }

        [Route("[controller]")]
        [HttpPut]
        public ActionResult<Resultado> Put( [FromBody]RegistroFinanceiro RegistroFinanceiro ) {
            var Registro = _service._context.RegistrosFinanceiros.Where(r => r.Id == RegistroFinanceiro.Id).FirstOrDefault();
            if(_service.UserProjectCan((int)Registro.ProjetoId, User, Authorizations.ProjectPermissions.LeituraEscrita))
                return _service.Atualizar(RegistroFinanceiro, this.userId());
            return Forbid();
        }

        [HttpDelete("[controller]/{Id}")]
        public ActionResult<Resultado> Delete( int id ) {

            var RegistroFinanceiro = _service._context.RegistrosFinanceiros.Where(r => r.Id == id).FirstOrDefault();

            if(RegistroFinanceiro != null) {
                if(_service.UserProjectCan((int)RegistroFinanceiro.ProjetoId, User, Authorizations.ProjectPermissions.LeituraEscrita))
                    return _service.Excluir(id);
                return Forbid();
            }

            return NotFound();
        }

        [HttpGet("{projetoId}/RegistroFinanceiro/{status}")]
        public IEnumerable<RegistroFinanceiro> Get( int projetoId, StatusRegistro status ) {
            return _service.ListarTodos(projetoId, status);
        }

        // [HttpGet("{projetoId}/RegistroFinanceiro/exportar")]
        // public FileResult Download(int id)  
        // {  
        //     var registro = _service.Obter(id);
        //     if (registro==null)
        //         return null;
        //     byte[] fileBytes = System.IO.File.ReadAllBytes(@upload.Url+id);
        //     string fileName = upload.NomeArquivo;
        //     return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        // }
    }
}