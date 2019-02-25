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
    public class RelatorioEmpresaCsv
    {
        public int Id;
        public string Etapa;
        public string NomeRecurso;
        public string CPF;
        public string FUNCAO;
        public string TITULACAO;
        public string CL;
        public string CategoriaContabil;
        public string EspecificacaoTecnica;
        public string Justificativa;
        public string EntidadePagadora;
        public string CnpjEntidadePagadora;
        public string EntidadeRecebedora;
        public string CnpjEntidadeRecebedora;
        public int QtdHoras;
        public string ValorHora;
        public int Unidades;
        public string ValorUnitario;
        public string ValorTotal;
    }
}