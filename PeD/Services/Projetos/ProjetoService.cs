using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using AutoMapper;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using PeD.Core.ApiModels.Projetos;
using PeD.Core.Models;
using PeD.Core.Models.Captacoes;
using PeD.Core.Models.Projetos;
using PeD.Core.Models.Projetos.Resultados;
using PeD.Core.Models.Projetos.Xml;
using PeD.Core.Models.Projetos.Xml.InicioExecucao;
using PeD.Core.Models.Projetos.Xml.Interesse;
using PeD.Core.Models.Projetos.Xml.ProjetoPeD;
using PeD.Core.Models.Propostas;
using PeD.Data;
using PeD.Services.Captacoes;
using PeD.Services.Projetos.Xml;
using TaesaCore.Interfaces;
using TaesaCore.Services;
using Alocacao = PeD.Core.Models.Projetos.Alocacao;
using AlocacaoRh = PeD.Core.Models.Projetos.AlocacaoRh;
using Empresa = PeD.Core.Models.Empresa;
using Escopo = PeD.Core.Models.Projetos.Escopo;
using Log = Serilog.Log;
using Meta = PeD.Core.Models.Projetos.Meta;
using PlanoTrabalho = PeD.Core.Models.Projetos.PlanoTrabalho;
using Produto = PeD.Core.Models.Projetos.Produto;
using Projeto = PeD.Core.Models.Projetos.Projeto;
using RecursoHumano = PeD.Core.Models.Projetos.RecursoHumano;
using RecursoMaterial = PeD.Core.Models.Projetos.RecursoMaterial;
using Risco = PeD.Core.Models.Projetos.Risco;

namespace PeD.Services.Projetos
{
    public class ProjetoService : BaseService<Projeto>
    {
        private PropostaService _propostaService;
        private GestorDbContext _context;
        private ArquivoService _arquivoService;
        private XlsxService _xlsxService;
        private IMapper Mapper;
        private ProjetoPeDService _projetoPeDService;

        public ProjetoService(IRepository<Projeto> repository,
            PropostaService propostaService,
            GestorDbContext context,
            IMapper mapper, ArquivoService arquivoService, XlsxService xlsxService, ProjetoPeDService projetoPeDService)
            : base(repository)
        {
            _propostaService = propostaService;
            _context = context;
            Mapper = mapper;
            _arquivoService = arquivoService;
            _xlsxService = xlsxService;
            _projetoPeDService = projetoPeDService;
        }

        #region Proposta para Projeto

        protected Dictionary<int, int> CopyPropostaNodes<TOut>(int projetoId, IEnumerable<PropostaNode> list,
            Action<TOut> actionNode = null)
            where TOut : ProjetoNode
        {
            var map = new Dictionary<int, int>();
            foreach (var item in list)
            {
                var model = Mapper.Map<TOut>(item);
                model.ProjetoId = projetoId;
                if (actionNode != null)
                {
                    actionNode(model);
                }

                _context.Add(model);
                _context.SaveChanges();

                map.Add(item.Id, model.Id);
            }

            return map;
        }

