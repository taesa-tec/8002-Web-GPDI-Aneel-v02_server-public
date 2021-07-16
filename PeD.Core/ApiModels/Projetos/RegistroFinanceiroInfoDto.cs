using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using PeD.Core.Converters;
using PeD.Core.Models;
using PeD.Core.Models.Projetos;
using TaesaCore.Models;

namespace PeD.Core.ApiModels.Projetos
{
    public class RegistroFinanceiroInfoDto
    {
        public int Id { get; set; }
        public int ProjetoId { get; set; }
        public string AuthorId { get; set; }
        public string Tipo { get; set; }
        public string Status { get; set; }
        public string Beneficiado { get; set; }
        public string CnpjBeneficiado { get; set; }

        [JsonConverter(typeof(YesOrNoConverter), "Sim", "Não")]
        public bool? EquipaLaboratorioExistente { get; set; }

        [JsonConverter(typeof(YesOrNoConverter), "Sim", "Não")]
        public bool? EquipaLaboratorioNovo { get; set; }

        [JsonConverter(typeof(YesOrNoConverter), "Sim", "Não")]
        public bool? IsNacional { get; set; }

        [JsonConverter(typeof(DateConverter))] public DateTime MesReferencia { get; set; }
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        [JsonConverter(typeof(DateConverter))] public DateTime DataDocumento { get; set; }
        public int? CategoriaContabilId { get; set; }
        public string CategoriaContabil { get; set; }

        public decimal Valor { get; set; }
        public string EspecificaoTecnica { get; set; }
        public string AtividadeRealizada { get; set; }
        public short Etapa { get; set; }
        public string FuncaoEtapa { get; set; }
        public string NomeItem { get; set; }
        public int? RecursoHumanoId { get; set; }
        public int? RecursoMaterialId { get; set; }
        public string Recurso { get; set; }
        public string CNPJParc { get; set; }
        public int FinanciadoraId { get; set; }
        public string FinanciadoraFuncao { get; set; }
        public string FinanciadoraCodigo { get; set; }
        public string Financiadora { get; set; }
        public string CNPJExec { get; set; }
        public int RecebedoraId { get; set; }
        public string RecebedoraCodigo { get; set; }
        public string RecebedoraFuncao { get; set; }
        public string Recebedora { get; set; }
        public string CategoriaContabilCodigo { get; set; }

        public decimal QuantidadeHoras { get; set; }

        public decimal Custo { get; set; }
    }

    public class RegistroObservacaoDto : BaseEntity
    {
        public DateTime CreatedAt { get; set; }
        public int RegistroId { get; set; }

        public string AuthorId { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
    }
}