using System.Collections.Generic;

namespace APIGestor.Models
{
    public class RelatorioEmpresa
    {
        public List<RelatorioEmpresas> Empresas { get; set; }
        public int Total { get; set; }
        public decimal Valor { get; set; }

    }
    public class RelatorioEmpresas
    {
        public string Nome { get; set; }
        public List<RelatorioEmpresaCategorias> Relatorios { get; set; }
        public int Total { get; set; }
        public decimal Valor { get; set; }
    }
    public class RelatorioEmpresaCategorias
    {
        public CategoriaContabil CategoriaContabil { get; set; }
        public string Desc {get; set;}
        public List<RelatorioEmpresaItems> Items { get; set; }
        public int Total { get; set; }
        public decimal Valor { get; set; }
    }
    public class RelatorioEmpresaItems{
        public int AlocacaoId { get; set; }
        public string Desc { get; set; }
        public Etapa Etapa { get; set; }
        public RecursoHumano RecursoHumano { get; set; }
        public RecursoMaterial RecursoMaterial { get; set; }
        public decimal Valor { get; set; }
    }
}