        public Projeto ParseProposta(int propostaId, int proponentId, string numero, string tituloCompleto,
            string responsavelId,
            TipoCompartilhamento compartilhamento,
            DateTime inicio)
        {
            var proposta = _propostaService.GetPropostaFull(propostaId) ?? throw new NullReferenceException();

            var relatorio = _propostaService.GetRelatorio(propostaId);
            var contrato = _propostaService.GetContrato(propostaId);
            var captacao = _context.Set<Captacao>().Include(c => c.SubTemas)
                .FirstOrDefault(c => c.Id == proposta.CaptacaoId);
            var proponente = _context.Set<Empresa>().FirstOrDefault(e => e.Id == proponentId);

            if (captacao is null)
            {
                // @todo disparar um email se isso acontecer, captaçõa não pode mais ser excluida nesse ponto
                throw new Exception("Captacao não encontrada");
            }

            if (proponente is null)
            {
                throw new Exception("Proponente não encontrado");
            }

            if (relatorio is null || !relatorio.FileId.HasValue)
            {
                // @todo gerar documento se não houver
                throw new Exception("Plano de trabalho não encontrado");
            }

            if (contrato is null || !contrato.FileId.HasValue)
            {
                // @todo gerar documento se não houver
                throw new Exception("Contrato não encontrado");
            }


            var planoTrabalho = Mapper.Map<PlanoTrabalho>(proposta.PlanoTrabalho);
            var escopo = Mapper.Map<Escopo>(proposta.Escopo);

            var dataInicio = new DateTime(inicio.Year, inicio.Month, 1);
            var codigo = $"PD-{proponente.Valor}-{numero}/{inicio.Year}";
            var projeto = new Projeto()
            {
                Compartilhamento = compartilhamento,
                PlanoTrabalhoFileId = relatorio.FileId.Value,
                Codigo = codigo,
                ContratoId = contrato.FileId.Value,
                EspecificacaoTecnicaFileId = captacao.EspecificacaoTecnicaFileId,
                TemaId = captacao.TemaId,
                TemaOutro = captacao.TemaOutro,
                DataCriacao = DateTime.Now,
                DataAlteracao = DateTime.Now,
                DataInicioProjeto = dataInicio,
                DataFinalProjeto = dataInicio.AddMonths(proposta.Duracao - 1),
                Numero = numero,
                PropostaId = propostaId,
                TituloCompleto = tituloCompleto,
                ResponsavelId = responsavelId,
                ProponenteId = proponentId,
                Titulo = proposta.Captacao.Titulo,
                CaptacaoId = proposta.CaptacaoId,
                FornecedorId = proposta.FornecedorId,
                PlanoTrabalho = planoTrabalho,
                Escopo = escopo,
                SubTemas = captacao.SubTemas
                    .Select(s => new ProjetoSubTema() {Outro = s.Outro, SubTemaId = s.SubTemaId})
                    .ToList()
            };
            Post(projeto);


            var empresasCopy = CopyPropostaNodes<Core.Models.Projetos.Empresa>(projeto.Id, proposta.Empresas);
            var produtosCopy = CopyPropostaNodes<Produto>(projeto.Id, proposta.Produtos);

            var rhCopy = CopyPropostaNodes<RecursoHumano>(projeto.Id, proposta.RecursosHumanos,
                r => { r.EmpresaId = empresasCopy[r.EmpresaId]; });
            var rmCopy = CopyPropostaNodes<RecursoMaterial>(projeto.Id, proposta.RecursosMateriais);
            var etapaCopy = CopyPropostaNodes<Core.Models.Projetos.Etapa>(projeto.Id, proposta.Etapas, etapa =>
            {
                etapa.Produto = null;
                etapa.ProdutoId = produtosCopy[etapa.ProdutoId];
                etapa.Alocacoes = new List<Alocacao>();
            });
            CopyPropostaNodes<Risco>(projeto.Id, proposta.Riscos);
            CopyPropostaNodes<Meta>(projeto.Id, proposta.Metas);
            CopyPropostaNodes<AlocacaoRh>(projeto.Id, proposta.RecursosHumanosAlocacoes,
                a =>
                {
                    a.RecursoHumano = null;
                    a.RecursoHumanoId = rhCopy[a.RecursoHumanoId];
                    a.Etapa = null;
                    a.EtapaId = etapaCopy[a.EtapaId];

                    a.EmpresaFinanciadoraId = empresasCopy[a.EmpresaFinanciadoraId];
                });
            CopyPropostaNodes<RecursoMaterial.AlocacaoRm>(projeto.Id, proposta.RecursosMateriaisAlocacoes,
                a =>
                {
                    a.RecursoMaterial = null;
                    a.RecursoMaterialId = rmCopy[a.RecursoMaterialId];
                    a.Etapa = null;
                    a.EtapaId = etapaCopy[a.EtapaId];
                    a.EmpresaRecebedora = null;
                    a.EmpresaFinanciadora = null;
                    a.EmpresaFinanciadoraId = empresasCopy[a.EmpresaFinanciadoraId];
                    a.EmpresaRecebedoraId = empresasCopy[a.EmpresaRecebedoraId];
                });
            SaveXml(projeto.Id, "1",
                new InicioExecucao(projeto.Codigo, projeto.DataInicioProjeto,
                    compartilhamento.ToString()));
            SaveXml(projeto.Id, "1", new Interesse(projeto.Codigo, true));
            SaveXml(projeto.Id, "1", _projetoPeDService.ProjetoPed(projeto.Id));
            return projeto;
        }

