using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PeD.Projetos.Controllers.Projetos {
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

         // CONTROLLER
        [HttpPost("[controller]")]
        public ActionResult<Resultado> Post( [FromBody]RegistroFinanceiro RegistroFinanceiro ) {
            var Registro = _service._context.RegistrosFinanceiros.Where(r => r.Id == RegistroFinanceiro.Id).FirstOrDefault();
            if(_service.UserProjectCan((int)RegistroFinanceiro.ProjetoId, User, ProjectPermissions.LeituraEscrita)) {
                var resultado = _service.Incluir(RegistroFinanceiro, this.userId());
                if(resultado.Sucesso) {
                    this.CreateLog(_service, (int)RegistroFinanceiro.ProjetoId, _service.Obter(RegistroFinanceiro.Id));
                }
                return resultado;
            }
            return Forbid();
        }

         // CONTROLLER
        [HttpPut("[controller]")]
        public ActionResult<Resultado> Put( [FromBody]RegistroFinanceiro RegistroFinanceiro ) {
            var Registro = _service.Obter(RegistroFinanceiro.Id);
            if(_service.UserProjectCan((int)Registro.ProjetoId, User, ProjectPermissions.LeituraEscrita)) {
                _service._context.Entry(Registro).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                var resultado = _service.Atualizar(RegistroFinanceiro, this.userId());
                if(resultado.Sucesso) {
                    this.CreateLog(_service, (int)Registro.ProjetoId, Registro, _service.Obter(RegistroFinanceiro.Id));
                }
                return resultado;
            }
            return Forbid();
        }

        [HttpDelete("[controller]/{Id}")]
        public ActionResult<Resultado> Delete( int id ) {
            var RegistroFinanceiro = _service.Obter(id);

            if(RegistroFinanceiro != null) {
                if(_service.UserProjectCan((int)RegistroFinanceiro.ProjetoId, User, ProjectPermissions.LeituraEscrita)) {
                    var resultado = _service.Excluir(id);
                    if(resultado.Sucesso) {
                        this.CreateLog(_service, (int)RegistroFinanceiro.ProjetoId, RegistroFinanceiro);
                    }
                    return resultado;
                }
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