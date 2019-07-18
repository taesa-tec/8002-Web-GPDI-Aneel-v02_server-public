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
    public class RecursoHumanoService : BaseGestorService {

        public RecursoHumanoService( GestorDbContext context, IAuthorizationService authorization, LogService logService ) : base(context, authorization, logService) {
        }
        public IEnumerable<RecursoHumano> ListarTodos( int projetoId ) {
            var RecursoHumano = _context.RecursoHumanos
                .Include("Empresa.CatalogEmpresa")
                .Where(p => p.ProjetoId == projetoId)
                .ToList();
            return RecursoHumano;
        }
        public Resultado Incluir( RecursoHumano dados ) {
            Resultado resultado = DadosValidos(dados);
            resultado.Acao = "Inclusão de RecursoHumano";
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
                _context.RecursoHumanos.Add(dados);
                _context.SaveChanges();
                resultado.Id = dados.Id.ToString();
            }
            return resultado;
        }

        public RecursoHumano Obter( int id ) {
            return _context.RecursoHumanos.Include("Empresa.CatalogEmpresa").FirstOrDefault(r => r.Id == id);
        }

        public Resultado Atualizar( RecursoHumano dados ) {
            Resultado resultado = DadosValidos(dados);
            resultado.Acao = "Atualização de RecursoHumano";

            if(resultado.Inconsistencias.Count == 0) {
                RecursoHumano RecursoHumano = _context.RecursoHumanos.Where(
                    p => p.Id == dados.Id).FirstOrDefault();

                if(RecursoHumano == null) {
                    resultado.Inconsistencias.Add(
                        "RecursoHumano não encontrado");
                }
                else {
                    RecursoHumano.EmpresaId = dados.EmpresaId <= 0 ? RecursoHumano.EmpresaId : dados.EmpresaId;
                    RecursoHumano.ValorHora = dados.ValorHora <= 0 ? RecursoHumano.ValorHora : dados.ValorHora;
                    RecursoHumano.NomeCompleto = dados.NomeCompleto == null ? RecursoHumano.NomeCompleto : dados.NomeCompleto;
                    RecursoHumano.Titulacao = Enum.IsDefined(typeof(RecursoHumanoTitulacao), dados.Titulacao) ? dados.Titulacao : RecursoHumano.Titulacao;
                    RecursoHumano.Funcao = Enum.IsDefined(typeof(RecursoHumanoFuncao), dados.Funcao) ? dados.Funcao : RecursoHumano.Funcao;
                    RecursoHumano.Nacionalidade = Enum.IsDefined(typeof(RecursoHumanoNacionalidade), dados.Nacionalidade) ? dados.Nacionalidade : RecursoHumano.Nacionalidade;
                    RecursoHumano.CPF = dados.CPF == null ? RecursoHumano.CPF : dados.CPF;
                    RecursoHumano.Passaporte = dados.Passaporte == null ? RecursoHumano.Passaporte : dados.Passaporte;
                    RecursoHumano.UrlCurriculo = dados.UrlCurriculo == null ? RecursoHumano.UrlCurriculo : dados.UrlCurriculo;
                    RecursoHumano.GerenteProjeto = (!dados.GerenteProjeto.HasValue) ? RecursoHumano.GerenteProjeto : dados.GerenteProjeto;
                    _context.SaveChanges();
                }
            }

            return resultado;
        }
        private Resultado DadosValidos( RecursoHumano dados ) {
            var resultado = new Resultado();
            if(dados == null) {
                resultado.Inconsistencias.Add("Preencha os Dados do RecursoHumano");
            }
            else {
                if(dados.ProjetoId > 0) {
                    if(String.IsNullOrEmpty(dados.NomeCompleto)) {
                        resultado.Inconsistencias.Add("Preencha o Nome Completo do RecursoHumano");
                    }
                    if(String.IsNullOrEmpty(dados.Titulacao.ToString())) {
                        resultado.Inconsistencias.Add("Preencha a Titulação do RecursoHumano");
                    }
                    if(String.IsNullOrEmpty(dados.Funcao.ToString())) {
                        resultado.Inconsistencias.Add("Preencha o Função do RecursoHumano");
                    }
                    if(String.IsNullOrEmpty(dados.Nacionalidade.ToString())) {
                        resultado.Inconsistencias.Add("Preencha a Nacionalidade do RecursoHumano");
                    }
                    if(dados.Nacionalidade.ToString() == "Brasileiro") {
                        if(String.IsNullOrEmpty(dados.CPF)) {
                            resultado.Inconsistencias.Add("Preencha o CPF do RecursoHumano");
                        }
                        else {
                            if(dados.Id <= 0) {
                                RecursoHumano RecursoHumano = _context.RecursoHumanos
                                        .Where(p => p.ProjetoId == dados.ProjetoId)
                                        .Where(p => p.CPF == dados.CPF).FirstOrDefault();
                                if(RecursoHumano != null) {
                                    resultado.Inconsistencias.Add("CPF já cadastrado como recurso humano para esse projeto. Remova ou Atualize.");
                                }
                            }
                        }
                    }
                    else {
                        if(String.IsNullOrEmpty(dados.Passaporte)) {
                            resultado.Inconsistencias.Add("Preencha o Passaporte do RecursoHumano");
                        }
                        else {
                            if(dados.ProjetoId > 0) {
                                RecursoHumano RecursoHumano = _context.RecursoHumanos
                                        .Where(p => p.ProjetoId == dados.ProjetoId)
                                        .Where(p => p.Passaporte == dados.Passaporte).FirstOrDefault();
                                if(RecursoHumano != null) {
                                    resultado.Inconsistencias.Add("Passporte já cadastrado como recurso humano para esse projeto. Remova ou Atualize.");
                                }
                            }
                        }
                    }
                }
                if(dados.EmpresaId <= 0) {
                    resultado.Inconsistencias.Add("Preencha o EmpresaId");
                }
                else {
                    if(dados.ProjetoId == null && dados.Id > 0) {
                        RecursoHumano RecursoHumano = _context.RecursoHumanos.Where(
                        p => p.Id == dados.Id).FirstOrDefault();

                        Empresa Empresa = _context.Empresas
                                            .Where(p => p.ProjetoId == RecursoHumano.ProjetoId)
                                            .Where(p => p.Id == dados.EmpresaId).FirstOrDefault();
                        if(Empresa == null) {
                            resultado.Inconsistencias.Add("Empresa não cadastrada ou não associada ao projeto.");
                        }
                    }
                }
            }
            return resultado;
        }
        public Resultado Excluir( int id ) {
            Resultado resultado = new Resultado();
            resultado.Acao = "Exclusão de RecursoHumano";

            RecursoHumano RecursoHumano = _context.RecursoHumanos
                .First(t => t.Id == id);
            if(RecursoHumano == null) {
                resultado.Inconsistencias.Add("RecursoHumano não encontrada");
            }
            else {
                try {
                    _context.RecursoHumanos.Remove(RecursoHumano);
                    _context.SaveChanges();
                }
                catch(Exception) {
                    resultado.Inconsistencias.Add("Não é possível excluir. " +
                        "Pode haver outros cadastros relacionados no projeto");
                }

            }

            return resultado;
        }
    }
}