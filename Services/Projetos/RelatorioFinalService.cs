using System.Linq;
using APIGestor.Data;
using APIGestor.Models.Projetos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace APIGestor.Services.Projetos {
    public class RelatorioFinalService : BaseGestorService {

        public RelatorioFinalService( GestorDbContext context, IAuthorizationService authorization, LogService logService ) : base(context, authorization, logService) {
        }

        public RelatorioFinal Obter( int projetoId ) {
            if(projetoId > 0) {
                return _context.RelatorioFinal
                    .Include("Uploads.User")
                    .Where(p => p.ProjetoId == projetoId).FirstOrDefault();
            }
            else
                return null;
        }

        public Resultado Incluir( RelatorioFinal dados ) {
            Resultado resultado = DadosValidos(dados);
            resultado.Acao = "Inclusão de RelatorioFinal";

            RelatorioFinal RelatorioFinal = _context.RelatorioFinal
                .Where(p => p.ProjetoId == dados.ProjetoId)
                .FirstOrDefault();

            if(RelatorioFinal != null) {
                resultado.Inconsistencias.Add("Já existe um RelatorioFinal final para o projeto. Remova-o ou atualize.");
            }

            if(resultado.Inconsistencias.Count == 0) {
                _context.RelatorioFinal.Add(dados);
                _context.SaveChanges();
                resultado.Id = dados.Id.ToString();
            }
            return resultado;
        }

        public Resultado Atualizar( RelatorioFinal dados ) {
            Resultado resultado = DadosValidos(dados);
            resultado.Acao = "Atualização de RelatorioFinal";

            if(resultado.Inconsistencias.Count == 0) {
                RelatorioFinal RelatorioFinal = _context.RelatorioFinal.Where(
                    p => p.Id == dados.Id).FirstOrDefault();

                if(RelatorioFinal == null) {
                    resultado.Inconsistencias.Add(
                        "RelatorioFinal não encontrado");
                }
                else {
                    RelatorioFinal.ProdutoAlcancado = !dados.ProdutoAlcancado.HasValue ? RelatorioFinal.ProdutoAlcancado : dados.ProdutoAlcancado;
                    RelatorioFinal.JustificativaProduto = (dados.JustificativaProduto == null) ? RelatorioFinal.JustificativaProduto : dados.JustificativaProduto;
                    RelatorioFinal.EspecificacaoProduto = (dados.EspecificacaoProduto == null) ? RelatorioFinal.EspecificacaoProduto : dados.EspecificacaoProduto;
                    RelatorioFinal.TecnicaPrevista = !dados.TecnicaPrevista.HasValue ? RelatorioFinal.TecnicaPrevista : dados.TecnicaPrevista;
                    RelatorioFinal.JustificativaTecnica = (dados.JustificativaTecnica == null) ? RelatorioFinal.JustificativaTecnica : dados.JustificativaTecnica;
                    RelatorioFinal.DescTecnica = (dados.DescTecnica == null) ? RelatorioFinal.DescTecnica : dados.DescTecnica;
                    RelatorioFinal.AplicabilidadePrevista = !dados.AplicabilidadePrevista.HasValue ? RelatorioFinal.AplicabilidadePrevista : dados.AplicabilidadePrevista;
                    RelatorioFinal.JustificativaAplicabilidade = (dados.JustificativaAplicabilidade == null) ? RelatorioFinal.JustificativaAplicabilidade : dados.JustificativaAplicabilidade;
                    RelatorioFinal.DescTestes = (dados.DescTestes == null) ? RelatorioFinal.DescTestes : dados.DescTestes;
                    RelatorioFinal.DescAbrangencia = (dados.DescAbrangencia == null) ? RelatorioFinal.DescAbrangencia : dados.DescAbrangencia;
                    RelatorioFinal.DescAmbito = (dados.DescAmbito == null) ? RelatorioFinal.DescAmbito : dados.DescAmbito;
                    RelatorioFinal.DescAtividades = (dados.DescAtividades == null) ? RelatorioFinal.DescAtividades : dados.DescAtividades;
                    _context.SaveChanges();
                }
            }

            return resultado;
        }

        public Resultado Excluir( int id ) {
            Resultado resultado = new Resultado();
            resultado.Acao = "Exclusão de RelatorioFinal";

            RelatorioFinal RelatorioFinal = _context.RelatorioFinal.FirstOrDefault(p => p.Id == id);
            if(RelatorioFinal == null) {
                resultado.Inconsistencias.Add("RelatorioFinal não encontrado");
            }
            else {
                _context.Uploads.RemoveRange(_context.Uploads.Where(t => t.RelatorioFinalId == id));
                _context.RelatorioFinal.Remove(RelatorioFinal);
                _context.SaveChanges();
            }

            return resultado;
        }

        private Resultado DadosValidos( RelatorioFinal dados ) {
            var resultado = new Resultado();
            if(dados == null) {
                resultado.Inconsistencias.Add(
                    "Preencha os Dados do RelatorioFinal");
            }

            return resultado;
        }
    }
}