        #endregion

        public List<T> NodeList<T>(int projetoId) where T : ProjetoNode
        {
            return _context.Set<T>().Where(r => r.ProjetoId == projetoId).ToList();
        }

        public List<T> NodeList<T>(int projetoId, Func<IQueryable<T>, IQueryable<T>> query)
            where T : ProjetoNode
        {
            return query(_context.Set<T>().Where(r => r.ProjetoId == projetoId)).ToList();
        }

        public List<Orcamento> GetOrcamentos(int projetoId)
        {
            return _context.Set<Orcamento>().Where(o => o.ProjetoId == projetoId).ToList();
        }

        public List<RegistroFinanceiroInfo> GetRegistrosFinanceiros(int projetoId, StatusRegistro status)
        {
            return _context.Set<RegistroFinanceiroInfo>().Where(o => o.ProjetoId == projetoId && o.Status == status)
                .ToList();
        }

        public IEnumerable<ExtratoFinanceiroEmpresaRefpDto> GetExtrato(int projetoId)
        {
            //Previsto
            var orcamentos = GetOrcamentos(projetoId);
            //Realizado
            var extratos = GetRegistrosFinanceiros(projetoId, StatusRegistro.Aprovado);
            // Financiadores
            var empresas = (new[]
                {
                    orcamentos.Select(o => new {o.Financiador, o.FinanciadoraId}),
                    extratos.Select(o => new {o.Financiador, o.FinanciadoraId})
                }).SelectMany(i => i)
                .GroupBy(e => e.FinanciadoraId)
                .Select(e => e.First());
            var categorias = (new[]
                {
                    orcamentos.Select(o => new {o.CategoriaContabil, o.CategoriaContabilCodigo}),
                    extratos.Select(o => new {o.CategoriaContabil, o.CategoriaContabilCodigo})
                }).SelectMany(i => i)
                .GroupBy(e => e.CategoriaContabilCodigo)
                .Select(e => e.First());


            return empresas.Select(e => new ExtratoFinanceiroEmpresaRefpDto()
            {
                Nome = e.Financiador,
                Codigo = e.FinanciadoraId,
                Categorias = categorias.Select(c => new ExtratoFinanceiroGroupRefpDto()
                {
                    Nome = c.CategoriaContabil,
                    Codigo = c.CategoriaContabilCodigo,
                    Orcamento = orcamentos.Where(o =>
                        o.FinanciadoraId == e.FinanciadoraId &&
                        o.CategoriaContabilCodigo == c.CategoriaContabilCodigo),
                    Registros = extratos.Where(o =>
                        o.FinanciadoraId == e.FinanciadoraId &&
                        o.CategoriaContabilCodigo == c.CategoriaContabilCodigo)
                }).Where(ef => ef.Previsto != 0 || ef.Realizado != 0)
            });
        }

