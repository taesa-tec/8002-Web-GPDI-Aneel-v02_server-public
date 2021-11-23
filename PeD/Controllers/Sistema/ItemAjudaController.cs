using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeD.Authorizations;
using PeD.Core.ApiModels.Sistema;
using PeD.Core.Models.Sistema;
using Swashbuckle.AspNetCore.Annotations;
using TaesaCore.Controllers;
using TaesaCore.Interfaces;

namespace PeD.Controllers.Sistema
{
    [SwaggerTag("Itens de Ajuda")]
    [Route("api/Sistema/ItemAjuda")]
    [ApiController]
    [Authorize("Bearer")]
    public class ItemAjudaController : ControllerCrudBase<ItemAjuda>
    {
        public ItemAjudaController(IService<ItemAjuda> service, IMapper mapper) : base(service, mapper)
        {
        }

        [HttpGet("/api/Ajuda/{codigo}")]
        public ItemAjudaDto GetPorCodigo(string codigo)
        {
            var item = Service.Filter(q => q.Where(i => i.Codigo == codigo))
                .FirstOrDefault();
            return Mapper.Map<ItemAjudaDto>(item);
        }

        [HttpGet("/api/Ajuda/{codigo}/Conteudo")]
        public string GetConteudoPorCodigo(string codigo)
        {
            return Service.Filter(q => q.Where(i => i.Codigo == codigo))
                .FirstOrDefault()?.Conteudo ?? "";
        }

        [Authorize(Policy = Policies.IsAdmin)]
        public override IActionResult Post(ItemAjuda model)
        {
            return base.Post(model);
        }

        [Authorize(Policy = Policies.IsAdmin)]
        public override IActionResult Put(ItemAjuda model)
        {
            return base.Put(model);
        }

        [Authorize(Policy = Policies.IsAdmin)]
        public override IActionResult Delete(int id)
        {
            return base.Delete(id);
        }
    }
}