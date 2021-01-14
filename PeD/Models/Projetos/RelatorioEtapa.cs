using System.Collections.Generic;

namespace PeD.Models.Projetos
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
        public RecursoHumano RecursoHumano{ get; set; }
        public RecursoMaterial RecursoMaterial{ get; set; }
        public string CategoriaContabil { get; set; }
        public decimal Valor { get; set; }
    }
}