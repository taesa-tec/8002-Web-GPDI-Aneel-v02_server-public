using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using APIGestor.Business;
using APIGestor.Models;
using System.IdentityModel.Tokens.Jwt;

namespace APIGestor.Controllers
{
    [Route("api/projeto/")]
    [ApiController]
    [Authorize("Bearer")]
    public class RegistroFinanceiroController : ControllerBase
    {
        private RegistroFinanceiroService _service;

        public RegistroFinanceiroController(RegistroFinanceiroService service)
        {
            _service = service;
        }

        // [HttpGet("{projetoId}/RegistroFinanceiro")]
        // public IEnumerable<RegistroFinanceiro> Get(int projetoId)
        // {
        //     return _service.ListarTodos(projetoId);
        // }

        [Route("[controller]")]
        [HttpPost]
        public Resultado Post([FromBody]RegistroFinanceiro RegistroFinanceiro)
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Jti).Value;
            return _service.Incluir(RegistroFinanceiro, userId);
        }

        [Route("[controller]")]
        [HttpPut]
        public Resultado Put([FromBody]RegistroFinanceiro RegistroFinanceiro)
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Jti).Value;
            return _service.Atualizar(RegistroFinanceiro, userId);
        }

        [HttpDelete("[controller]/{Id}")]
        public Resultado Delete(int id)
        {
            return _service.Excluir(id);
        }

        [HttpGet("{projetoId}/RegistroFinanceiro/{status}")]
        public IEnumerable<RegistroFinanceiro> Get(int projetoId, StatusRegistro status)
        {
            return _service.ListarTodos(projetoId, status);
        }
    }
}