using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PeD.Core.ApiModels.Projetos;
using PeD.Core.Models.Captacoes;
using PeD.Core.Models.Projetos;
using PeD.Core.Models.Propostas;
using PeD.Data;
using PeD.Services.Captacoes;
using TaesaCore.Interfaces;
using TaesaCore.Services;
using Alocacao = PeD.Core.Models.Projetos.Alocacao;
using CoExecutor = PeD.Core.Models.Projetos.CoExecutor;
using Escopo = PeD.Core.Models.Projetos.Escopo;
using Meta = PeD.Core.Models.Projetos.Meta;
using PlanoTrabalho = PeD.Core.Models.Projetos.PlanoTrabalho;
using Produto = PeD.Core.Models.Projetos.Produto;
using RecursoHumano = PeD.Core.Models.Projetos.RecursoHumano;
using RecursoMaterial = PeD.Core.Models.Projetos.RecursoMaterial;
using Risco = PeD.Core.Models.Projetos.Risco;

namespace PeD.Services.Projetos
{
    public class ProjetoService : BaseService<Projeto>
    {
        private PropostaService _propostaService;
        private GestorDbContext _context;
        private IMapper Mapper;

        public ProjetoService(IRepository<Projeto> repository, PropostaService propostaService, GestorDbContext context,
            IMapper mapper)
            : base(repository)
        {
            _propostaService = propostaService;
            _context = context;
            Mapper = mapper;
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
            DateTime inicio)
        {
            var proposta = _propostaService.GetPropostaFull(propostaId) ?? throw new NullReferenceException();

            var relatorio = _propostaService.GetRelatorio(propostaId);
            var contrato = _propostaService.GetContrato(propostaId);
            var captacao = _context.Set<Captacao>().FirstOrDefault(c => c.Id == proposta.CaptacaoId);

            if (captacao is null)
            {
                // @todo disparar um email se isso acontecer, captaçõa não pode mais ser excluida nesse ponto
                throw new Exception("Captacao não encontrada");
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
            var projeto = new Projeto()
            {
                PlanoTrabalhoFileId = relatorio.FileId.Value,
                ContratoId = contrato.FileId.Value,
                EspecificacaoTecnicaFileId = captacao.EspecificacaoTecnicaFileId.Value,
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
                Escopo = escopo
            };
            Post(projeto);


            var coexecutorCopy = CopyPropostaNodes<CoExecutor>(projeto.Id, proposta.CoExecutores);
            var produtosCopy = CopyPropostaNodes<Produto>(projeto.Id, proposta.Produtos);

            var rhCopy = CopyPropostaNodes<RecursoHumano>(projeto.Id, proposta.RecursosHumanos, rh =>
            {
                if (rh.CoExecutorId != null)
                {
                    rh.CoExecutor = null;
                    rh.CoExecutorId = coexecutorCopy[rh.CoExecutorId.Value];
                }
            });
            var rmCopy = CopyPropostaNodes<RecursoMaterial>(projeto.Id, proposta.RecursosMateriais);
            var etapaCopy = CopyPropostaNodes<Core.Models.Projetos.Etapa>(projeto.Id, proposta.Etapas, etapa =>
            {
                etapa.Produto = null;
                etapa.ProdutoId = produtosCopy[etapa.ProdutoId];
                etapa.Alocacoes = new List<Alocacao>();
            });
            CopyPropostaNodes<Risco>(projeto.Id, proposta.Riscos);
            CopyPropostaNodes<Meta>(projeto.Id, proposta.Metas);
            CopyPropostaNodes<RecursoHumano.AlocacaoRh>(projeto.Id, proposta.RecursosHumanosAlocacoes,
                a =>
                {
                    a.RecursoHumano = null;
                    a.RecursoHumanoId = rhCopy[a.RecursoHumanoId];
                    a.Etapa = null;
                    a.EtapaId = etapaCopy[a.EtapaId];

                    if (a.CoExecutorFinanciadorId != null)
                    {
                        a.CoExecutorFinanciadorId = coexecutorCopy[a.CoExecutorFinanciadorId.Value];
                    }
                });
            CopyPropostaNodes<RecursoMaterial.AlocacaoRm>(projeto.Id, proposta.RecursosMateriaisAlocacoes,
                a =>
                {
                    a.RecursoMaterial = null;
                    a.RecursoMaterialId = rmCopy[a.RecursoMaterialId];

                    a.Etapa = null;
                    a.EtapaId = etapaCopy[a.EtapaId];
                    a.CoExecutorRecebedor = null;
                    a.CoExecutorFinanciador = null;

                    if (a.CoExecutorFinanciadorId != null)
                    {
                        a.CoExecutorFinanciadorId = coexecutorCopy[a.CoExecutorFinanciadorId.Value];
                    }

                    if (a.CoExecutorRecebedorId != null)
                    {
                        a.CoExecutorRecebedorId = coexecutorCopy[a.CoExecutorRecebedorId.Value];
                    }
                });

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

        public IEnumerable<object> GetExtrato(int projetoId)
        {
            //Previsto
            var orcamentos = GetOrcamentos(projetoId);
            //Realizado
            var extratos = GetRegistrosFinanceiros(projetoId, StatusRegistro.Aprovado);
            // Financiadores
            var empresas = (new[]
                {
                    orcamentos.Select(o => new {o.Financiador, o.FinanciadorCode}),
                    extratos.Select(o => new {o.Financiador, o.FinanciadorCode})
                }).SelectMany(i => i)
                .GroupBy(e => e.FinanciadorCode)
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
                Codigo = e.FinanciadorCode,
                Categorias = categorias.Select(c => new ExtratoFinanceiroGroupRefpDto()
                {
                    Nome = c.CategoriaContabil,
                    Codigo = c.CategoriaContabilCodigo,
                    Orcamento = orcamentos.Where(o =>
                        o.FinanciadorCode == e.FinanciadorCode &&
                        o.CategoriaContabilCodigo == c.CategoriaContabilCodigo),
                    Registros = extratos.Where(o =>
                        o.FinanciadorCode == e.FinanciadorCode &&
                        o.CategoriaContabilCodigo == c.CategoriaContabilCodigo)
                }).Where(ef => ef.Previsto != 0 || ef.Realizado != 0)
            });
        }
    }
}