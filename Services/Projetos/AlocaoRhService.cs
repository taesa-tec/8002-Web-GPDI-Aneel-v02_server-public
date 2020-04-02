using System;
using System.Collections.Generic;
using System.Linq;
using APIGestor.Data;
using APIGestor.Models.Projetos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace APIGestor.Services.Projetos {
    public class AlocacaoRhService : BaseGestorService {


        public AlocacaoRhService( GestorDbContext context, IAuthorizationService authorization, LogService logService ) : base(context, authorization, logService) {

        }
        public IEnumerable<AlocacaoRh> ListarTodos( int projetoId ) {
            var AlocacaoRh = _context.AlocacoesRh
                .Include("RecursoHumano")
                .Include("Empresa.CatalogEmpresa")
                .Include("Etapa")
                .Where(p => p.ProjetoId == projetoId)
                .ToList();
            return AlocacaoRh;
        }

        public AlocacaoRh Obter( int id ) {
            return _context.AlocacoesRh
                .Include("RecursoHumano")
                .Include("Empresa.CatalogEmpresa")
                .Include("Etapa")
                .FirstOrDefault(a => a.Id == id);
        }

        public Resultado Incluir( AlocacaoRh dados ) {
            Resultado resultado = DadosValidos(dados);
            resultado.Acao = "Inclusão de AlocacaoRh";
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
                _context.AlocacoesRh.Add(dados);
                _context.SaveChanges();
                resultado.Id = dados.Id.ToString();
            }
            return resultado;
        }
        public Resultado Atualizar( AlocacaoRh dados ) {
            Resultado resultado = DadosValidos(dados);
            resultado.Acao = "Atualização de AlocacaoRh";

            if(resultado.Inconsistencias.Count == 0) {
                AlocacaoRh AlocacaoRh = _context.AlocacoesRh.Where(
                    p => p.Id == dados.Id).FirstOrDefault();

                if(AlocacaoRh == null) {
                    resultado.Inconsistencias.Add(
                        "AlocacaoRh não encontrado");
                }
                else {

                    AlocacaoRh.EtapaId = dados.EtapaId == null ? AlocacaoRh.EtapaId : dados.EtapaId;
                    AlocacaoRh.EmpresaId = dados.EmpresaId == null ? AlocacaoRh.EmpresaId : dados.EmpresaId;
                    AlocacaoRh.RecursoHumanoId = dados.RecursoHumanoId == null ? AlocacaoRh.RecursoHumanoId : dados.RecursoHumanoId;
                    AlocacaoRh.Justificativa = dados.Justificativa == null ? AlocacaoRh.Justificativa : dados.Justificativa;
                    AlocacaoRh.HrsMes1 = dados.HrsMes1;
                    AlocacaoRh.HrsMes2 = dados.HrsMes2;
                    AlocacaoRh.HrsMes3 = dados.HrsMes3;
                    AlocacaoRh.HrsMes4 = dados.HrsMes4;
                    AlocacaoRh.HrsMes5 = dados.HrsMes5;
                    AlocacaoRh.HrsMes6 = dados.HrsMes6;
                    AlocacaoRh.HrsMes7 = dados.HrsMes7 == null ? AlocacaoRh.HrsMes7 : dados.HrsMes7;
                    AlocacaoRh.HrsMes8 = dados.HrsMes8 == null ? AlocacaoRh.HrsMes8 : dados.HrsMes8;
                    AlocacaoRh.HrsMes9 = dados.HrsMes9 == null ? AlocacaoRh.HrsMes9 : dados.HrsMes9;
                    AlocacaoRh.HrsMes10 = dados.HrsMes10 == null ? AlocacaoRh.HrsMes10 : dados.HrsMes10;
                    AlocacaoRh.HrsMes11 = dados.HrsMes11 == null ? AlocacaoRh.HrsMes11 : dados.HrsMes11;
                    AlocacaoRh.HrsMes12 = dados.HrsMes12 == null ? AlocacaoRh.HrsMes12 : dados.HrsMes12;
                    AlocacaoRh.HrsMes13 = dados.HrsMes13 == null ? AlocacaoRh.HrsMes13 : dados.HrsMes13;
                    AlocacaoRh.HrsMes14 = dados.HrsMes14 == null ? AlocacaoRh.HrsMes14 : dados.HrsMes14;
                    AlocacaoRh.HrsMes15 = dados.HrsMes15 == null ? AlocacaoRh.HrsMes15 : dados.HrsMes15;
                    AlocacaoRh.HrsMes16 = dados.HrsMes16 == null ? AlocacaoRh.HrsMes16 : dados.HrsMes16;
                    AlocacaoRh.HrsMes17 = dados.HrsMes17 == null ? AlocacaoRh.HrsMes17 : dados.HrsMes17;
                    AlocacaoRh.HrsMes18 = dados.HrsMes18 == null ? AlocacaoRh.HrsMes18 : dados.HrsMes18;
                    AlocacaoRh.HrsMes19 = dados.HrsMes19 == null ? AlocacaoRh.HrsMes19 : dados.HrsMes19;
                    AlocacaoRh.HrsMes20 = dados.HrsMes20 == null ? AlocacaoRh.HrsMes20 : dados.HrsMes20;
                    AlocacaoRh.HrsMes21 = dados.HrsMes21 == null ? AlocacaoRh.HrsMes21 : dados.HrsMes21;
                    AlocacaoRh.HrsMes22 = dados.HrsMes22 == null ? AlocacaoRh.HrsMes22 : dados.HrsMes22;
                    AlocacaoRh.HrsMes23 = dados.HrsMes23 == null ? AlocacaoRh.HrsMes23 : dados.HrsMes23;
                    AlocacaoRh.HrsMes24 = dados.HrsMes24 == null ? AlocacaoRh.HrsMes24 : dados.HrsMes24;
                    _context.SaveChanges();
                }
            }

            return resultado;
        }
        private Resultado DadosValidos( AlocacaoRh dados ) {
            var resultado = new Resultado();
            if(dados == null) {
                resultado.Inconsistencias.Add("Preencha os Dados do AlocacaoRh");
            }
            else {
                if(dados.ProjetoId == null && dados.Id > 0) {
                    dados.ProjetoId = _context.AlocacoesRh.Where(
                    p => p.Id == dados.Id).FirstOrDefault().ProjetoId;
                }
                if(dados.RecursoHumanoId == null) {
                    resultado.Inconsistencias.Add("Preencha o RecursoHumanoId");
                }
                else {
                    RecursoHumano RecursoHumano = _context.RecursoHumanos
                                        .Where(p => p.ProjetoId == dados.ProjetoId)
                                        .Where(p => p.Id == dados.RecursoHumanoId).FirstOrDefault();
                    if(RecursoHumano == null) {
                        resultado.Inconsistencias.Add("RecursoHumanoId não cadastrada ou não associada ao projeto.");
                    }
                }
                // if (dados.EtapaId == null)
                // {
                //     resultado.Inconsistencias.Add("Preencha o Nome do RecursoHumanoId");
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
                if(dados.EmpresaId == null) {
                    resultado.Inconsistencias.Add("Preencha a EmpresaId");
                }
                else {
                    Empresa Empresa = _context.Empresas
                                        .Where(p => p.ProjetoId == dados.ProjetoId)
                                        .Where(p => p.Id == dados.EmpresaId).FirstOrDefault();
                    if(Empresa == null) {
                        resultado.Inconsistencias.Add("EmpresaId não cadastrada ou não associada ao projeto.");
                    }
                }
                if(String.IsNullOrEmpty(dados.Justificativa)) {
                    resultado.Inconsistencias.Add("Preencha a Justificativa da alocação");
                }
            }
            return resultado;
        }
        public Resultado Excluir( int id ) {
            Resultado resultado = new Resultado();
            resultado.Acao = "Exclusão de AlocacaoRh";

            AlocacaoRh AlocacaoRh = _context.AlocacoesRh.First(t => t.Id == id);
            if(AlocacaoRh == null) {
                resultado.Inconsistencias.Add("AlocacaoRh não encontrada");
            }
            else {
                _context.AlocacoesRh.Remove(AlocacaoRh);
                _context.SaveChanges();
            }

            return resultado;
        }
    }
}