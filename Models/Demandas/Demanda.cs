using System;
using System.Linq;
using System.Collections.Generic;


namespace APIGestor.Models.Demandas
{
    public enum EtapaStatus
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
        public Etapa EtapaAtual { get; set; }
        public EtapaStatus EtapaStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<DemandaComentario> Comentarios { get; set; }
        public List<DemandaFile> Files { get; set; }

        public void ProximaEtapa()
        {
            if (this.EtapaAtual < Etapa.Captacao)
                this.EtapaAtual++;
        }
        public void EtapaAnterior()
        {
            if (this.EtapaAtual > Etapa.Elaboracao)
                this.EtapaAtual--;
        }

    }
}