        public XLWorkbook XlsExtrato(int projetoId)
        {
            var doc = new XLWorkbook();
            var extratos = GetExtrato(projetoId).ToArray();
            var geral = doc.AddWorksheet("Geral");
            geral.ColumnWidth = 20;
            var registros = extratos.SelectMany(r => r.Categorias.SelectMany(c => c.Registros)).ToArray();
            var table = _xlsxService.DataTableFrom(registros);
            geral.FirstCell().InsertTable(table);
            var geraRange = geral.RangeUsed();
            geraRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            geraRange.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;

            foreach (var extrato in extratos)
            {
                var tableCat = _xlsxService.DataTableFrom(extrato.Categorias);
                var tableRegistros = _xlsxService.DataTableFrom(extrato.Categorias.SelectMany(c => c.Registros));
                var aba = doc.AddWorksheet(extrato.Nome);
                var header = aba.Cell("A1");
                header.SetValue(extrato.Nome);
                header.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                aba.Range("A1:F1").Row(1).Merge();
                aba.ColumnWidth = 20;
                aba.Cell("A2").InsertTable(tableCat);
                var range = aba.Range(1, 1, aba.LastRowUsed().RowNumber(), 6);

                range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                range.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;

                aba.LastRowUsed().RowBelow(2).FirstCell().SetValue("Registros");
                var rowNumber = aba.LastRowUsed().RowNumber();

                header = aba.Range(rowNumber, 1, rowNumber, 6).FirstCell();
                header.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                aba.Range(rowNumber, 1, rowNumber, 6).Row(1).Merge();
                aba.LastRowUsed().RowBelow(1).FirstCell().InsertTable(tableRegistros);
                range = aba.Range(rowNumber, 1, aba.LastRowUsed().RowNumber(), 8);
                range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                range.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
            }


            return doc;
        }

        public ProjetoXml SaveXml(int projetoId, string versao, BaseXml xml)
        {
            var projeto = _context.Set<Projeto>().Include(p => p.Proponente).FirstOrDefault(p => p.Id == projetoId);
            if (projeto is null)
                return null;
            xml.Attributes.Add("CodigoEmpresa", projeto.Proponente.Valor);

            var document = xml.ToXml();
            //filename =  APLPED + Codigo Empresa _ Tipo _ Numero _ Versao
            var filename = $"APLPED{projeto.Proponente.Valor}_{xml.Tipo.ToString()}_{projeto.Numero}_{versao}.XML";
            var tempFileName = Path.GetTempFileName();
            document.Save(tempFileName);

            string[] lines = File.ReadAllLines(tempFileName, System.Text.Encoding.GetEncoding("ISO-8859-1"));

            lines[0] = Regex.Replace(lines[0], "encoding=\"iso-8859-1\"", "encoding=\"ISO8859-1\"");

            File.WriteAllLines(tempFileName, lines, System.Text.Encoding.GetEncoding("ISO-8859-1"));

            var file = _arquivoService.FromPath(tempFileName, "application/xml", filename);

            var projetoFile = new ProjetoXml()
            {
                ProjetoId = projetoId,
                FileId = file.Id,
                Tipo = xml.Tipo,
                Versao = versao
            };
            _context.Add(projetoFile);
            _context.SaveChanges();
            projetoFile.File = file;
            return projetoFile;
        }

        public List<RelatorioEtapa> RelatoriosEtapas(int projetoId)
        {
            var projeto = _context.Set<Projeto>().Include(p => p.Etapas).ThenInclude(e => e.Relatorio)
                .FirstOrDefault(p => p.Id == projetoId);
            if (projeto is null)
                return null;
            var etapas = projeto.Etapas;
            var relatorios = new List<RelatorioEtapa>();
            foreach (var etapa in etapas)
            {
                var relatorio = etapa.Relatorio ?? new RelatorioEtapa() {ProjetoId = projetoId, EtapaId = etapa.Id};
                if (relatorio.Id == 0)
                    _context.Add(relatorio);
                relatorios.Add(relatorio);
            }

            _context.SaveChanges();

            return relatorios;
        }
    }
}