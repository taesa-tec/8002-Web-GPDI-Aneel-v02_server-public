using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeD.Authorizations;
using PeD.Core.ApiModels.Catalogos;
using PeD.Core.Models.Catalogos;
using TaesaCore.Controllers;
using TaesaCore.Interfaces;

namespace PeD.Controllers.Catalogo
{
    [Route("api/Catalogo/[controller]")]
    [ApiController]
    [Authorize("Bearer")]
    public class TemasController : ControllerCrudBase<Tema, TemaDto>
    {
        public TemasController(IService<Tema> service, IMapper mapper) : base(service, mapper)
        {
        }

        public override ActionResult<List<TemaDto>> Get()
        {
            return Mapper.Map<List<TemaDto>>(Service.Get().Where(t => t.ParentId == null));
        }

        [Authorize(Policy = Policies.IsAdmin)]
        public override IActionResult Post(TemaDto model)
        {
            return base.Post(model);
        }

        [Authorize(Policy = Policies.IsAdmin)]
        public override IActionResult Put(TemaDto model)
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