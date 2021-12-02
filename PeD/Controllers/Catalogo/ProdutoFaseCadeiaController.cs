using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeD.Core.Models.Catalogos;
using PeD.Data;
using Swashbuckle.AspNetCore.Annotations;

namespace PeD.Controllers.Catalogo
{
    [SwaggerTag("Catalogo")]
    [Route("api/Catalogo/[controller]")]
    [ApiController]
    [Authorize("Bearer")]
    public class ProdutoFaseCadeiaController : CatalogController<FaseCadeiaProduto>
    {
        public ProdutoFaseCadeiaController(GestorDbContext context) : base(context)
        {
        }

        public override ActionResult Get()
        {
            var list = _context.Set<FaseCadeiaProduto>().Include(fcd => fcd.TiposDetalhados).ToList();
            return Ok(list);
        }
    }
}