using System;
using System.Collections.Generic;
using System.Linq;
using APIGestor.Data;
using APIGestor.Models.Projetos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace APIGestor.Services.Projetos {
    public class RegistroFinanceiroService : BaseGestorService {

        public RegistroFinanceiroService( GestorDbContext context, IAuthorizationService authorization, LogService logService ) : base(context, authorization, logService) {

        }

        public RegistroFinanceiro Obter( int id ) {
            if(id > 0) {
                return _context.RegistrosFinanceiros
                .Include("Uploads")
                .Include("CategoriaContabilGestao")
                .Include("Atividade")
                .Include("EmpresaFinanciadora.CatalogEmpresa")
                .Include("EmpresaRecebedora..CatalogEmpresa")
                .Include("RecursoHumano")
                .Include("RecursoMaterial")
                .Include(p => p.ObsInternas)
                .FirstOrDefault(p => p.Id == id);
            }
            else
                return null;
        }

        public IEnumerable<RegistroFinanceiro> ListarTodos( int projetoId, StatusRegistro status ) {
            var RegistroFinanceiro = _context.RegistrosFinanceiros
                .Include("Uploads")
                .Include("CategoriaContabilGestao")
                .Include("Atividade")
                .Include("EmpresaFinanciadora")
                .Include("RecursoHumano")
                .Include("RecursoMaterial")
                .Include("ObsInternas.User")
                .Where(p => p.ProjetoId == projetoId)
                .Where(p => p.Status == (StatusRegistro)status)
                .ToList();
            return RegistroFinanceiro;
        }

        public Resultado Incluir( RegistroFinanceiro dados, string userId ) {
            Resultado resultado = DadosValidos(dados);
            resultado.Acao = "Inclusão de RegistroFinanceiro";
            if(dados.ProjetoId <= 0) {
                resultado.Inconsistencias.Add("Preencha o ProjetoId");
            }
            else {
                Projeto Projeto = _context.Projetos.Where(
                        p => p.Id == dados.ProjetoId).FirstOrDefault();

                if(Projeto == null) {
                    resultado.Inconsistencias.Add("Projeto não localizado");
                }
            }

            if(resultado.Inconsistencias.Count == 0) {
                if(dados.ObsInternas.Count() > 0)
                    foreach(RegistroObs obs in dados.ObsInternas) {
                        obs.Created = DateTime.Now;
                        obs.UserId = userId;
                    }
                dados.Status = (StatusRegistro)1;
                _context.RegistrosFinanceiros.Add(dados);
                _context.SaveChanges();
                resultado.Id = dados.Id.ToString();
            }
            return resultado;
        }

        public Resultado Atualizar( RegistroFinanceiro dados, string userId ) {
            Resultado resultado = DadosValidos(dados);
            resultado.Acao = "Atualização de RegistroFinanceiro";

            if(resultado.Inconsistencias.Count == 0) {
                RegistroFinanceiro registro = this.Obter(dados.Id);

                if(registro == null) {
                    resultado.Inconsistencias.Add(
                        "RegistroFinanceiro não encontrado");
                }
                else {
                    if(dados.ObsInternas != null && dados.ObsInternas.Count() > 0) {
                        foreach(RegistroObs obs in dados.ObsInternas) {
                            obs.Created = DateTime.Now;
                            obs.UserId = userId;
                        }
                        registro.ObsInternas.AddRange(dados.ObsInternas);
                    }
                    registro.ProjetoId = dados.ProjetoId == null ? registro.ProjetoId : dados.ProjetoId;
                    registro.Tipo = (dados.Tipo != null && Enum.IsDefined(typeof(TipoRegistro), dados.Tipo)) ? dados.Tipo : registro.Tipo;
                    registro.Status = (dados.Status != null && Enum.IsDefined(typeof(StatusRegistro), dados.Status)) ? dados.Status : registro.Status;
                    registro.RecursoHumanoId = dados.RecursoHumanoId == null ? registro.RecursoHumanoId : dados.RecursoHumanoId;
                    registro.Mes = dados.Mes == null ? registro.Mes : dados.Mes;
                    registro.QtdHrs = dados.QtdHrs == null ? registro.QtdHrs : dados.QtdHrs;
                    registro.EmpresaFinanciadoraId = dados.EmpresaFinanciadoraId == null ? registro.EmpresaFinanciadoraId : dados.EmpresaFinanciadoraId;
                    registro.TipoDocumento = (dados.TipoDocumento != null && Enum.IsDefined(typeof(TipoDocumento), dados.TipoDocumento)) ? dados.TipoDocumento : registro.TipoDocumento;
                    registro.NumeroDocumento = dados.NumeroDocumento == null ? registro.NumeroDocumento : dados.NumeroDocumento;
                    registro.DataDocumento = dados.DataDocumento == null ? registro.DataDocumento : dados.DataDocumento;
                    registro.AtividadeRealizada = dados.AtividadeRealizada == null ? registro.AtividadeRealizada : dados.AtividadeRealizada;
                    // registro.ObsInternas = dados.ObsInternas == null ? registro.ObsInternas : dados.ObsInternas;
                    registro.NomeItem = dados.NomeItem == null ? registro.NomeItem : dados.NomeItem;
                    registro.RecursoMaterialId = dados.RecursoMaterialId == null ? registro.RecursoMaterialId : dados.RecursoMaterialId;
                    registro.EmpresaRecebedoraId = dados.EmpresaRecebedoraId == null ? registro.EmpresaRecebedoraId : dados.EmpresaRecebedoraId;
                    registro.Beneficiado = dados.Beneficiado == null ? registro.Beneficiado : dados.Beneficiado;
                    registro.CnpjBeneficiado = dados.CnpjBeneficiado == null ? registro.CnpjBeneficiado : dados.CnpjBeneficiado;
                    registro.CategoriaContabil = (dados.CategoriaContabil != null && Enum.IsDefined(typeof(CategoriaContabil), dados.CategoriaContabil)) ? dados.CategoriaContabil : registro.CategoriaContabil;
                    registro.EquiparLabExistente = dados.EquiparLabExistente.HasValue ? dados.EquiparLabExistente : registro.EquiparLabExistente;
                    registro.EquiparLabNovo = dados.EquiparLabNovo.HasValue ? dados.EquiparLabNovo : registro.EquiparLabNovo;
                    registro.ItemNacional = dados.ItemNacional.HasValue ? dados.ItemNacional : dados.ItemNacional;
                    registro.QtdItens = dados.QtdItens == null ? registro.QtdItens : dados.QtdItens;
                    registro.ValorUnitario = dados.ValorUnitario == null ? registro.ValorUnitario : dados.ValorUnitario;
                    registro.EspecificacaoTecnica = dados.EspecificacaoTecnica == null ? registro.EspecificacaoTecnica : dados.EspecificacaoTecnica;
                    registro.FuncaoRecurso = dados.FuncaoRecurso == null ? registro.FuncaoRecurso : dados.FuncaoRecurso;
                    registro.CatalogCategoriaContabilGestaoId = dados.CatalogCategoriaContabilGestaoId == null ? registro.CatalogCategoriaContabilGestaoId : dados.CatalogCategoriaContabilGestaoId;
                    registro.CatalogAtividadeId = dados.CatalogAtividadeId == null ? registro.CatalogAtividadeId : dados.CatalogAtividadeId;

                    _context.SaveChanges();
                }
            }

            return resultado;
        }
        private Resultado DadosValidos( RegistroFinanceiro dados ) {
            var resultado = new Resultado();
            if(dados == null) {
                resultado.Inconsistencias.Add("Preencha os Dados do RegistroFinanceiro");
            }
            return resultado;
        }
        public Resultado Excluir( int id ) {
            Resultado resultado = new Resultado();
            resultado.Acao = "Exclusão de RegistroFinanceiro";

            RegistroFinanceiro registro = _context.RegistrosFinanceiros
                .Where(p => p.Id == id).FirstOrDefault();
            if(registro == null) {
                resultado.Inconsistencias.Add("RegistroFinanceiro não encontrada");
            }
            else {
                _context.Uploads.RemoveRange(_context.Uploads.Where(t => t.RegistroFinanceiroId == id));
                _context.RegistroObs.RemoveRange(_context.RegistroObs.Where(t => t.RegistroFinanceiroId == id));
                _context.RegistrosFinanceiros.Remove(registro);
                _context.SaveChanges();
            }

            return resultado;
        }
    }
}