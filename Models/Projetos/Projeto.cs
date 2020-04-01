using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using APIGestor.Attributes;
using APIGestor.Models.Catalogs;
using APIGestor.Models.Projetos.Resultados;
using Newtonsoft.Json;

namespace APIGestor.Models.Projetos {
    public class Projeto {
        public DateTime Created { get; set; }
        private int _id;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id {
            get => _id;
            set => _id = value;
        }

        private string _titulo;
        [Logger("Titulo")]
        public string Titulo {
            get => _titulo;
            set => _titulo = value?.Trim();
        }
        public TipoProjeto Tipo { get; set; }
        [NotMapped]
        public string TipoValor { get => Enum.GetName(typeof(TipoProjeto), Tipo); }
        [Logger("Data de início")]
        public DateTime? DataInicio { get; set; }
        [NotMapped]
        public DateTime? DataFim { get; set; }

        [Logger("Código")]
        public string Codigo { get; set; }

        [Logger("Título descritivo")]
        public string TituloDesc { get; set; }

        [Logger("Número")]
        public string Numero { get; set; }
        [Logger("Empresa", "CatalogEmpresa.Nome")]
        public int? CatalogEmpresaId { get; set; }
        public CatalogEmpresa CatalogEmpresa { get; set; }
        [Logger("Status", "CatalogStatus.Status")]
        public int? CatalogStatusId { get; set; }
        public CatalogStatus CatalogStatus { get; set; }
        [Logger("Segmento", "CatalogSegmento.Nome")]
        public int? CatalogSegmentoId { get; set; }
        public CatalogSegmento CatalogSegmento { get; set; }
        public bool? AvaliacaoInicial { get; set; }
        public CompartResultados? CompartResultados { get; set; }
        [NotMapped]
        public string CompartResultadosValor {
            get => CompartResultados == null ? null : Enum.GetName(typeof(CompartResultados), CompartResultados);
        }
        [Logger("Motivação")]
        public string Motivacao { get; set; }
        [Logger("Originalidade")]
        public string Originalidade { get; set; }
        [Logger("Aplicabilidade")]
        public string Aplicabilidade { get; set; }
        [Logger("Relevância")]
        public string Relevancia { get; set; }
        [Logger("Razoabilidade")]
        public string Razoabilidade { get; set; }
        [Logger("Pesquisas")]
        public string Pesquisas { get; set; }
        public List<Produto> Produtos { get; set; }
        public List<RecursoHumano> RecursosHumanos { get; set; }
        public List<AlocacaoRh> AlocacoesRh { get; set; }
        public List<RecursoMaterial> RecursosMateriais { get; set; }
        public List<AlocacaoRm> AlocacoesRm { get; set; }
        public List<Etapa> Etapas { get; set; }
        [NotMapped]
        public Etapa Etapa { get; set; }
        public AtividadesGestao Atividades { get; set; }
        public Tema Tema { get; set; }
        [JsonIgnore]
        public List<UserProjeto> UsersProjeto { get; set; }
        public List<Empresa> Empresas { get; set; }
        public RelatorioFinal RelatorioFinal { get; set; }
        public List<RegistroFinanceiro> RegistroFinanceiro { get; set; }
        public List<ResultadoCapacitacao> ResultadosCapacitacao { get; set; }
        public List<ResultadoProducao> ResultadosProducao { get; set; }
        public List<ResultadoIntelectual> ResultadosIntelectual { get; set; }
        public List<ResultadoInfra> ResultadosInfra { get; set; }
        public List<ResultadoSocioAmbiental> ResultadosSocioAmbiental { get; set; }
        public List<ResultadoEconomico> ResultadosEconomico { get; set; }
    }
    public enum TipoProjeto {
        PD = 1,
        PG = 2
    }
    public enum CompartResultados {
        DP = 1,
        EE = 2,
        EX = 3,
        CE = 4
    }
}