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
        public RelatorioEmpresa ExtratoFinanceiro(int projetoId)
        {
            RelatorioEmpresa relatorio = new RelatorioEmpresa();
            var Empresas = _context.Empresas
                .Where(p => p.ProjetoId == projetoId)
                .Where(p => p.Classificacao.ToString() == "Proponente" || p.Classificacao.ToString() == "Energia" || p.Classificacao.ToString() == "Parceira")
                .Include("CatalogEmpresa")
                .ToList();

            relatorio.Empresas = new List<RelatorioEmpresas>();
            relatorio.Total = Empresas.Count();
            relatorio.Valor = 0;
            foreach(Empresa empresa in Empresas){
                decimal ValorEmpresa = 0;
                var categorias = new List<RelatorioEmpresaCategorias>();
                string nomeEmpresa = null;
                if (empresa.CatalogEmpresaId>0)
                    nomeEmpresa = empresa.CatalogEmpresa.Nome;
                else
                    nomeEmpresa = empresa.RazaoSocial;
                
                foreach(CategoriaContabil categoria in CategoriaContabil.GetValues(typeof(CategoriaContabil)))
                {   
                    //obter alocações recursos humanos
                    var data = new List<RelatorioEmpresaItems>();
                    int total = 0;
                    decimal ValorCategoria = 0;
                    if (categoria.ToString()=="RH"){
                        var AlocacoesRh = _context.AlocacoesRh
                            .Where(p=>p.EmpresaId == empresa.Id)
                            .Include("RecursoHumano")
                            .Include("Etapa.EtapaProdutos")
                            .ToList();
                        total = AlocacoesRh.Count();
                        if (total>0){
                            foreach(AlocacaoRh a in AlocacoesRh){
                                decimal valor = (a.HrsMes1+a.HrsMes2+a.HrsMes3+a.HrsMes4+a.HrsMes5+a.HrsMes6)*a.RecursoHumano.ValorHora;
                                data.Add(new RelatorioEmpresaItems
                                    {
                                        AlocacaoId = a.Id,
                                        Desc = a.RecursoHumano.NomeCompleto,
                                        Etapa = a.Etapa,
                                        Valor = valor
                                    });
                                ValorCategoria += valor;
                            }
                        }
                    // Fim RH
                    }else{
                        // Outras Categorias
                        var AlocacoesRm = _context.AlocacoesRm
                        .Where(p=>p.EmpresaFinanciadoraId == empresa.Id)
                        .Include(p=>p.RecursoMaterial)
                        .Where(p=>p.RecursoMaterial.CategoriaContabil==categoria)
                        .Include("Etapa.EtapaProdutos")
                        .ToList();
                        total = AlocacoesRm.Count();
                        if (total>0){
                            foreach(AlocacaoRm a in AlocacoesRm){
                                decimal valor = (a.Qtd)*a.RecursoMaterial.ValorUnitario;
                                data.Add(new RelatorioEmpresaItems
                                    {
                                        AlocacaoId = a.Id,
                                        Desc = a.RecursoMaterial.Nome,
                                        Etapa = a.Etapa,
                                        Valor = valor
                                    });
                                ValorCategoria += valor;
                            }
                            
                        }
                    }
                    if (total>0){
                        categorias.Add(new RelatorioEmpresaCategorias{
                            CategoriaContabil = categoria,
                            Desc = categoria.ToString(),
                            Items = data,
                            Total = total,
                            Valor = ValorCategoria
                        });
                    }
                    ValorEmpresa += ValorCategoria;
                }
                // Fim Outros Relatorios
                relatorio.Empresas.Add(new RelatorioEmpresas
                    {
                        Nome = nomeEmpresa,
                        Relatorios = categorias,
                        Total = categorias.Count(),
                        Valor = ValorEmpresa
                    });
                relatorio.Valor += ValorEmpresa;
            }
            return relatorio;
        }

        public IEnumerable<Empresa> ListarTodos(int projetoId)
        {
            var Empresas = _context.Empresas
                .Where(p => p.ProjetoId == projetoId)
                .ToList();
            return Empresas;
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
                _context.Empresas.Add(dados);
                _context.SaveChanges();
                resultado.Id = dados.Id;
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