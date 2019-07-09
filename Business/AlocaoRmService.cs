using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using APIGestor.Data;
using APIGestor.Models;
using Microsoft.AspNetCore.Identity;
using APIGestor.Security;
using Microsoft.AspNetCore.Authorization;

namespace APIGestor.Business {
    public class AlocacaoRmService : BaseAuthorizationService {

        public AlocacaoRmService( GestorDbContext context, IAuthorizationService authorization ) : base(context, authorization) { }
        public IEnumerable<AlocacaoRm> ListarTodos( int projetoId ) {
            var AlocacaoRm = _context.AlocacoesRm
                .Include("RecursoMaterial.CategoriaContabilGestao")
                .Include("RecursoMaterial.Atividade")
                .Include("EmpresaFinanciadora.CatalogEmpresa")
                .Include("EmpresaRecebedora.CatalogEmpresa")
                .Where(p => p.ProjetoId == projetoId)
                .ToList();
            return AlocacaoRm;
        }
        public Resultado Incluir( AlocacaoRm dados ) {
            Resultado resultado = DadosValidos(dados);
            resultado.Acao = "Inclusão de AlocacaoRm";
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
                _context.AlocacoesRm.Add(dados);
                _context.SaveChanges();
            }
            return resultado;
        }
        public Resultado Atualizar( AlocacaoRm dados ) {
            Resultado resultado = DadosValidos(dados);
            resultado.Acao = "Atualização de AlocacaoRm";

            if(resultado.Inconsistencias.Count == 0) {
                AlocacaoRm AlocacaoRm = _context.AlocacoesRm.Where(
                    p => p.Id == dados.Id).FirstOrDefault();

                if(AlocacaoRm == null) {
                    resultado.Inconsistencias.Add(
                        "AlocacaoRm não encontrado");
                }
                else {
                    AlocacaoRm.EtapaId = dados.EtapaId == null ? AlocacaoRm.EtapaId : dados.EtapaId;
                    AlocacaoRm.EmpresaFinanciadoraId = dados.EmpresaFinanciadoraId == null ? AlocacaoRm.EmpresaFinanciadoraId : dados.EmpresaFinanciadoraId;
                    AlocacaoRm.EmpresaRecebedoraId = dados.EmpresaRecebedoraId == null ? AlocacaoRm.EmpresaRecebedoraId : dados.EmpresaRecebedoraId;
                    AlocacaoRm.RecursoMaterialId = dados.RecursoMaterialId == null ? AlocacaoRm.RecursoMaterialId : dados.RecursoMaterialId;
                    AlocacaoRm.Qtd = dados.Qtd;
                    AlocacaoRm.Justificativa = dados.Justificativa;
                    _context.SaveChanges();
                }
            }

            return resultado;
        }
        private Resultado DadosValidos( AlocacaoRm dados ) {
            var resultado = new Resultado();
            if(dados == null) {
                resultado.Inconsistencias.Add("Preencha os Dados do AlocacaoRm");
            }
            else {
                if(dados.ProjetoId == null && dados.Id > 0) {
                    AlocacaoRm AlocacaoRm = _context.AlocacoesRm.Where(
                    p => p.Id == dados.Id).FirstOrDefault();
                    dados.ProjetoId = AlocacaoRm.ProjetoId;
                }
                if(dados.RecursoMaterialId == null) {
                    resultado.Inconsistencias.Add("Preencha o RecursoMaterialId");
                }
                else {
                    RecursoMaterial RecursoMaterial = _context.RecursoMateriais
                                        .Where(p => p.ProjetoId == dados.ProjetoId)
                                        .Where(p => p.Id == dados.RecursoMaterialId).FirstOrDefault();
                    if(RecursoMaterial == null) {
                        resultado.Inconsistencias.Add("RecursoMaterialId não cadastrada ou não associada ao projeto.");
                    }
                }
                // if (dados.EtapaId == null)
                // {
                //     resultado.Inconsistencias.Add("Preencha a Etapa do RecursoMaterialId");
                // }
                // else
                // {
                //     Etapa Etapa = _context.Etapas
                //                         .Where(p => p.ProjetoId == dados.ProjetoId)
                //                         .Where(p => p.Id == dados.EtapaId).FirstOrDefault();
                //     if (Etapa == null)
                //     {
                //         resultado.Inconsistencias.Add("EtapaId não cadastrada ou não associada ao projeto.");
                //     }
                // }
                if(dados.EmpresaFinanciadoraId == null) {
                    resultado.Inconsistencias.Add("Preencha o EmpresaFinanciadoraId");
                }
                else {
                    Empresa Empresa = _context.Empresas
                                        .Where(p => p.ProjetoId == dados.ProjetoId)
                                        .Where(p => p.Id == dados.EmpresaFinanciadoraId).FirstOrDefault();
                    if(Empresa == null) {
                        resultado.Inconsistencias.Add("EmpresaFinanciadoraId não cadastrada ou não associada ao projeto.");
                    }
                }
                if(dados.EmpresaRecebedoraId != null) {
                    Empresa Empresa = _context.Empresas
                                            .Where(p => p.ProjetoId == dados.ProjetoId)
                                            .Where(p => p.Id == dados.EmpresaRecebedoraId).FirstOrDefault();
                    if(Empresa == null) {
                        resultado.Inconsistencias.Add("EmpresaRecebedoraId não cadastrada ou não associada ao projeto.");
                    }
                }
                if(dados.Qtd <= 0) {
                    resultado.Inconsistencias.Add("Preencha o Quantidade do Recurso Material alocado");
                }
                if(String.IsNullOrEmpty(dados.Justificativa)) {
                    resultado.Inconsistencias.Add("Preencha a Justificativa da alocação");
                }
            }
            return resultado;
        }
        public Resultado Excluir( int id ) {
            Resultado resultado = new Resultado();
            resultado.Acao = "Exclusão de AlocacaoRm";

            AlocacaoRm AlocacaoRm = _context.AlocacoesRm.First(t => t.Id == id);
            if(AlocacaoRm == null) {
                resultado.Inconsistencias.Add("AlocacaoRm não encontrada");
            }
            else {
                _context.AlocacoesRm.Remove(AlocacaoRm);
                _context.SaveChanges();
            }

            return resultado;
        }
    }
}