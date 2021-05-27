using System;
using System.Collections.Generic;
using AutoMapper;
using PeD.Core.Models.Projetos;
using PeD.Core.Models.Propostas;
using PeD.Data;
using PeD.Services.Captacoes;
using TaesaCore.Interfaces;
using TaesaCore.Services;
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

        public Projeto ParseProposta(int propostaId, string numero, string tituloCompleto, string responsavelId,
            DateTime inicio)
        {
            var proposta = _propostaService.GetPropostaFull(propostaId) ?? throw new NullReferenceException();

            var planoTrabalho = Mapper.Map<PlanoTrabalho>(proposta.PlanoTrabalho);
            var escopo = Mapper.Map<Escopo>(proposta.Escopo);
            

            var projeto = new Projeto()
            {
                DataCriacao = DateTime.Now,
                DataAlteracao = DateTime.Now,
                DataInicioProjeto = inicio,
                Numero = numero,
                PropostaId = propostaId,
                TituloCompleto = tituloCompleto,
                ResponsavelId = responsavelId,
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
                etapa.RecursosHumanosAlocacoes = new List<RecursoHumano.AlocacaoRh>();
                etapa.RecursosMateriaisAlocacoes = new List<RecursoMaterial.AlocacaoRm>();
            });
            CopyPropostaNodes<Risco>(projeto.Id, proposta.Riscos);
            CopyPropostaNodes<Meta>(projeto.Id, proposta.Metas);
            CopyPropostaNodes<RecursoHumano.AlocacaoRh>(projeto.Id, proposta.RecursosHumanosAlocacoes,
                a => {
                    a.Recurso = null;
                    a.RecursoId = rhCopy[a.RecursoId];
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
                    a.Recurso = null;
                    a.RecursoId = rmCopy[a.RecursoId];

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
    }
}