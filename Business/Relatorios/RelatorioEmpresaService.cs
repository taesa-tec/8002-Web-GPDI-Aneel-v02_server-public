
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using APIGestor.Data;
using APIGestor.Models;
using Microsoft.AspNetCore.Identity;
using APIGestor.Security;
using System.Text;
using System.Reflection;
using System.IO;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;

namespace APIGestor.Business
{
    public class RelatorioEmpresaService
    {
        private GestorDbContext _context;

        public RelatorioEmpresaService(GestorDbContext context)
        {
            _context = context;
        }

        public RelatorioEmpresa ExtratoREFP(int projetoId)
        {
            var relatorio = ExtratoFinanceiro(projetoId);
           // var records = new List<RelatorioREFP>();
        
            foreach(var empresa in relatorio.Empresas)
            {
                int TotalEmpresaAprovado = 0;
                decimal ValorEmpresaAprovado = 0;
                foreach(var r in empresa.Relatorios)
                {
                    int TotalAprovado = 0;
                    decimal ValorAprovado = 0;
                    foreach(var item in r.Items.ToList()){
                        if (item.RecursoHumano!=null){
                            RegistroFinanceiro registro = _context.RegistrosFinanceiros
                                .Include("Uploads.User")
                                .Include("ObsInternas.User")
                                .Where(p => p.RecursoHumanoId == item.RecursoHumano.Id)
                                .Where(p => p.StatusValor=="Aprovado")
                                .FirstOrDefault();
                            if (registro==null){
                                r.Items.Remove(item);
                            }else{
                                item.RegistroFinanceiro=registro;
                                TotalAprovado ++;
                                ValorAprovado += item.Valor;
                            }
                        }
                        if (item.RecursoMaterial!=null){
                            RegistroFinanceiro registro = _context.RegistrosFinanceiros
                                .Include("Uploads.User")
                                .Include("ObsInternas.User")
                                .Where(p => p.RecursoMaterialId == item.RecursoMaterial.Id)
                                .Where(p => p.StatusValor=="Aprovado")
                                .FirstOrDefault();
                            if (registro==null){
                                r.Items.Remove(item);
                            }else{
                                item.RegistroFinanceiro=registro;
                                TotalAprovado ++;
                                ValorAprovado += item.Valor;
                            }
                        }
                    }
                    TotalEmpresaAprovado += TotalAprovado;
                    ValorEmpresaAprovado += ValorAprovado;
                    r.TotalAprovado = TotalAprovado;
                    r.ValorAprovado = ValorAprovado;
                    r.Desvio = (r.Valor==0 || r.ValorAprovado==0)? 0 : (r.ValorAprovado/r.Valor)*100;
                }
                empresa.TotalAprovado = TotalEmpresaAprovado;
                empresa.ValorAprovado = ValorEmpresaAprovado;
                empresa.Desvio = (empresa.Valor==0 || empresa.ValorAprovado==0)? 0 : (empresa.ValorAprovado/empresa.Valor)*100;
            }
            return relatorio;
        }
        public List<RelatorioEmpresaCsv> FormatRelatorioCsv(RelatorioEmpresa Relatorios)
        {
            var records = new List<RelatorioEmpresaCsv>();
            foreach(var empresa in Relatorios.Empresas)
            {
                foreach(var relatorio in empresa.Relatorios)
                {
                    foreach(var item in relatorio.Items){
                        var newItem = new RelatorioEmpresaCsv();
                        newItem.NomeRecurso = item.Desc;
                        if (item.RecursoHumano!=null){
                            newItem.CPF = item.RecursoHumano.CPF;
                            newItem.FUNCAO = item.RecursoHumano.FuncaoValor;
                            newItem.TITULACAO = item.RecursoHumano.TitulacaoValor;
                            newItem.CL = item.RecursoHumano.UrlCurriculo;
                            newItem.ValorHora = item.RecursoHumano.ValorHora;
                            newItem.QtdHoras = item.AlocacaoRh.HrsMes1+item.AlocacaoRh.HrsMes2+item.AlocacaoRh.HrsMes3+item.AlocacaoRh.HrsMes4+item.AlocacaoRh.HrsMes5+item.AlocacaoRh.HrsMes6;
                            newItem.ValorTotal = newItem.ValorHora*newItem.QtdHoras;
                            newItem.Justificativa = item.AlocacaoRh.Justificativa;
                            newItem.EntidadePagadora = (item.AlocacaoRh.Empresa.Cnpj==null) ? item.AlocacaoRh.Empresa.CatalogEmpresa.Nome : item.AlocacaoRh.Empresa.RazaoSocial;
                            newItem.CnpjEntidadePagadora = item.AlocacaoRh.Empresa.Cnpj;
                            newItem.EntidadeRecebedora = (item.RecursoHumano.Empresa.Cnpj==null) ? item.RecursoHumano.Empresa.CatalogEmpresa.Nome : item.RecursoHumano.Empresa.RazaoSocial;
                            newItem.CnpjEntidadeRecebedora = (item.RecursoHumano.Empresa.Cnpj==null) ? null : item.RecursoHumano.Empresa.Cnpj;  
                            newItem.CategoriaContabil = CategoriaContabil.RH.ToString();
                        }
                        if (item.RecursoMaterial!=null){
                            newItem.CategoriaContabil = item.RecursoMaterial.CategoriaContabilValor;
                            newItem.ValorUnitario = item.RecursoMaterial.ValorUnitario;
                            newItem.Unidades = item.AlocacaoRm.Qtd;
                            newItem.ValorTotal = newItem.ValorUnitario*newItem.Unidades;
                            newItem.EspecificacaoTecnica = item.RecursoMaterial.Especificacao;
                            newItem.Justificativa = item.AlocacaoRm.Justificativa;
                            newItem.EntidadePagadora = (item.AlocacaoRm.EmpresaFinanciadora.Cnpj==null) ? item.AlocacaoRm.EmpresaFinanciadora.CatalogEmpresa.Nome : item.AlocacaoRm.EmpresaFinanciadora.RazaoSocial;
                            newItem.CnpjEntidadePagadora = (item.AlocacaoRm.EmpresaFinanciadora.Cnpj==null) ? null : item.AlocacaoRm.EmpresaFinanciadora.Cnpj;  ;
                            newItem.EntidadeRecebedora = (item.AlocacaoRm.EmpresaRecebedora==null) ? null : (item.AlocacaoRm.EmpresaRecebedora.Cnpj==null) ? item.AlocacaoRm.EmpresaRecebedora.CatalogEmpresa.Nome : item.AlocacaoRm.EmpresaRecebedora.RazaoSocial;
                            newItem.CnpjEntidadeRecebedora = (item.AlocacaoRm.EmpresaRecebedora==null) ? null : (item.AlocacaoRm.EmpresaRecebedora.Cnpj==null) ? null : item.AlocacaoRm.EmpresaRecebedora.Cnpj; 
                        }
                        if (item.RegistroFinanceiro!=null){
                            newItem.MesReferencia = item.RegistroFinanceiro.Mes.Value.Month.ToString()+"/"+item.RegistroFinanceiro.Mes.Value.Year.ToString();
                            newItem.TipoDocumento = item.RegistroFinanceiro.TipoDocumentoValor;
                            newItem.DataDocumento = item.RegistroFinanceiro.DataDocumento.ToString();
                            newItem.ArquivoComprovante = (item.RegistroFinanceiro.Uploads.FirstOrDefault()!=null)? item.RegistroFinanceiro.Uploads.FirstOrDefault().NomeArquivo : null;
                            newItem.AtividadeRealizada = item.RegistroFinanceiro.AtividadeRealizada;
                            newItem.Beneficiado = item.RegistroFinanceiro.Beneficiado;
                            newItem.ObsInternas = (item.RegistroFinanceiro.ObsInternas.LastOrDefault()!=null)? item.RegistroFinanceiro.ObsInternas.LastOrDefault().Texto : null;
                            newItem.UsuarioAprovacao = (item.RegistroFinanceiro.ObsInternas.LastOrDefault()!=null)? item.RegistroFinanceiro.ObsInternas.LastOrDefault().User.NomeCompleto : null;
                            newItem.EquiparLabExistente = (item.RegistroFinanceiro.EquiparLabExistente.HasValue) ? "Sim" : "Nao";
                            newItem.EquiparLabNovo = (item.RegistroFinanceiro.EquiparLabNovo.HasValue) ? "Sim" : "Nao";
                            newItem.ItemNacional = (item.RegistroFinanceiro.ItemNacional.HasValue)? "Sim" : "Nao";
                            newItem.DataAprovacao = (item.RegistroFinanceiro.ObsInternas.LastOrDefault()!=null)? item.RegistroFinanceiro.ObsInternas.LastOrDefault().Created.ToString() : null;
                        }
                       newItem.Id = item.AlocacaoId;
                       newItem.Etapa = item.Etapa.Nome;

                       records.Add(newItem);
                    }
                }
            }
            return records;
        }
        public MemoryStream ExportarRelatorio(List<RelatorioEmpresaCsv> data, string tipo)
        {
            var mr = new MemoryStream();
            var tw = new StreamWriter(mr, Encoding.UTF8);
            var csv = new CsvWriter(tw);
            csv.Configuration.Delimiter = ";";
            csv.Configuration.RegisterClassMap(new RelatorioEmpresaCsvMap(tipo));

            csv.WriteRecords(data);
            tw.Flush();
            tw.Close();
            return mr;
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
            foreach (Empresa empresa in Empresas)
            {
                decimal ValorEmpresa = 0;
                var categorias = new List<RelatorioEmpresaCategorias>();
                string nomeEmpresa = null;
                if (empresa.CatalogEmpresaId > 0)
                    nomeEmpresa = empresa.CatalogEmpresa.Nome;
                else
                    nomeEmpresa = empresa.RazaoSocial;

                foreach (CategoriaContabil categoria in CategoriaContabil.GetValues(typeof(CategoriaContabil)))
                {
                    //obter alocações recursos humanos
                    var data = new List<RelatorioEmpresaItems>();
                    int total = 0;
                    decimal ValorCategoria = 0;
                    if (categoria.ToString() == "RH")
                    {
                        var AlocacoesRh = _context.AlocacoesRh
                            .Where(p => p.EmpresaId == empresa.Id)
                            .Where(p => p.RecursoHumano != null)
                            .Include("RecursoHumano")
                            .Include("Etapa.EtapaProdutos")
                            .ToList();
                        total = AlocacoesRh.Count();
                        if (AlocacoesRh != null && total > 0)
                        {
                            foreach (AlocacaoRh a in AlocacoesRh)
                            {
                                decimal valor = (a.HrsMes1 + a.HrsMes2 + a.HrsMes3 + a.HrsMes4 + a.HrsMes5 + a.HrsMes6) * a.RecursoHumano.ValorHora;
                                data.Add(new RelatorioEmpresaItems
                                {
                                    AlocacaoId = a.Id,
                                    Desc = a.RecursoHumano.NomeCompleto,
                                    Etapa = a.Etapa,
                                    AlocacaoRh = a,
                                    RecursoHumano = a.RecursoHumano,
                                    Valor = valor
                                });
                                ValorCategoria += valor;
                            }
                        }
                        // Fim RH
                    }
                    else
                    {
                        // Outras Categorias
                        var AlocacoesRm = _context.AlocacoesRm
                        .Where(p => p.EmpresaFinanciadoraId == empresa.Id)
                        .Where(p => p.RecursoMaterial != null)
                        .Include(p => p.RecursoMaterial)
                        .Where(p => p.RecursoMaterial.CategoriaContabil == categoria)
                        .Include("Etapa.EtapaProdutos")
                        .ToList();
                        total = AlocacoesRm.Count();
                        if (total > 0)
                        {
                            foreach (AlocacaoRm a in AlocacoesRm)
                            {
                                decimal valor = (a.Qtd) * a.RecursoMaterial.ValorUnitario;
                                data.Add(new RelatorioEmpresaItems
                                {
                                    AlocacaoId = a.Id,
                                    Desc = a.RecursoMaterial.Nome,
                                    Etapa = a.Etapa,
                                    AlocacaoRm = a,
                                    RecursoMaterial = a.RecursoMaterial,
                                    Valor = valor
                                });
                                ValorCategoria += valor;
                            }

                        }
                    }
                    if (total > 0)
                    {
                        categorias.Add(new RelatorioEmpresaCategorias
                        {
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
    }
}