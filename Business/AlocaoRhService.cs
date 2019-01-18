using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using APIGestor.Data;
using APIGestor.Models;
using Microsoft.AspNetCore.Identity;
using APIGestor.Security;

namespace APIGestor.Business
{
    public class AlocacaoRhService
    {
        private GestorDbContext _context;

        public AlocacaoRhService(GestorDbContext context)
        {
            _context = context;
        }
        public IEnumerable<AlocacaoRh> ListarTodos(int projetoId)
        {
            var AlocacaoRh = _context.AlocacoesRh
                .Where(p => p.ProjetoId == projetoId)
                .ToList();
            return AlocacaoRh;
        }
        public Resultado Incluir(AlocacaoRh dados)
        {
            Resultado resultado = DadosValidos(dados);
            resultado.Acao = "Inclusão de AlocacaoRh";
            if (dados.ProjetoId <= 0)
            {
                resultado.Inconsistencias.Add("Preencha o ProjetoId");
            }
            else
            {
                Projeto Projeto = _context.Projetos.Where(
                        p => p.Id == dados.ProjetoId).FirstOrDefault();

                if (Projeto == null)
                {
                    resultado.Inconsistencias.Add("Projeto não localizado");
                }
            }

            if (resultado.Inconsistencias.Count == 0)
            {
                var data = new AlocacaoRh
                {
                    ProjetoId = dados.ProjetoId,
                    EtapaId = dados.EtapaId,
                    EmpresaId = dados.EmpresaId,
                    RecursoHumanoId = dados.RecursoHumanoId,
                    Justificativa = dados.Justificativa,
                    ValorHora = dados.ValorHora,
                    HrsMes1 = dados.HrsMes1,
                    HrsMes2 = dados.HrsMes2,
                    HrsMes3 = dados.HrsMes3,
                    HrsMes4 = dados.HrsMes5,
                    HrsMes5 = dados.HrsMes5,
                    HrsMes6 = dados.HrsMes6
                };
                _context.AlocacoesRh.Add(data);
                _context.SaveChanges();
            }
            return resultado;
        }
        public Resultado Atualizar(AlocacaoRh dados)
        {
            Resultado resultado = DadosValidos(dados);
            resultado.Acao = "Atualização de AlocacaoRh";

            if (resultado.Inconsistencias.Count == 0)
            {
                AlocacaoRh AlocacaoRh = _context.AlocacoesRh.Where(
                    p => p.Id == dados.Id).FirstOrDefault();

                if (AlocacaoRh == null)
                {
                    resultado.Inconsistencias.Add(
                        "AlocacaoRh não encontrado");
                }
                else
                {

                    AlocacaoRh.EtapaId = dados.EtapaId;
                    AlocacaoRh.EmpresaId = dados.EmpresaId;
                    AlocacaoRh.RecursoHumanoId = dados.RecursoHumanoId;
                    AlocacaoRh.Justificativa = dados.Justificativa;
                    AlocacaoRh.ValorHora = dados.ValorHora;
                    AlocacaoRh.HrsMes1 = dados.HrsMes1;
                    AlocacaoRh.HrsMes2 = dados.HrsMes2;
                    AlocacaoRh.HrsMes3 = dados.HrsMes3;
                    AlocacaoRh.HrsMes4 = dados.HrsMes5;
                    AlocacaoRh.HrsMes5 = dados.HrsMes5;
                    AlocacaoRh.HrsMes6 = dados.HrsMes6;
                    _context.SaveChanges();
                }
            }

            return resultado;
        }
        private Resultado DadosValidos(AlocacaoRh dados)
        {
            var resultado = new Resultado();
            if (dados == null)
            {
                resultado.Inconsistencias.Add("Preencha os Dados do AlocacaoRh");
            }
            else
            {
                if (dados.ProjetoId == null && dados.Id > 0)
                {
                    AlocacaoRh AlocacaoRh = _context.AlocacoesRh.Where(
                    p => p.Id == dados.Id).FirstOrDefault();
                    dados.ProjetoId = AlocacaoRh.ProjetoId;
                }
                if (dados.RecursoHumanoId == null)
                {
                    resultado.Inconsistencias.Add("Preencha o RecursoHumanoId");
                }
                else
                {
                    RecursoHumano RecursoHumano = _context.RecursoHumanos
                                        .Where(p => p.ProjetoId == dados.ProjetoId)
                                        .Where(p => p.Id == dados.RecursoHumanoId).FirstOrDefault();
                    if (RecursoHumano == null)
                    {
                        resultado.Inconsistencias.Add("RecursoHumanoId não cadastrada ou não associada ao projeto.");
                    }
                }
                if (dados.EtapaId == null)
                {
                    resultado.Inconsistencias.Add("Preencha o Nome do RecursoHumanoId");
                }
                else
                {
                    Etapa Etapa = _context.Etapas
                                        .Where(p => p.ProjetoId == dados.ProjetoId)
                                        .Where(p => p.Id == dados.EtapaId).FirstOrDefault();
                    if (Etapa == null)
                    {
                        resultado.Inconsistencias.Add("EtapaId não cadastrada ou não associada ao projeto.");
                    }
                }
                if (dados.EmpresaId == null)
                {
                    resultado.Inconsistencias.Add("Preencha o Nome do RecursoHumanoId");
                }
                else
                {
                    Empresa Empresa = _context.Empresas
                                        .Where(p => p.ProjetoId == dados.ProjetoId)
                                        .Where(p => p.Id == dados.EmpresaId).FirstOrDefault();
                    if (Empresa == null)
                    {
                        resultado.Inconsistencias.Add("EmpresaId não cadastrada ou não associada ao projeto.");
                    }
                }
                if (String.IsNullOrEmpty(dados.ValorHora.ToString()))
                {
                    resultado.Inconsistencias.Add("Preencha o Valor Hora do Recurso Humano alocado");
                }
                if (String.IsNullOrEmpty(dados.Justificativa))
                {
                    resultado.Inconsistencias.Add("Preencha a Justificativa da alocação");
                }
            }
            return resultado;
        }
        public Resultado Excluir(int id)
        {
            Resultado resultado = new Resultado();
            resultado.Acao = "Exclusão de AlocacaoRh";

            AlocacaoRh AlocacaoRh = _context.AlocacoesRh.First(t => t.Id == id);
            if (AlocacaoRh == null)
            {
                resultado.Inconsistencias.Add("AlocacaoRh não encontrada");
            }
            else
            {
                _context.AlocacoesRh.Remove(AlocacaoRh);
                _context.SaveChanges();
            }

            return resultado;
        }
    }
}