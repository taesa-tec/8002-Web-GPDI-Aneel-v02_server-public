using AutoMapper;
using PeD.Core.Models.Projetos;
using PeD.Core.Models.Propostas;

namespace PeD.Mapping
{
    public class ProjetoMapping : Profile
    {
        public ProjetoMapping()
        {
            CreateMap<Proposta, Projeto>()
                .ForMember(dest => dest.Titulo, opt => opt.MapFrom(src => src.Captacao.Titulo))
                .ForMember(dest => dest.PropostaId, opt => opt.MapFrom(src => src.Id));
            CreateMap<PropostaNode, ProjetoNode>()
                .ForMember(p => p.Id, opt => opt.MapFrom(s => 0));

            CreateMap<Core.Models.Propostas.CoExecutor, Core.Models.Projetos.CoExecutor>()
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
            CreateMap<Core.Models.Propostas.RecursoHumano.AlocacaoRh,
                Core.Models.Projetos.RecursoHumano.AlocacaoRh>().IncludeBase<PropostaNode, ProjetoNode>();
            CreateMap<Core.Models.Propostas.RecursoMaterial.AlocacaoRm,
                Core.Models.Projetos.RecursoMaterial.AlocacaoRm>().IncludeBase<PropostaNode, ProjetoNode>();
            CreateMap<Core.Models.Propostas.Escopo, Core.Models.Projetos.Escopo>()
                .IncludeBase<PropostaNode, ProjetoNode>();
            CreateMap<Core.Models.Propostas.PlanoTrabalho, Core.Models.Projetos.PlanoTrabalho>()
                .IncludeBase<PropostaNode, ProjetoNode>();
            CreateMap<Core.Models.Propostas.Meta, Core.Models.Projetos.Meta>().IncludeBase<PropostaNode, ProjetoNode>();
        }
    }
}