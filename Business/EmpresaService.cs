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
    public class EmpresaService
    {
        private GestorDbContext _context;

        public EmpresaService(GestorDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Empresa> ListarTodos(int projetoId)
        {
            var Empresa = _context.Empresas
                .Where(p => p.ProjetoId == projetoId)
                .ToList();
            return Empresa;
        }
        public Resultado Incluir(Empresa dados)
        {
            Resultado resultado = DadosValidos(dados);
            resultado.Acao = "Inclusão de Empresa";
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
                var etapa = new Empresa
                {
                    ProjetoId = dados.ProjetoId,
                    Classificacao = dados.Classificacao,
                    CatalogEmpresaId = dados.CatalogEmpresaId,
                    Cnpj = dados.Cnpj,
                    CatalogEstadoId = dados.CatalogEstadoId,
                    RazaoSocial = dados.RazaoSocial
                };
                _context.Empresas.Add(etapa);
                _context.SaveChanges();
            }
            return resultado;
        }
        public Resultado Atualizar(Empresa dados)
        {
            Resultado resultado = DadosValidos(dados);
            resultado.Acao = "Atualização de Empresa";

            if (resultado.Inconsistencias.Count == 0)
            {
                Empresa Empresa = _context.Empresas.Where(
                    p => p.Id == dados.Id).FirstOrDefault();
                
                if (Empresa == null)
                {
                    resultado.Inconsistencias.Add(
                        "Empresa não encontrado");
                }
                else
                {
                    Empresa.Classificacao = dados.Classificacao;
                    Empresa.CatalogEmpresaId = dados.CatalogEmpresaId;
                    Empresa.Cnpj = dados.Cnpj;
                    Empresa.CatalogEstadoId = dados.CatalogEstadoId;
                    Empresa.RazaoSocial = dados.RazaoSocial;
                    _context.SaveChanges();
                }
            }

            return resultado;
        }
        private Resultado DadosValidos(Empresa dados)
        {
            var resultado = new Resultado();
            if (dados == null)
            {
                resultado.Inconsistencias.Add("Preencha os Dados do Empresa");
            }
            else
            {
                if (String.IsNullOrEmpty(dados.Classificacao.ToString()))
                {
                    resultado.Inconsistencias.Add("Preencha a Classificação da Empresa");
                }
                else
                {
                    if (dados.Classificacao.ToString() == "Proponente"){
                        resultado.Inconsistencias.Add("Operação não permitida");
                    }
                    if (dados.Classificacao.ToString() == "Energia")
                    {
                        if (dados.CatalogEmpresaId > 0)
                        {
                            CatalogEmpresa CatalogEmpresa = _context.CatalogEmpresas.Where(
                                p => p.Id == dados.CatalogEmpresaId).FirstOrDefault();

                            if (CatalogEmpresa == null)
                            {
                                resultado.Inconsistencias.Add("CatalogEmpresa não localizado");
                            }
                            if ((!String.IsNullOrEmpty(dados.Cnpj))
                                ||(!String.IsNullOrEmpty(dados.RazaoSocial))
                                ||(dados.CatalogEstadoId>0))
                            {
                                resultado.Inconsistencias.Add("Não permitido adicionar o CNPJ, Razão Social e Estado para empresas de Ernergia");
                            }
                        }
                        else
                        {
                            resultado.Inconsistencias.Add("Preencha o CatalogEmpresa para classificação Energia");
                        }
                    }
                    if (dados.Classificacao.ToString() == "Executora")
                    {

                        if (dados.CatalogEstadoId > 0)
                        {
                            CatalogEstado CatalogEstado = _context.CatalogEstados.Where(
                                p => p.Id == dados.CatalogEstadoId).FirstOrDefault();

                            if (CatalogEstado == null)
                            {
                                resultado.Inconsistencias.Add("CatalogEstado não localizado.");
                            }
                        }
                        else
                        {
                            resultado.Inconsistencias.Add("Preencha o CatalogEstado para classificação Executora");
                        }
                    }
                    if (dados.Classificacao.ToString() == "Executora" || dados.Classificacao.ToString() == "Parceira")
                    {
                        if (dados.CatalogEmpresaId>0){
                            resultado.Inconsistencias.Add("Não permitido adicionar uma Empreasa Catalogo para essa Classificação");
                        }
                        else{
                            if (String.IsNullOrEmpty(dados.Cnpj))
                            {
                                resultado.Inconsistencias.Add("Preencha o CNPJ da Empresa");
                            }else{
                                if (dados.ProjetoId > 0){
                                    Empresa Empresa = _context.Empresas
                                        .Where(p => p.ProjetoId == dados.ProjetoId)
                                        .Where(p => p.Classificacao == dados.Classificacao)
                                        .Where(p => p.Cnpj == dados.Cnpj).FirstOrDefault();

                                    if (Empresa != null)
                                    {
                                        resultado.Inconsistencias.Add(
                                            "Cnpj já cadastrada para esse projeto. Remova ou Atualize.");
                                    }
                                }
                            }
                            if (String.IsNullOrEmpty(dados.RazaoSocial))
                            {
                                resultado.Inconsistencias.Add("Preencha a Razão Social da Empresa");
                            }else{
                                if (dados.ProjetoId > 0){
                                    Empresa Empresa = _context.Empresas
                                        .Where(p => p.ProjetoId == dados.ProjetoId)
                                        .Where(p => p.Classificacao == dados.Classificacao)
                                        .Where(p => p.RazaoSocial == dados.RazaoSocial).FirstOrDefault();

                                    if (Empresa != null)
                                    {
                                        resultado.Inconsistencias.Add(
                                            "RazaoSocial já cadastrada para esse projeto. Remova ou Atualize.");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return resultado;
        }
        public Resultado Excluir(int id)
        {
            Resultado resultado = new Resultado();
            resultado.Acao = "Exclusão de Empresa";

            Empresa Empresa = _context.Empresas.First(t => t.Id == id);
            if (Empresa == null)
            {
                resultado.Inconsistencias.Add("Empresa não encontrada");
            }
            else
            {
                _context.Empresas.Remove(Empresa);
                _context.SaveChanges();
            }

            return resultado;
        }
    }
}