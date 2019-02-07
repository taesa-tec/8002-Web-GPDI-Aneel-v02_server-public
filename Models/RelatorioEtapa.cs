using System.Collections.Generic;

namespace APIGestor.Models
{
    public class RelatorioEtapa
    {
        public List<RelatorioEtapas> Etapas { get; set; }
        public int Total { get; set; }
        public decimal Valor { get; set; }

    }
    public class RelatorioEtapas
    {
        public string Nome { get; set; }
        public Etapa Etapa { get; set; }
        public List<RelatorioEtapaEmpresas> Empresas { get; set; }
        public int Total { get; set; }
        public decimal Valor { get; set; }
    }
    public class RelatorioEtapaEmpresas
    {
        public Empresa Empresa { get; set; }
        public string Desc {get; set;}
        public List<RelatorioEtapaItems> Items { get; set; }
        public int Total { get; set; }
        public decimal Valor { get; set; }
    }
    public class RelatorioEtapaItems{
        public int AlocacaoId { get; set; }
        public string Desc { get; set; }
        public CategoriaContabil CategoriaContabil { get; set; }
        public decimal Valor { get; set; }
    }
}