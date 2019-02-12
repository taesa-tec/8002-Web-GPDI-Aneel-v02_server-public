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

        public IEnumerable<CatalogTema> ListarCatalogTema()=>
            _context.CatalogTema
                .Include("SubTemas")
                .Select(st=>new CatalogTema{
                    Id = st.Id,
                    Nome = st.Nome,
                    Valor = st.Valor,
                    SubTemas = st.SubTemas.OrderBy(sb=>sb.Order).OrderBy(sb=>sb.Valor).ToList()
                })
                .OrderBy(q => q.Order)
                .ToList();
        public IEnumerable<CatalogStatus> ListarCatalogStatus()=>
            _context.CatalogStatus
                .OrderBy(p => p.Id)
                .ToList();
        public IEnumerable<CatalogSegmento> ListarCatalogSegmento()=>
            _context.CatalogSegmentos
                .OrderBy(p => p.Id)
                .ToList();
        public IEnumerable<CatalogUserPermissao> ListarCatalogUserPermissao()=>
            _context.CatalogUserPermissoes
                .OrderBy(p => p.Id)
                .ToList();
        public IEnumerable<CatalogEmpresa> ListarCatalogEmpresa()=>
            _context.CatalogEmpresas
                .OrderBy(p => p.Id)
                .ToList();
        public IEnumerable<CatalogEstado> ListarCatalogEstados()=>
            _context.CatalogEstados
                .OrderBy(p => p.Nome)
                .ToList();
    }
}