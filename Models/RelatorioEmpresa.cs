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
        public AlocacaoRh AlocacaoRh { get; set; }
        public AlocacaoRm AlocacaoRm { get; set; }
        public RecursoHumano RecursoHumano { get; set; }
        public RecursoMaterial RecursoMaterial { get; set; }
        public decimal Valor { get; set; }
    }
    public class RelatorioEmpresaCsv
    {
        public int Id { get; set; }
        public string Etapa { get; set; }
        public string NomeRecurso { get; set; }
        public string CPF { get; set; }
        public string FUNCAO { get; set; }
        public string TITULACAO { get; set; }
        public string CL { get; set; }
        public string CategoriaContabil { get; set; }
        public string EspecificacaoTecnica { get; set; }
        public string Justificativa { get; set; }
        public string EntidadePagadora { get; set; }
        public string CnpjEntidadePagadora { get; set; }
        public string EntidadeRecebedora { get; set; }
        public string CnpjEntidadeRecebedora { get; set; }
        public int QtdHoras { get; set; }
        public decimal ValorHora { get; set; }
        public int Unidades { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal ValorTotal { get; set; }
    }
}