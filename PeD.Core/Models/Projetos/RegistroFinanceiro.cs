using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using PeD.Core.Attributes;
using TaesaCore.Models;

// ReSharper disable InconsistentNaming

namespace PeD.Core.Models.Projetos
{
    public enum TipoDocumento
    {
        CupomFiscal,
        Declaracao,
        Fatura,
        Guia,
        Nota,
        Recibo,
        Reserva,
        ReciboSemCnpj
    }

    public enum StatusRegistro
    {
        Pendente,
        Aprovado,
        Reprovado
    }

    public class RegistroFinanceiro : ProjetoNode
    {
        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }
        public string Tipo { get; set; }
        public StatusRegistro Status { get; set; }
        [Column(TypeName = "decimal(18, 2)")] public decimal Valor { get; set; }

        #region Empresa Financiadora

        public int FinanciadoraId { get; set; }
        public Empresa Financiadora { get; set; }

        #endregion

        public DateTime MesReferencia { get; set; }

        public Etapa Etapa { get; set; }
        public int EtapaId { get; set; }

        #region Documento

        public TipoDocumento TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public DateTime DataDocumento { get; set; }

        #endregion

        public int? ComprovanteId { get; set; }
        public FileUpload Comprovante { get; set; }

        public List<RegistroObservacao> Observacoes { get; set; }
    }

    public class RegistroFinanceiroInfo
    {
        public int Id { get; set; }
        public int ProjetoId { get; set; }
        public string AuthorId { get; set; }
        public string Tipo { get; set; }
        public StatusRegistro Status { get; set; }
        public string Beneficiado { get; set; }
        public string CnpjBeneficiado { get; set; }
        public bool? EquipaLaboratorioExistente { get; set; }
        public bool? EquipaLaboratorioNovo { get; set; }
        public bool? IsNacional { get; set; }
        public DateTime MesReferencia { get; set; }
        public TipoDocumento TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public DateTime DataDocumento { get; set; }
        public int? CategoriaContabilId { get; set; }
        [XlxsColumn("Categoria", 3)] public string CategoriaContabil { get; set; }

        [XlxsColumn(9, Type = typeof(decimal))]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Valor { get; set; }
        public string EspecificaoTecnica { get; set; }
        public string AtividadeRealizada { get; set; }
        [XlxsColumn("Etapa", 1)] public short Etapa { get; set; }
        public string FuncaoEtapa { get; set; }
        [XlxsColumn("Nome", 0)] public string NomeItem { get; set; }
        public int? RecursoHumanoId { get; set; }
        public int? RecursoMaterialId { get; set; }
        public string Recurso { get; set; }
        


        
        
        public string CNPJParc { get; set; }
        public int FinanciadoraId { get; set; }
        public Funcao FinanciadoraFuncao { get; set; }
        public string FinanciadoraCodigo { get; set; }
        [XlxsColumn(4)] public string Financiador { get; set; }
        public string CNPJExec { get; set; }
        public int RecebedoraId { get; set; }
        public string RecebedoraCodigo { get; set; }
        public Funcao RecebedoraFuncao { get; set; }
        [XlxsColumn(4)] public string Recebedor { get; set; }

        public string CategoriaContabilCodigo { get; set; }
        


        [XlxsColumn("Quantidade / Horas", 8, Type = typeof(decimal))]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal QuantidadeHoras { get; set; }

        [XlxsColumn(10, Type = typeof(decimal))]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Custo { get; set; }
    }

    public class RegistroObservacao : BaseEntity
    {
        public DateTime CreatedAt { get; set; }
        public int RegistroId { get; set; }

        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }
        public string Content { get; set; }
    }
}