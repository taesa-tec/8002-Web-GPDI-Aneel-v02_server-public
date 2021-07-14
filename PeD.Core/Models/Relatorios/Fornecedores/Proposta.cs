using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Query.Internal;
using PeD.Core.Models.Captacoes;
using PeD.Core.Models.Catalogos;
using PeD.Core.Models.Fornecedores;
using PeD.Core.Models.Propostas;

namespace PeD.Core.Models.Relatorios.Fornecedores
{
    public class Proposta
    {
        public Dictionary<int, string> EmpresasFinanciadoras
        {
            get
            {
                return Etapas.SelectMany(i => i.Alocacoes)
                    .GroupBy(i => i.EmpresaFinanciadoraId)
                    .Select(i => i.First())
                    .ToDictionary(a => a.EmpresaFinanciadoraId, a => a.EmpresaFinanciadora);
            }
        }

        public Dictionary<int, string> EmpresasRecebedoras
        {
            get
            {
                return Etapas.SelectMany(i => i.Alocacoes)
                    .GroupBy(i => i.EmpresaRecebedoraId)
                    .Select(i => i.First())
                    .ToDictionary(a => a.EmpresaRecebedoraId, a => a.EmpresaRecebedora);
            }
        }

        public List<RecursoHumano> RecursosHumanos { get; set; }
        public List<RecursoMaterial> RecursosMateriais { get; set; }
        public List<RecursoHumano.AlocacaoRh> RecursosHumanosAlocacoes { get; set; }
        public List<RecursoMaterial.AlocacaoRm> RecursosMateriaisAlocacoes { get; set; }

        /// <summary>
        /// 1   - Título do Projeto                 Proposta.Captacao.Titulo 
        ///</summary>
        public string Titulo { get; set; }

        /// <summary>
        /// 2   - Meses de duração do projeto       Proposta.Duracao 
        ///</summary>
        public short Duracao { get; set; }

        /// <summary>
        /// 3   - Fases da Cadeia de Inovação       Proposta.Produtos[Classificacao=Final].FaseCadeia 
        ///</summary>
        public FaseCadeiaProduto FaseCadeia { get; set; }

        /// <summary>
        /// 4   - Tema                              Proposta.Captacao.Tema 
        ///</summary>
        public Tema Tema { get; set; }

        /// <summary>
        /// 4   - Tema                              Proposta.Captacao.Tema 
        ///</summary>
        public string TemaOutro { get; set; }

        /// <summary>
        /// 5   - Demanda                           Proposta.Captacao.Subtemas 
        ///</summary>
        public List<CaptacaoSubTema> Demandas { get; set; }

        /// <summary>
        /// 6   - Entidades
        ///         1 - Fornecedor                  Proposta.Fornecedor 
        ///</summary>
        public Fornecedor Fornecedor { get; set; }

        /// <summary>
        /// 6   - Entidades
        ///         2 - CoExecutores                Proposta.CoExecutores 
        ///</summary>
        public List<Propostas.Empresa> Empresas { get; set; }

        /// <summary>
        /// 7   - Justificativa e Motivação         Proposta.PlanoTrabalho.Motivacao 
        ///</summary>
        public string Motivacao { get; set; }

        /// <summary>
        /// 8   - BuscaAnterioridade                Proposta.PlanoTrabalho.BuscaAnterioridade 
        ///</summary>
        public string BuscaAnterioridade { get; set; }

        /// <summary>
        /// 9   - Bibliografia                      Proposta.PlanoTrabalho.Bibliografia 
        ///</summary>
        public string Bibliografia { get; set; }

        /// <summary>
        /// 10  - MetodologiaTrabalho               Proposta.PlanoTrabalho.MetodologiaTrabalho 
        ///</summary>
        public string MetodologiaTrabalho { get; set; }

        /// <summary>
        /// 11  - Escopo 
        ///</summary>
        /// <summary>
        ///         a - Objetivos                   Proposta.PlanoTrabalho.Objetivo 
        ///</summary>
        public string Objetivo { get; set; }

