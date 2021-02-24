using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PeD.Data;

namespace PeD.Controllers.Catalogo
{
    public abstract class CatalogController<T> : Controller where T : class
    {
        protected GestorDbContext _context;

        protected CatalogController(GestorDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public virtual ActionResult Get()
        {
            return Ok(_context.Set<T>().ToList());
        }
    }
}