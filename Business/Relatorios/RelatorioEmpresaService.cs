
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

namespace APIGestor.Business {
    public class RelatorioEmpresaService {
        private GestorDbContext context;

        public RelatorioEmpresaService(GestorDbContext context) {
            this.context = context;
        }

        public OrcamentoEmpresas orcamentoEmpresas(int projetoId) {
            OrcamentoEmpresas orcamentos = new OrcamentoEmpresas();
            return orcamentos;
        }

        public ExtratoEmpresas extratoEmpresas(int projetoId) {
            ExtratoEmpresas extratos = new ExtratoEmpresas();

            return extratos;
        }

        public RelatorioEmpresas ExtratoFinanceiro(int projetoId) {
            RelatorioEmpresas relatorio = new RelatorioEmpresas();
            var Empresas = context.Empresas
                .Where(p => p.ProjetoId == projetoId)
                .Where(p => p.Classificacao.ToString() == "Proponente" || p.Classificacao.ToString() == "Energia" || p.Classificacao.ToString() == "Parceira")
                .Include("CatalogEmpresa")
                .ToList();

            relatorio.Empresas = new List<RelatorioEmpresa>();
            relatorio.Total = Empresas.Count();
            relatorio.Valor = 0;
            foreach (Empresa empresa in Empresas) {
                decimal? ValorEmpresa = 0;
                var categorias = new List<RelatorioEmpresaCategorias>();
                string nomeEmpresa = null;
                if (empresa.CatalogEmpresaId > 0)
                    nomeEmpresa = empresa.CatalogEmpresa.Nome;
                else
                    nomeEmpresa = empresa.RazaoSocial;

                foreach (CategoriaContabil categoria in CategoriaContabil.GetValues(typeof(CategoriaContabil))) {
                    //obter alocações recursos humanos
                    var data = new List<RelatorioEmpresaItem>();
                    int total = 0;
                    decimal? ValorCategoria = 0;

                    if (categoria.ToString() == "RH") {
                        var AlocacoesRh = context.AlocacoesRh
                            .Include("RecursoHumano.Empresa.CatalogEmpresa")
                            .Include("Etapa.EtapaProdutos")
                            .Where(p => p.EmpresaId == empresa.Id)
                            .Where(p => p.RecursoHumano != null)
                            .ToList();

                        total = AlocacoesRh.Count();

                        if (AlocacoesRh != null && total > 0) {
                            foreach (AlocacaoRh a in AlocacoesRh) {
                                decimal? valor = a.HrsTotais * a.RecursoHumano.ValorHora;

                                data.Add(new RelatorioEmpresaItem {
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
                    else {
                        // Outras Categorias
                        var AlocacoesRm = context.AlocacoesRm
                        .Where(p => p.EmpresaFinanciadoraId == empresa.Id)
                        .Include(p => p.RecursoMaterial)
                        .Where(p => p.RecursoMaterial.CategoriaContabil == categoria)
                        .Include("Etapa.EtapaProdutos")
                        .ToList();

                        total = AlocacoesRm.Count();

                        if (total > 0) {
                            foreach (AlocacaoRm a in AlocacoesRm) {
                                decimal? valor = (a.Qtd) * a.RecursoMaterial.ValorUnitario;

                                data.Add(new RelatorioEmpresaItem {
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
                    if (total > 0) {
                        categorias.Add(new RelatorioEmpresaCategorias {
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
                relatorio.Empresas.Add(new RelatorioEmpresa {
                    Id = empresa.Id,
                    Nome = nomeEmpresa,
                    Relatorios = categorias,
                    Total = categorias.Count(),
                    Valor = ValorEmpresa
                });
                relatorio.Valor += ValorEmpresa;
            }
            return relatorio;
        }

        public RelatorioEmpresas ExtratoREFP(int projetoId) {
            var relatorio = ExtratoFinanceiro(projetoId);
            // var records = new List<RelatorioREFP>();

            foreach (var empresa in relatorio.Empresas) {
                int TotalEmpresaAprovado = 0;
                decimal? ValorEmpresaAprovado = 0;

                #region Registro Financeiros Recursos Humanos Aprovados
                var rhs = context.RegistrosFinanceiros
                                .Include("Uploads.User")
                                .Include("RecursoHumano")
                                .Include("ObsInternas.User")
                                .Where(p => p.RecursoHumano.EmpresaId == empresa.Id)
                                .Where(p => p.StatusValor == "Aprovado")
                                .ToList();
                #endregion

                #region Registro Financeiros Recursos Materias Aprovados
                var rms = context.RegistrosFinanceiros
                                .Include("Uploads.User")
                                .Include("RecursoMaterial")
                                .Include("ObsInternas.User")
                                .Where(p => p.EmpresaFinanciadoraId == empresa.Id)
                                .Where(p => p.StatusValor == "Aprovado")
                                .Where(p => p.RecursoMaterialId != null)
                                .ToList();
                #endregion


                foreach (var r in empresa.Relatorios) {
                    int TotalAprovado = 0;
                    decimal? ValorAprovado = 0;
                    if (r.Desc == "RH") {
                        foreach (var rh in rhs) {
                            decimal? valor = (rh.QtdHrs) * rh.RecursoHumano.ValorHora;
                            r.Items.Add(new RelatorioEmpresaItem {
                                //AlocacaoId = a.Id,
                                //RegistroFinanceiro = rh,
                                Desc = rh.RecursoHumano.NomeCompleto,
                                //Etapa = a.Etapa,
                                //AlocacaoRh = a,
                                RecursoHumano = rh.RecursoHumano,
                                Valor = valor
                            });
                            //TotalAprovado ++;
                            //ValorAprovado += valor;
                        }
                    }
                    else {
                        foreach (var rm in rms) {
                            decimal? valor = (rm.QtdItens) * rm.ValorUnitario;
                            r.Items.Add(new RelatorioEmpresaItem {
                                //AlocacaoId = a.Id,
                                Desc = rm.RecursoMaterial.Nome,
                                //Etapa = a.Etapa,
                                //AlocacaoRm = a,
                                RecursoMaterial = rm.RecursoMaterial,
                                Valor = valor
                            });
                        }
                    }

                    foreach (var item in r.Items.ToList()) {
                        if (item.RecursoHumano != null) {
                            RegistroFinanceiro registro = context.RegistrosFinanceiros
                                .Include("Uploads.User")
                                .Include("ObsInternas.User")
                                .Where(p => p.RecursoHumanoId == item.RecursoHumano.Id)
                                .Where(p => p.StatusValor == "Aprovado")
                                .FirstOrDefault();
                            if (registro == null) {
                                r.Items.Remove(item);
                            }
                            else {
                                item.RegistroFinanceiro = registro;
                                TotalAprovado++;
                                ValorAprovado += item.Valor;
                            }
                        }
                        if (item.RecursoMaterial != null) {
                            RegistroFinanceiro registro = context.RegistrosFinanceiros
                                .Include("Uploads.User")
                                .Include("ObsInternas.User")
                                .Where(p => p.RecursoMaterialId == item.RecursoMaterial.Id)
                                .Where(p => p.StatusValor == "Aprovado")
                                .FirstOrDefault();
                            if (registro == null) {
                                r.Items.Remove(item);
                            }
                            else {
                                item.RegistroFinanceiro = registro;
                                TotalAprovado++;
                                ValorAprovado += item.Valor;
                            }
                        }
                    }
                    TotalEmpresaAprovado += TotalAprovado;
                    ValorEmpresaAprovado += ValorAprovado;
                    r.TotalAprovado = TotalAprovado;
                    r.ValorAprovado = ValorAprovado;
                    r.Desvio = (r.Valor == 0 || r.ValorAprovado == 0) ? 0 : (r.ValorAprovado / r.Valor) * 100;
                }
                empresa.TotalAprovado = TotalEmpresaAprovado;
                empresa.ValorAprovado = ValorEmpresaAprovado;
                empresa.Desvio = (empresa.Valor == 0 || empresa.ValorAprovado == 0) ? 0 : (empresa.ValorAprovado / empresa.Valor) * 100;
            }
            return relatorio;
        }


        protected void getTotal_e_Valor(int empresaId, CategoriaContabil categoria, out int total, out decimal valor) {
            if (categoria.ToString() == "RH") {
                var alocacoes = context.AlocacoesRh
                        .Include("RecursoHumano.Empresa.CatalogEmpresa")
                        .Include("Etapa.EtapaProdutos")
                        .Where(p => p.EmpresaId == empresaId)
                        .Where(p => p.RecursoHumano != null);

                total = alocacoes.Count();
                valor = alocacoes.Sum(aloc => aloc.HrsTotais * aloc.RecursoHumano.ValorHora);
            }
            else {
                var alocacoes = context.AlocacoesRm
                .Include(p => p.RecursoMaterial)
                .Include("Etapa.EtapaProdutos")
                .Where(p => p.EmpresaFinanciadoraId == empresaId)
                .Where(p => p.RecursoMaterial.CategoriaContabil == categoria);

                total = alocacoes.Count();
                valor = alocacoes.Sum(aloc => aloc.Qtd * aloc.RecursoMaterial.ValorUnitario);

            }
        }

        public RelatorioEmpresas ExtratoREFP2(int projetoId) {
            RelatorioEmpresas relatorio = new RelatorioEmpresas();
            var empresas = context.Empresas
                .Where(p => p.ProjetoId == projetoId)
                .Where(p => p.Classificacao.ToString() == "Proponente" || p.Classificacao.ToString() == "Energia" || p.Classificacao.ToString() == "Parceira")
                .Include("CatalogEmpresa")
                .ToList();

            relatorio.Empresas = new List<RelatorioEmpresa>();
            relatorio.Total = empresas.Count();
            relatorio.Valor = 0;

            foreach (var empresa in empresas) {

                RelatorioEmpresa relatorioEmpresa = new RelatorioEmpresa {
                    Id = empresa.Id,
                    Nome = empresa.CatalogEmpresaId > 0 ? empresa.CatalogEmpresa.Nome : empresa.RazaoSocial,
                    Relatorios = new List<RelatorioEmpresaCategorias>(),
                    Desvio = 0,
                    Total = 0,
                    TotalAprovado = 0,
                    Valor = 0,
                    ValorAprovado = 0

                };

                var registrosRH = context.RegistrosFinanceiros
                                .Include("Uploads.User")
                                .Include("RecursoHumano")
                                .Include("ObsInternas.User")
                                .Where(p => p.RecursoHumano.EmpresaId == empresa.Id)
                                .Where(p => p.StatusValor == "Aprovado")
                                .ToList();

                var registrosRM = context.RegistrosFinanceiros
                            .Include("Uploads.User")
                            .Include("RecursoMaterial")
                            .Include("ObsInternas.User")
                            .Where(p => p.EmpresaFinanciadoraId == empresa.Id)
                            .Where(p => p.StatusValor == "Aprovado")
                            .Where(p => p.RecursoMaterialId != null)
                            .ToList();

                foreach (CategoriaContabil categoria in CategoriaContabil.GetValues(typeof(CategoriaContabil))) {

                    List<RelatorioEmpresaItem> itens = new List<RelatorioEmpresaItem>();

                    getTotal_e_Valor(empresa.Id, categoria, out int total, out decimal valor);

                    if (total == 0)
                        continue;

                    relatorioEmpresa.Total += total;
                    relatorioEmpresa.Valor += valor;

                    if (categoria.ToString() == "RH") {
                        foreach (var registroRH in registrosRH) {
                            itens.Add(new RelatorioEmpresaItem {
                                RegistroFinanceiro = registroRH,
                                Desc = registroRH.RecursoHumano.NomeCompleto,
                                RecursoHumano = registroRH.RecursoHumano,
                                Valor = registroRH.ValorTotalRH
                            });
                        }
                    }
                    else {

                        foreach (var registroRM in registrosRM.Where(reg => reg.CategoriaContabilValor == categoria.ToString())) {
                            itens.Add(new RelatorioEmpresaItem {
                                RegistroFinanceiro = registroRM,
                                Desc = registroRM.NomeItem,
                                RecursoMaterial = registroRM.RecursoMaterial,
                                Valor = registroRM.ValorTotalRM
                            });
                        }
                    }

                    decimal ValorAprovado = (decimal)itens.Sum(i => i.Valor);
                    decimal Desvio = valor * ValorAprovado == 0 ? 0 : ValorAprovado / valor * 100.0m;

                    relatorioEmpresa.Relatorios.Add(new RelatorioEmpresaCategorias {
                        CategoriaContabil = categoria,
                        Desc = categoria.ToString(),
                        Items = itens,
                        Total = total,
                        Valor = valor,
                        TotalAprovado = itens.Count,
                        ValorAprovado = ValorAprovado,
                        Desvio = Desvio
                    });
                }

                relatorioEmpresa.TotalAprovado = relatorioEmpresa.Relatorios.Sum(r => r.TotalAprovado);
                relatorioEmpresa.ValorAprovado = relatorioEmpresa.Relatorios.Sum(r => r.ValorAprovado);
                relatorio.Empresas.Add(relatorioEmpresa);

            }
            relatorio.Valor = relatorio.Empresas.Sum(emp => emp.Valor);
            return relatorio;
        }


        #region Gerar CSV
        public List<RelatorioEmpresaCsv> FormatRelatorioCsv(RelatorioEmpresas Relatorios) {
            var records = new List<RelatorioEmpresaCsv>();
            foreach (var empresa in Relatorios.Empresas) {
                foreach (var relatorio in empresa.Relatorios) {
                    foreach (var item in relatorio.Items) {
                        var newItem = new RelatorioEmpresaCsv();
                        newItem.NomeRecurso = item.Desc;
                        if (item.RecursoHumano != null) {
                            newItem.CPF = item.RecursoHumano.CPF;
                            newItem.FUNCAO = item.RecursoHumano.FuncaoValor;
                            newItem.TITULACAO = item.RecursoHumano.TitulacaoValor;
                            newItem.CL = item.RecursoHumano.UrlCurriculo;
                            newItem.ValorHora = item.RecursoHumano.ValorHora;
                            newItem.EntidadeRecebedora = (item.RecursoHumano.Empresa.Cnpj == null) ? item.RecursoHumano.Empresa.CatalogEmpresa.Nome : item.RecursoHumano.Empresa.RazaoSocial;
                            newItem.CnpjEntidadeRecebedora = (item.RecursoHumano.Empresa.Cnpj == null) ? null : item.RecursoHumano.Empresa.Cnpj;
                            newItem.CategoriaContabil = CategoriaContabil.RH.ToString();
                        }
                        if (item.AlocacaoRh != null) {
                            newItem.QtdHoras = item.AlocacaoRh.HrsTotais;
                            newItem.ValorTotal = newItem.ValorHora * newItem.QtdHoras;
                            newItem.Justificativa = item.AlocacaoRh.Justificativa;
                            newItem.EntidadePagadora = (item.AlocacaoRh.Empresa.Cnpj == null) ? item.AlocacaoRh.Empresa.CatalogEmpresa.Nome : item.AlocacaoRh.Empresa.RazaoSocial;
                            newItem.CnpjEntidadePagadora = item.AlocacaoRh.Empresa.Cnpj;
                        }
                        if (item.RecursoMaterial != null) {
                            newItem.CategoriaContabil = item.RecursoMaterial.CategoriaContabilValor;
                            newItem.ValorUnitario = item.RecursoMaterial.ValorUnitario;
                            newItem.EspecificacaoTecnica = item.RecursoMaterial.Especificacao;
                        }
                        if (item.AlocacaoRm != null) {
                            newItem.Unidades = item.AlocacaoRm.Qtd;
                            newItem.ValorTotal = newItem.ValorUnitario * newItem.Unidades;
                            newItem.Justificativa = item.AlocacaoRm.Justificativa;
                            newItem.EntidadePagadora = (item.AlocacaoRm.EmpresaFinanciadora.Cnpj == null) ? item.AlocacaoRm.EmpresaFinanciadora.CatalogEmpresa.Nome : item.AlocacaoRm.EmpresaFinanciadora.RazaoSocial;
                            newItem.CnpjEntidadePagadora = (item.AlocacaoRm.EmpresaFinanciadora.Cnpj == null) ? null : item.AlocacaoRm.EmpresaFinanciadora.Cnpj; ;
                            newItem.EntidadeRecebedora = (item.AlocacaoRm.EmpresaRecebedora == null) ? null : (item.AlocacaoRm.EmpresaRecebedora.Cnpj == null) ? item.AlocacaoRm.EmpresaRecebedora.CatalogEmpresa.Nome : item.AlocacaoRm.EmpresaRecebedora.RazaoSocial;
                            newItem.CnpjEntidadeRecebedora = (item.AlocacaoRm.EmpresaRecebedora == null) ? null : (item.AlocacaoRm.EmpresaRecebedora.Cnpj == null) ? null : item.AlocacaoRm.EmpresaRecebedora.Cnpj;
                        }
                        if (item.RegistroFinanceiro != null) {
                            newItem.NomeItem = item.RegistroFinanceiro.NomeItem;
                            newItem.MesReferencia = item.RegistroFinanceiro.Mes.Value.Month.ToString() + "/" + item.RegistroFinanceiro.Mes.Value.Year.ToString();
                            newItem.TipoDocumento = item.RegistroFinanceiro.TipoDocumentoValor;
                            newItem.DataDocumento = item.RegistroFinanceiro.DataDocumento.ToString();
                            newItem.ArquivoComprovante = (item.RegistroFinanceiro.Uploads.FirstOrDefault() != null) ? item.RegistroFinanceiro.Uploads.FirstOrDefault().NomeArquivo : null;
                            newItem.AtividadeRealizada = item.RegistroFinanceiro.AtividadeRealizada;
                            newItem.Beneficiado = item.RegistroFinanceiro.Beneficiado;
                            newItem.ObsInternas = (item.RegistroFinanceiro.ObsInternas.LastOrDefault() != null) ? item.RegistroFinanceiro.ObsInternas.LastOrDefault().Texto : null;
                            newItem.UsuarioAprovacao = (item.RegistroFinanceiro.ObsInternas.LastOrDefault() != null) ? item.RegistroFinanceiro.ObsInternas.LastOrDefault().User.NomeCompleto : null;
                            newItem.EquiparLabExistente = (item.RegistroFinanceiro.EquiparLabExistente.HasValue) ? "Sim" : "Nao";
                            newItem.EquiparLabNovo = (item.RegistroFinanceiro.EquiparLabNovo.HasValue) ? "Sim" : "Nao";
                            newItem.ItemNacional = (item.RegistroFinanceiro.ItemNacional.HasValue) ? "Sim" : "Nao";
                            newItem.DataAprovacao = (item.RegistroFinanceiro.ObsInternas.LastOrDefault() != null) ? item.RegistroFinanceiro.ObsInternas.LastOrDefault().Created.ToString() : null;
                        }
                        newItem.Id = item.AlocacaoId;
                        newItem.Etapa = (item.Etapa != null) ? item.Etapa.Nome : null;

                        records.Add(newItem);
                    }
                }
            }
            return records;
        }
        public MemoryStream ExportarRelatorio(List<RelatorioEmpresaCsv> data, string tipo) {
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
        #endregion

    }
}