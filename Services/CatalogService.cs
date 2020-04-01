using System.Collections.Generic;
using System.Linq;
using APIGestor.Data;
using APIGestor.Models.Catalogs;
using APIGestor.Models.Projetos;
using Microsoft.EntityFrameworkCore;

namespace APIGestor.Services
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
        public IEnumerable<CatalogPais> ListarCatalogPaises()=>
            _context.CatalogPaises
                .OrderBy(p => p.Nome)
                .ToList();
        public IEnumerable<CatalogCategoriaContabilGestao> ListarCatalogCategoriaContabilGestao()=>
            _context.CatalogCategoriaContabilGestao
                .Include("Atividades")
                .Select(st=>new CatalogCategoriaContabilGestao{
                    Id = st.Id,
                    Nome = st.Nome,
                    Valor = st.Valor,
                    Atividades = st.Atividades.OrderBy(sb=>sb.Nome).OrderBy(sb=>sb.Nome).ToList()
                })
                .OrderBy(q => q.Nome)
                .ToList();
        public IEnumerable<CatalogProdutoFaseCadeia> ListarProdutoFasesCadeia()=>
            _context.CatalogProdutoFaseCadeia
                .Include("TiposDetalhados")
                .Select(st=>new CatalogProdutoFaseCadeia{
                    Id = st.Id,
                    Nome = st.Nome,
                    Valor = st.Valor,
                    TiposDetalhados = st.TiposDetalhados.OrderBy(sb=>sb.Nome).OrderBy(sb=>sb.Nome).ToList()
                })
                .OrderBy(q => q.Nome)
                .ToList();
    }
}