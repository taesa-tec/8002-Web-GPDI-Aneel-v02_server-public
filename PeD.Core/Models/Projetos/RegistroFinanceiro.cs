using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TaesaCore.Models;

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
        public string Tipo { get; set; }
        public StatusRegistro Status { get; set; }
        [Column(TypeName = "decimal(18, 2)")] public decimal Valor { get; set; }

        #region Empresa Financiadora

        public int? FinanciadoraId { get; set; }
        public Empresa Financiadora { get; set; }

        public int? CoExecutorFinanciadorId { get; set; }
        public CoExecutor CoExecutorFinanciador { get; set; }

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
        public string Tipo { get; set; }
        public string AtividadeRealizada { get; set; }


        public string FuncaoEtapa { get; set; }
        public string EspecificaoTecnica { get; set; }
        public string NomeItem { get; set; }
        public string Beneficiado { get; set; }
        public string CnpjBeneficiado { get; set; }
        public bool? EquipaLaboratorioExistente { get; set; }
        public bool? EquipaLaboratorioNovo { get; set; }
        public bool? IsNacional { get; set; }
        public DateTime MesReferencia { get; set; }
        public int ProjetoId { get; set; }
        public StatusRegistro Status { get; set; }
        public TipoDocumento TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public DateTime DataDocumento { get; set; }
        public string Recurso { get; set; }
        public int? RecursoHumanoId { get; set; }
        public int? RecursoMaterialId { get; set; }
        public int? FinanciadoraId { get; set; }
        public int? CoExecutorFinanciadorId { get; set; }
        public string FinanciadorCode { get; set; }
        public int? RecebedoraId { get; set; }
        public int? CoExecutorRecebedorId { get; set; }
        public int? CategoriaContabilId { get; set; }
        public string CategoriaContabil { get; set; }
        public string CategoriaContabilCodigo { get; set; }
        public string Financiador { get; set; }
        public string Recebedor { get; set; }
        [Column(TypeName = "decimal(18, 2)")] public decimal Valor { get; set; }
        [Column(TypeName = "decimal(18, 2)")] public decimal QuantidadeHoras { get; set; }
        [Column(TypeName = "decimal(18, 2)")] public decimal Custo { get; set; }
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