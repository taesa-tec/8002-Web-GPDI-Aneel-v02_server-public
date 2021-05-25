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

            CreateMap<Core.Models.Propostas.CoExecutor, Core.Models.Projetos.CoExecutor>();
            CreateMap<Core.Models.Propostas.Produto, Core.Models.Projetos.Produto>();
            CreateMap<Core.Models.Propostas.Etapa, Core.Models.Projetos.Etapa>();
            CreateMap<Core.Models.Propostas.Risco, Core.Models.Projetos.Risco>();
            CreateMap<Core.Models.Propostas.RecursoHumano, Core.Models.Projetos.RecursoHumano>();
            CreateMap<Core.Models.Propostas.RecursoMaterial, Core.Models.Projetos.RecursoMaterial>();
            CreateMap<Core.Models.Propostas.RecursoHumano.AlocacaoRh,
                Core.Models.Projetos.RecursoHumano.AlocacaoRh>();
            CreateMap<Core.Models.Propostas.RecursoMaterial.AlocacaoRm,
                Core.Models.Projetos.RecursoMaterial.AlocacaoRm>();
            CreateMap<Core.Models.Propostas.Escopo, Core.Models.Projetos.Escopo>();
            CreateMap<Core.Models.Propostas.PlanoTrabalho, Core.Models.Projetos.PlanoTrabalho>();
            CreateMap<Core.Models.Propostas.Meta, Core.Models.Projetos.Meta>();
        }
    }
}