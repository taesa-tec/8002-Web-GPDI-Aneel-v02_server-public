using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using APIGestor.Data;
using APIGestor.Models;
using Microsoft.AspNetCore.Identity;
using APIGestor.Security;

namespace APIGestor.Business
{
    public class CatalogService
    {
        private GestorDbContext _context;

        public CatalogService(GestorDbContext context)
        {
            _context = context;
        }

        public IEnumerable<CatalogTema> ListarCatalogTema()
        {
            var Catalog = _context.CatalogTema
                .Include("SubTemas")
                .OrderBy(p => p.Id)
                .ToList();
            return Catalog;
        }
        public IEnumerable<CatalogStatus> ListarCatalogStatus()
        {
            var Catalog = _context.CatalogStatus
                .OrderBy(p => p.Id)
                .ToList();
            return Catalog;
        }
        public IEnumerable<CatalogSegmento> ListarCatalogSegmento()
        {
            var Catalog = _context.CatalogSegmentos
                .OrderBy(p => p.Id)
                .ToList();
            return Catalog;
        }
        public IEnumerable<CatalogUserPermissao> ListarCatalogUserPermissao()
        {
            var Catalog = _context.CatalogUserPermissoes
                .OrderBy(p => p.Id)
                .ToList();
            return Catalog;
        }
        public IEnumerable<CatalogEmpresa> ListarCatalogEmpresa()
        {
            var Catalog = _context.CatalogEmpresas
                .OrderBy(p => p.Id)
                .ToList();
            return Catalog;
        }
        public IEnumerable<CatalogEstado> ListarCatalogEstados()
        {
            var Catalog = _context.CatalogEstados
                .OrderBy(p => p.Nome)
                .ToList();
            return Catalog;
        }
    }
}