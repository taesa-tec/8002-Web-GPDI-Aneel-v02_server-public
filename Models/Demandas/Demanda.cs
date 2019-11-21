using System;
using System.Linq;
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
    public enum Etapa
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
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string CriadorId { get; set; }
        public ApplicationUser Criador { get; set; }
        public string SuperiorDiretoId { get; set; }
        public ApplicationUser SuperiorDireto { get; set; }

        public string RevisorId { get; set; }
        public ApplicationUser Revisor { get; set; }
        public Etapa EtapaAtual { get; set; }
        public DemandaStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CaptacaoDate { get; set; }
        public List<DemandaComentario> Comentarios { get; set; }


        public void ProximaEtapa()
        {
            if (this.EtapaAtual > Etapa.Elaboracao && String.IsNullOrWhiteSpace(SuperiorDiretoId))
            {
                throw new DemandaException("A demanda não tem superior direto definido");
            }

            if (this.EtapaAtual >= Etapa.RevisorPendente && String.IsNullOrWhiteSpace(RevisorId))
            {
                throw new DemandaException("Não é possível avançar para a proxíma etapa sem revisor");
            }

            if (this.EtapaAtual < Etapa.AprovacaoDiretor)
            {
                this.EtapaAtual++;
                this.Status = DemandaStatus.EmElaboracao;
            }
            else
            {
                Status = DemandaStatus.Aprovada;
            }
        }
        public void EtapaAnterior()
        {
            if (this.EtapaAtual > Etapa.Elaboracao)
            {
                this.EtapaAtual--;
                this.Status = DemandaStatus.EmElaboracao;
            }
        }
        public void IrParaEtapa(Etapa etapa)
        {
            if (etapa > EtapaAtual)
                ProximaEtapa();
            else if (etapa < EtapaAtual)
                EtapaAnterior();

            if (EtapaAtual != etapa)
                IrParaEtapa(etapa);
        }
        public void ReprovarReiniciar()
        {
            this.EtapaAtual = Etapa.Elaboracao;
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
                return Enum.GetName(typeof(Etapa), this.EtapaAtual);
            }
        }

    }


}