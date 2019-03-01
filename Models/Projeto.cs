using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace APIGestor.Models
{
    public class Projeto
    {
        public DateTime Created { get; set; }
        private int _id;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id
        {
            get => _id;
            set => _id = value;
        }

        private string _titulo;
        public string Titulo
        {
            get => _titulo;
            set => _titulo = value?.Trim();
        }
        public TipoProjeto Tipo { get; set; }
        [NotMapped]
        public string TipoValor { get => Enum.GetName(typeof(TipoProjeto),Tipo); }
        public DateTime? DataInicio { get; set; }
        [NotMapped]
        public DateTime? DataFim { get; set; }
        public string Codigo { get; set; }
        public string TituloDesc { get; set; }
        public string Numero { get;set;}
        public int? CatalogEmpresaId { get; set; }
        public CatalogEmpresa CatalogEmpresa { get; set; }
        public int? CatalogStatusId { get; set; }
        public CatalogStatus CatalogStatus { get; set; }
        public int? CatalogSegmentoId { get; set; }
        public CatalogSegmento CatalogSegmento { get; set; }
        public bool? AvaliacaoInicial { get; set; }
        public CompartResultados? CompartResultados { get; set; }
        [NotMapped]
        public string CompartResultadosValor { 
            get => CompartResultados == null ? null : Enum.GetName(typeof(CompartResultados),CompartResultados);
        }
        public string Motivacao { get; set; }
        public string Originalidade { get; set; }
        public string Aplicabilidade { get; set; }
        public string Relevancia { get; set; }
        public string Razoabilidade { get; set; }
        public string Pesquisas { get; set; }
        public List<Produto> Produtos { get; set; }
        public List<RecursoHumano> RecursosHumanos { get; set; }
        public List<AlocacaoRh> AlocacoesRh { get; set; }
        public List<RecursoMaterial> RecursosMateriais { get; set; }
        public List<AlocacaoRm> AlocacoesRm { get; set; }
        public List<Etapa> Etapas { get; set; }
        [NotMapped]
        public Etapa Etapa { get; set; }
        public Tema Tema { get; set; }
        [JsonIgnore]
        public List<UserProjeto> UsersProjeto { get; set; }
        public List<Empresa> Empresas { get; set; }
        public RelatorioFinal RelatorioFinal { get; set; }
    }
    public enum TipoProjeto
    {
        PD = 1,
        PG = 2
    }
    public enum CompartResultados
    {
        DP = 1,
        EE = 2,
        EX = 3,
        CE = 4
    }
}