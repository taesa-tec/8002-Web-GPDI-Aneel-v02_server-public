using System;
using System.Collections.Generic;
using APIGestor.Exceptions.Demandas;

namespace APIGestor.Models.Demandas
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

    public class Demanda
    {
        protected static Dictionary<DemandaEtapa, string> _etapaDesc = new Dictionary<DemandaEtapa, string>()
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
        public int Id { get; set; }
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


        public void ProximaEtapa()
        {
            if (this.EtapaAtual > DemandaEtapa.Elaboracao && String.IsNullOrWhiteSpace(SuperiorDiretoId))
            {
                throw new DemandaException("A demanda não tem superior direto definido");
            }

            if (this.EtapaAtual >= DemandaEtapa.RevisorPendente && String.IsNullOrWhiteSpace(RevisorId))
            {
                throw new DemandaException("Não é possível avançar para a proxíma etapa sem revisor");
            }

            if (this.EtapaAtual < DemandaEtapa.AprovacaoDiretor)
            {
                this.EtapaAtual++;
                if (this.EtapaAtual == DemandaEtapa.RevisorPendente && !String.IsNullOrWhiteSpace(RevisorId))
                {
                    this.EtapaAtual++;
                }
                this.Status = DemandaStatus.EmElaboracao;
            }
            else
            {
                Status = DemandaStatus.Aprovada;
            }
        }
        public void EtapaAnterior()
        {
            if (this.EtapaAtual > DemandaEtapa.Elaboracao)
            {
                this.EtapaAtual--;
                this.Status = DemandaStatus.EmElaboracao;
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
            this.EtapaAtual = DemandaEtapa.Elaboracao;
            this.Status = DemandaStatus.Reprovada;
        }
        public void ReprovarPermanente()
        {
            this.Status = DemandaStatus.ReprovadaPermanente;
        }

        public string EtapaStatusText
        {
            get
            {
                return Enum.GetName(typeof(DemandaStatus), this.Status);
            }
        }
        public string EtapaAtualText
        {
            get
            {
                return Enum.GetName(typeof(DemandaEtapa), this.EtapaAtual);
            }
        }

        public string EtapaDesc
        {
            get
            {
                return Demanda._etapaDesc[EtapaAtual];
            }
        }

    }


}