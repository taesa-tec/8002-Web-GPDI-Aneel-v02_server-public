using System.Collections.Generic;

namespace APIGestor.Models
{
    public class RelatorioAtividade
    {
        public List<RelatorioAtividades> Atividades { get; set; }
        public int Total { get; set; }
        public decimal? Valor { get; set; }

    }
    public class RelatorioAtividades
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