        /// <summary>
        ///         b - Metas                       Proposta.PlanoTrabalho.Metas 
        ///</summary>
        public List<Meta> Metas { get; set; }

        /// <summary>
        ///         c - Resultados Esperados        Proposta.PlanoTrabalho.ResultadoEsperado 
        ///</summary>
        public string ResultadoEsperado { get; set; }

        /// <summary>
        ///         d - Benefícios 
        ///             i - A Taesa                 Proposta.PlanoTrabalho.BeneficioTaesa 
        ///</summary>
        public string BeneficioTaesa { get; set; }

        /// <summary>
        ///         d - Benefícios
        ///             ii - Institucional           Proposta.PlanoTrabalho.BeneficioInstitucional 
        ///</summary>
        public string BeneficioInstitucional { get; set; }

        /// <summary>
        ///         d - Benefícios
        ///             iii - Industria               Proposta.PlanoTrabalho.BeneficioIndustria 
        ///</summary>
        public string BeneficioIndustria { get; set; }


        /// <summary>
        ///         d - Benefícios
        ///             iv - Setor Eletrico          Proposta.PlanoTrabalho.BeneficioSetorEletrico 
        ///</summary>
        public string BeneficioSetorEletrico { get; set; }

        /// <summary>
        ///         d - Benefícios
        ///             v - Sociedade               Proposta.PlanoTrabalho.BeneficioSociedade 
        ///</summary>
        public string BeneficioSociedade { get; set; }

        /// <summary>
        /// 12  - Especificação dos Produtos        Proposta.Produtos 
        ///</summary>
        public List<Produto> Produtos { get; set; }

        /// <summary>
        /// 13  - Originalidade                     Proposta.PlanoTrabalho.Originalidade 
        ///</summary>
        public string Originalidade { get; set; } = "";

        /// <summary>
        /// 14  - Aplicabilidade                    Proposta.PlanoTrabalho.Aplicabilidade 
        ///</summary>
        public string Aplicabilidade { get; set; } = "";

        /// <summary>
        /// 15  - Relevancia                        Proposta.PlanoTrabalho.Relevancia 
        ///</summary>
        public string Relevancia { get; set; } = "";

        /// <summary>
        /// 16  - Razoabilidade Custos              Proposta.PlanoTrabalho.RazoabilidadeCustos 
        ///</summary>
        public string RazoabilidadeCustos { get; set; } = "";

        /// <summary>
        /// 17  - PesquisasCorrelatas
        ///         a - PeD Aneel                   Proposta.PlanoTrabalho.PesquisasCorrelatasPeDAneel 
        ///</summary>
        public string PesquisasCorrelatasPeDAneel { get; set; } = "";

        /// <summary>
        ///  17  - PesquisasCorrelatas
        ///         b - PeD                         Proposta.PlanoTrabalho.PesquisasCorrelatasPeD 
        ///</summary>
        public string PesquisasCorrelatasPeD { get; set; } = "";

        /// <summary>
        ///  17  - PesquisasCorrelatas
        ///         c - Empresa Executora           Proposta.PlanoTrabalho.PesquisasCorrelatasExecutora 
        ///</summary>
        public string PesquisasCorrelatasExecutora { get; set; } = "";

        /// <summary>
        /// 18  - Tabela de Riscos                  Proposta.Riscos 
        ///</summary>
        public List<Risco> Riscos { get; set; }

        /// <summary>
        /// 19  - Experiência Prévia                Proposta.Escopo.ExperienciaPrevia 
        ///</summary>
        public string ExperienciaPrevia { get; set; }

        /// <summary>
        /// 20  - Contrapartidas                    Proposta.Escopo.Contrapartidas 
        ///</summary>
        public string Contrapartidas { get; set; }

        /// <summary>
        /// 22  - Recursos Financeiros - Etapas     Proposta.Etapas 
        ///</summary>
        public List<EtapaRelatorio> Etapas { get; set; }
    }
}