using System.Collections.Generic;
using PeD.Core.Models.Catalogs;
using CatalogAtividade = PeD.Projetos.Models.Catalogs.CatalogAtividade;

namespace PeD.Projetos.Models.Projetos
{
    public class RelatorioAtividades
    {
        public List<RelatorioAtividade> Atividades { get; set; }
        public int Total { get; set; }
        public decimal? Valor { get; set; }

    }
    public class RelatorioAtividade
    {
        public string Nome { get; set; }
        public CatalogAtividade Atividade { get; set; }
        public List<RelatorioAtividadeEmpresas> Empresas { get; set; }
        public int Total { get; set; }
        public decimal? Valor { get; set; }
    }
    public class RelatorioAtividadeEmpresas
    {
        public Empresa Empresa { get; set; }
        public string Desc {get; set;}
        public List<RelatorioAtividadeItems> Items { get; set; }
        public int Total { get; set; }
        public decimal? Valor { get; set; }
    }
    public class RelatorioAtividadeItems{
        public int AlocacaoId { get; set; }
        public string Desc { get; set; }
        public RecursoHumano RecursoHumano{ get; set; }
        public RecursoMaterial RecursoMaterial{ get; set; }
        public string CategoriaContabil { get; set; }
        public decimal? Valor { get; set; }
    }
}