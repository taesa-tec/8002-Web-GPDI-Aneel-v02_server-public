using System;
using System.Collections.Generic;
using PeD.Core.Exceptions.Demandas;
using TaesaCore.Models;

namespace PeD.Core.Models.Demandas
{
    public enum DemandaStatus
    {
        EmElaboracao,
        Reprovada,
        ReprovadaPermanente,
        Aprovada,
        Concluido,
        Pendente
    }

    public enum DemandaEtapa
    {
        Elaboracao = 0,
        PreAprovacao = 1,
        RevisorPendente = 2,
        AprovacaoRevisor = 3,
        AprovacaoCoordenador = 4,
        AprovacaoGerente = 5,
        AprovacaoDiretor = 6,
        Captacao = 7
    }

    public class Demanda : BaseEntity
    {
        protected static Dictionary<DemandaEtapa, string> _etapaDesc = new Dictionary<DemandaEtapa, string>
        {
            {DemandaEtapa.Elaboracao, "Elaboração"},
            {DemandaEtapa.PreAprovacao, "Pre-Aprovação"},
            {DemandaEtapa.RevisorPendente, "Revisor Pendente"},
            {DemandaEtapa.AprovacaoRevisor, "Aprovação Revisor"},
            {DemandaEtapa.AprovacaoCoordenador, "Aprovação Coordenador"},
            {DemandaEtapa.AprovacaoGerente, "Aprovação Gerente"},
            {DemandaEtapa.AprovacaoDiretor, "Aprovação Diretor"},
            {DemandaEtapa.Captacao, "Enviado para captação"}
        };

        public string Titulo { get; set; }
        public string CriadorId { get; set; }
        public ApplicationUser Criador { get; set; }
        public string SuperiorDiretoId { get; set; }
        public ApplicationUser SuperiorDireto { get; set; }

        public string RevisorId { get; set; }
        public ApplicationUser Revisor { get; set; }
        public DemandaEtapa EtapaAtual { get; set; }
        public DemandaStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CaptacaoDate { get; set; }
        public List<DemandaComentario> Comentarios { get; set; }

        public int? EspecificacaoTecnicaFileId { get; set; }
        public FileUpload EspecificacaoTecnicaFile { get; set; }

        public void ValidarContinuidade()
        {
            if (EtapaAtual > DemandaEtapa.Elaboracao && string.IsNullOrWhiteSpace(SuperiorDiretoId))
            {
                throw new DemandaException("A demanda não tem superior direto definido");
            }

            if (EtapaAtual >= DemandaEtapa.RevisorPendente && string.IsNullOrWhiteSpace(RevisorId))
            {
                throw new DemandaException("Não é possível avançar para a proxíma etapa sem revisor");
            }
        }

        public void ProximaEtapa()
        {
            ValidarContinuidade();

            if (EtapaAtual < DemandaEtapa.AprovacaoDiretor)
            {
                EtapaAtual++;
                if (EtapaAtual == DemandaEtapa.RevisorPendente && !string.IsNullOrWhiteSpace(RevisorId))
                {
                    EtapaAtual++;
                }

                Status = DemandaStatus.EmElaboracao;
            }
            else
            {
                Status = DemandaStatus.Aprovada;
            }
        }

        public void EtapaAnterior()
        {
            if (EtapaAtual > DemandaEtapa.Elaboracao)
            {
                EtapaAtual--;
                Status = DemandaStatus.EmElaboracao;
            }
        }

        public void IrParaEtapa(DemandaEtapa demandaEtapa)
        {
            if (demandaEtapa > EtapaAtual)
                ProximaEtapa();
            else if (demandaEtapa < EtapaAtual)
                EtapaAnterior();

            if (EtapaAtual != demandaEtapa)
                IrParaEtapa(demandaEtapa);
        }

        public void ReprovarReiniciar()
        {
            EtapaAtual = DemandaEtapa.Elaboracao;
            Status = DemandaStatus.Reprovada;
        }

        public void ReprovarPermanente()
        {
            Status = DemandaStatus.ReprovadaPermanente;
        }

        public string EtapaStatusText
        {
            get { return Enum.GetName(typeof(DemandaStatus), Status); }
        }

        public string EtapaAtualText
        {
            get { return Enum.GetName(typeof(DemandaEtapa), EtapaAtual); }
        }

        public string EtapaDesc
        {
            get { return _etapaDesc[EtapaAtual]; }
        }
    }
}