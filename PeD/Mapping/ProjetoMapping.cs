using System.Linq;
using AutoMapper;
using PeD.Core.ApiModels.Projetos;
using PeD.Core.ApiModels.Projetos.Resultados;
using PeD.Core.Models.Projetos;
using PeD.Core.Models.Projetos.Resultados;
using PeD.Core.Models.Propostas;
using PeD.Core.Requests.Projetos;
using PeD.Core.Requests.Projetos.Resultados;
using AlocacaoRh = PeD.Core.Models.Propostas.AlocacaoRh;
using AlocacaoRhHorasMes = PeD.Core.Models.Propostas.AlocacaoRhHorasMes;
using Empresa = PeD.Core.Models.Projetos.Empresa;
using Etapa = PeD.Core.Models.Projetos.Etapa;
using Produto = PeD.Core.Models.Projetos.Produto;
using RecursoHumano = PeD.Core.Models.Projetos.RecursoHumano;
using RecursoMaterial = PeD.Core.Models.Projetos.RecursoMaterial;

namespace PeD.Mapping
{
    public class ProjetoMapping : Profile
    {
        public ProjetoMapping()
        {
            PropostaMapping();
            ProjetoResponses();
            ProjetoRequest();
            Relatorios();
        }

        public void PropostaMapping()
        {
            CreateMap<Proposta, Projeto>()
                .ForMember(dest => dest.Titulo, opt => opt.MapFrom(src => src.Captacao.Titulo))
                .ForMember(dest => dest.PropostaId, opt => opt.MapFrom(src => src.Id));
            CreateMap<PropostaNode, ProjetoNode>()
                .ForMember(p => p.Id, opt => opt.MapFrom(s => 0));

            CreateMap<Core.Models.Propostas.Empresa, Core.Models.Projetos.Empresa>()
                .IncludeBase<PropostaNode, ProjetoNode>();
            CreateMap<Core.Models.Propostas.Produto, Core.Models.Projetos.Produto>()
                .IncludeBase<PropostaNode, ProjetoNode>();
            CreateMap<Core.Models.Propostas.Etapa, Core.Models.Projetos.Etapa>()
                .IncludeBase<PropostaNode, ProjetoNode>();
            CreateMap<Core.Models.Propostas.Risco, Core.Models.Projetos.Risco>()
                .IncludeBase<PropostaNode, ProjetoNode>();
            CreateMap<Core.Models.Propostas.RecursoHumano, Core.Models.Projetos.RecursoHumano>()
                .IncludeBase<PropostaNode, ProjetoNode>();
            CreateMap<Core.Models.Propostas.RecursoMaterial, Core.Models.Projetos.RecursoMaterial>()
                .IncludeBase<PropostaNode, ProjetoNode>();
            CreateMap<AlocacaoRhHorasMes, Core.Models.Projetos.AlocacaoRhHorasMes>();
            CreateMap<AlocacaoRh,
                    Core.Models.Projetos.AlocacaoRh>().IncludeBase<PropostaNode, ProjetoNode>()
                .ForMember(dest => dest.HorasMeses, opt => opt.MapFrom(src => src.HorasMeses))
                .ForMember(dest => dest.RecursoHumanoId, opt => opt.MapFrom(src => src.RecursoId));
            CreateMap<AlocacaoRm,
                    Core.Models.Projetos.RecursoMaterial.AlocacaoRm>().IncludeBase<PropostaNode, ProjetoNode>()
                .ForMember(dest => dest.RecursoMaterialId, opt => opt.MapFrom(src => src.RecursoId));
            ;
            CreateMap<Core.Models.Propostas.Escopo, Core.Models.Projetos.Escopo>()
                .IncludeBase<PropostaNode, ProjetoNode>();
            CreateMap<Core.Models.Propostas.PlanoTrabalho, Core.Models.Projetos.PlanoTrabalho>()
                .IncludeBase<PropostaNode, ProjetoNode>();
            CreateMap<Core.Models.Propostas.Meta, Core.Models.Projetos.Meta>().IncludeBase<PropostaNode, ProjetoNode>();
        }

