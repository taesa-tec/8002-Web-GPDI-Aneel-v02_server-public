using System;

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
        ReciboSemCNPJ
    }

    public enum StatusRegistro
    {
        Pendente = 0,
        Aprovado = 1,
        Reprovado = 2
    }

    public class RegistroFinanceiro : ProjetoNode
    {
        public string Tipo { get; set; }
        public StatusRegistro Status { get; set; }

        #region Empresa Financiadora

        public int? FinanciadoraId { get; set; }
        public Empresa Financiadora { get; set; }

        public int? CoExecutorFinanciadorId { get; set; }
        public CoExecutor CoExecutorFinanciador { get; set; }

        #endregion

        public int MesReferencia { get; set; }

        public Etapa Etapa { get; set; }
        public int EtapaId { get; set; }

        #region Documento

        public TipoDocumento TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public DateTime DataDocumento { get; set; }

        #endregion

        public string ObservacaoInterna { get; set; }
        public int? ComprovanteId { get; set; }
        public FileUpload Comprovante { get; set; }
    }
}