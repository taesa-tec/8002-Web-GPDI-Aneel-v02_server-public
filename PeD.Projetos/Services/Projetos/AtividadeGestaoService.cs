using System.Linq;
using Microsoft.AspNetCore.Authorization;
using PeD.Core.Models.Projetos;
using PeD.Data;

namespace PeD.Services.Projetos {
    public class AtividadeGestaoService : BaseService {

        public AtividadeGestaoService( GestorDbContext context, IAuthorizationService authorization, LogService logService ) : base(context, authorization, logService) {

        }

        public AtividadesGestao Obter( int ProjetoId ) {
            if(ProjetoId > 0) {
                return _context.AtividadesGestao
                    .Where(p => p.ProjetoId == ProjetoId).FirstOrDefault();
            }
            else
                return null;
        }
        public Resultado Incluir( AtividadesGestao dados ) {
            Resultado resultado = DadosValidos(dados);
            resultado.Acao = "Inclusão de Atividades";

            if(resultado.Inconsistencias.Count == 0) {
                _context.AtividadesGestao.Add(dados);
                _context.SaveChanges();
                resultado.Id = dados.Id.ToString();
            }
            return resultado;
        }

        public Resultado Atualizar( AtividadesGestao dados ) {
            Resultado resultado = DadosValidos(dados);
            resultado.Acao = "Atualização de Atividades";

            if(resultado.Inconsistencias.Count == 0) {
                AtividadesGestao AtividadesGestao = _context.AtividadesGestao.Where(
                    p => p.Id == dados.Id).FirstOrDefault();

                if(AtividadesGestao == null) {
                    resultado.Inconsistencias.Add(
                        "Atividades não encontradas");
                }
                else {
                    AtividadesGestao.DedicacaoHorario = dados.DedicacaoHorario == null ? AtividadesGestao.DedicacaoHorario : dados.DedicacaoHorario;
                    AtividadesGestao.ParticipacaoMembros = dados.ParticipacaoMembros == null ? AtividadesGestao.ParticipacaoMembros : dados.ParticipacaoMembros;
                    AtividadesGestao.DesenvFerramenta = dados.DesenvFerramenta == null ? AtividadesGestao.DesenvFerramenta : dados.DesenvFerramenta;
                    AtividadesGestao.ProspTecnologica = dados.ProspTecnologica == null ? AtividadesGestao.ProspTecnologica : dados.ProspTecnologica;
                    AtividadesGestao.DivulgacaoResultados = dados.DivulgacaoResultados == null ? AtividadesGestao.DivulgacaoResultados : dados.DivulgacaoResultados;
                    AtividadesGestao.ParticipacaoTecnicos = dados.ParticipacaoTecnicos == null ? AtividadesGestao.ParticipacaoTecnicos : dados.ParticipacaoTecnicos;
                    AtividadesGestao.BuscaAnterioridade = dados.BuscaAnterioridade == null ? AtividadesGestao.BuscaAnterioridade : dados.BuscaAnterioridade;
                    AtividadesGestao.ContratacaoAuditoria = dados.ContratacaoAuditoria == null ? AtividadesGestao.ContratacaoAuditoria : dados.ContratacaoAuditoria;
                    AtividadesGestao.ApoioCitenel = dados.ApoioCitenel == null ? AtividadesGestao.ApoioCitenel : dados.ApoioCitenel;

                    //Resultado Atividades
                    AtividadesGestao.ResDedicacaoHorario = dados.ResDedicacaoHorario == null ? AtividadesGestao.ResDedicacaoHorario : dados.ResDedicacaoHorario;
                    AtividadesGestao.ResParticipacaoMembros = dados.ResParticipacaoMembros == null ? AtividadesGestao.ResParticipacaoMembros : dados.ResParticipacaoMembros;
                    AtividadesGestao.ResDesenvFerramenta = dados.ResDesenvFerramenta == null ? AtividadesGestao.ResDesenvFerramenta : dados.ResDesenvFerramenta;
                    AtividadesGestao.ResProspTecnologica = dados.ResProspTecnologica == null ? AtividadesGestao.ResProspTecnologica : dados.ResProspTecnologica;
                    AtividadesGestao.ResDivulgacaoResultados = dados.ResDivulgacaoResultados == null ? AtividadesGestao.ResDivulgacaoResultados : dados.ResDivulgacaoResultados;
                    AtividadesGestao.ResParticipacaoTecnicos = dados.ResParticipacaoTecnicos == null ? AtividadesGestao.ResParticipacaoTecnicos : dados.ResParticipacaoTecnicos;
                    AtividadesGestao.ResBuscaAnterioridade = dados.ResBuscaAnterioridade == null ? AtividadesGestao.ResBuscaAnterioridade : dados.ResBuscaAnterioridade;
                    AtividadesGestao.ResContratacaoAuditoria = dados.ResContratacaoAuditoria == null ? AtividadesGestao.ResContratacaoAuditoria : dados.ResContratacaoAuditoria;
                    AtividadesGestao.ResApoioCitenel = dados.ResApoioCitenel == null ? AtividadesGestao.ResApoioCitenel : dados.ResApoioCitenel;

                    _context.SaveChanges();
                }
            }

            return resultado;
        }

        public Resultado Excluir( int id ) {
            Resultado resultado = new Resultado();
            resultado.Acao = "Exclusão de Atividades Gestão";

            AtividadesGestao AtividadesGestao = _context.AtividadesGestao.FirstOrDefault(p => p.Id == id);
            if(AtividadesGestao == null) {
                resultado.Inconsistencias.Add("Atividade Gestão não encontrada");
            }
            else {
                _context.AtividadesGestao.Remove(AtividadesGestao);
                _context.SaveChanges();
            }

            return resultado;
        }

        private Resultado DadosValidos( AtividadesGestao dados ) {
            var resultado = new Resultado();
            if(dados == null) {
                resultado.Inconsistencias.Add("Preencha os Dados das Atividades");
            }

            return resultado;
        }
    }
}