        public void ProjetoResponses()
        {
            CreateMap<Projeto, ProjetoDto>()
                .ForMember(dest => dest.Proponente, opt =>
                    opt.MapFrom(src => src.Proponente.Nome))
                .ForMember(dest => dest.Fornecedor, opt =>
                    opt.MapFrom(src => src.Fornecedor.Nome))
                .ForMember(dest => dest.Tema,
                    opt => opt.MapFrom(src => src.TemaId.HasValue ? (src.Tema.Nome ?? "") : src.TemaOutro))
                ;
            CreateMap<Etapa, EtapaDto>().ForMember(e => e.Produto, o => o.MapFrom(s => s.Produto.Titulo));
            CreateMap<RecursoHumano, RecursoHumanoDto>()
                .ForMember(dest => dest.Empresa, opt =>
                    opt.MapFrom(src => src.Empresa.Nome));
            ;
            CreateMap<RecursoMaterial, RecursoMaterialDto>()
                .ForMember(dest => dest.CategoriaContabil, opt =>
                    opt.MapFrom(src => src.CategoriaContabil.Nome));
            CreateMap<Empresa, EmpresaDto>();

            CreateMap<RegistroFinanceiroRh, RegistroFinanceiroDto>()
                .ForMember(r => r.RecursoHumano, opt => opt.MapFrom(src => src.RecursoHumano.NomeCompleto));
            CreateMap<RegistroFinanceiroRm, RegistroFinanceiroDto>();
            CreateMap<RegistroFinanceiroInfo, RegistroFinanceiroInfoDto>();
            CreateMap<Produto, ProjetoProdutoDto>();
            CreateMap<ProjetoXml, ProjetoXmlDto>()
                .ForMember(dest => dest.File, opt => opt.MapFrom(src => src.File.FileName))
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.File.CreatedAt));
        }

        public void ProjetoRequest()
        {
            CreateMap<RegistroRhRequest, RegistroFinanceiroRh>()
                .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => nameof(RegistroFinanceiroRh)));
            CreateMap<RegistroRmRequest, RegistroFinanceiroRm>()
                .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => nameof(RegistroFinanceiroRm)));
            CreateMap<RegistroObservacao, RegistroObservacaoDto>()
                .ForMember(r => r.Author, opt => opt.MapFrom(src => src.Author.NomeCompleto));

            CreateMap<RecursoHumanoRequest, RecursoHumano>();
            CreateMap<RecursoMaterialRequest, RecursoMaterial>();
        }

        public void Relatorios()
        {
            CreateMap<Apoio, ApoioDto>();
            CreateMap<ApoioRequest, Apoio>();
            CreateMap<IndicadorEconomico, IndicadorEconomicoDto>();
            CreateMap<IndicadorEconomicoRequest, IndicadorEconomico>();
            CreateMap<PropriedadeIntelectualDepositante, PropriedadeIntelectualDepositanteDto>()
                .ForMember(d => d.Depositante, o => o.MapFrom(src => src.Empresa.Nome));

            CreateMap<RelatorioEtapa, RelatorioEtapaDto>()
                .ForMember(d => d.Inicio,
                    o => o.MapFrom(s => s.Projeto.DataInicioProjeto.AddMonths(s.Etapa.Meses.Min() - 1)))
                .ForMember(d => d.Fim,
                    o => o.MapFrom(s => s.Projeto.DataInicioProjeto.AddMonths(s.Etapa.Meses.Max() - 1)))
                ;
            CreateMap<RelatorioEtapaRequest, RelatorioEtapa>();
            CreateMap<Socioambiental, SocioambientalDto>();
            CreateMap<SocioambientalRequest, Socioambiental>();
            CreateMap<Capacitacao, CapacitacaoDto>()
                .ForMember(d => d.Recurso, o => o.MapFrom(s => s.Recurso.NomeCompleto));
            CreateMap<CapacitacaoRequest, Capacitacao>();
            CreateMap<ProducaoCientifica, ProducaoCientificaDto>();
            CreateMap<ProducaoCientificaRequest, ProducaoCientifica>();
            //CreateMap<PropriedadeIntelectualInventores, RecursoHumanoDto>().IncludeMembers(i => i.Recurso);
            CreateMap<PropriedadeIntelectualDepositanteRequest, PropriedadeIntelectualDepositante>();
            CreateMap<PropriedadeIntelectual, PropriedadeIntelectualDto>()
                .ForMember(dest => dest.Inventores,
                    opt => opt.MapFrom(src => src.Inventores.Select(i => i.Recurso)))
                ;
            CreateMap<PropriedadeIntelectualRequest, PropriedadeIntelectual>()
                .ForMember(d => d.Inventores, o => o.MapFrom(s => s.Inventores.Select(i =>
                    new PropriedadeIntelectualInventor()
                    {
                        RecursoId = i,
                        PropriedadeId = s.Id
                    })));
            CreateMap<RelatorioFinal, RelatorioFinalDto>();
            CreateMap<RelatorioFinalRequest, RelatorioFinal>();
        }
    }
}