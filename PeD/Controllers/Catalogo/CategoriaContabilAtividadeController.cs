using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeD.Core.Models.Catalogos;
using PeD.Data;
using Swashbuckle.AspNetCore.Annotations;

namespace PeD.Controllers.Catalogo
{
    [SwaggerTag("Catalogo")]
    [Route("api/Catalogo/[controller]")]
    [ApiController]
    [Authorize("Bearer")]
    public class CategoriaContabilAtividadeController : CatalogController<CategoriaContabilAtividade>
    {
        public CategoriaContabilAtividadeController(GestorDbContext context) : base(context)
        {
        }
    }
}