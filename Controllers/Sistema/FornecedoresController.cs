using APIGestor.Models.Fornecedores;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
    public class FornecedoresController : ControllerServiceBase<Fornecedor>
    {
        public FornecedoresController(IService<Fornecedor> service, IMapper mapper) : base(service, mapper)
        {
        }
    }
}