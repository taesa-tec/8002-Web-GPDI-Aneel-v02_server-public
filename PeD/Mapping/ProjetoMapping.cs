using System.Linq;
using AutoMapper;
using PeD.Core.ApiModels.Projetos;
using PeD.Core.Models.Projetos;
using PeD.Core.Models.Propostas;
using PeD.Core.Requests.Projetos;
using CoExecutor = PeD.Core.Models.Projetos.CoExecutor;
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
        }

        public void PropostaMapping()
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
                    Core.Models.Projetos.RecursoHumano.AlocacaoRh>().IncludeBase<PropostaNode, ProjetoNode>()
                .ForMember(dest => dest.HorasMeses, opt => opt.MapFrom(
                    src => src.HoraMeses.Select(kv => new RecursoHumano.AlocacaoRhHorasMes()
                        {Mes = kv.Key, Horas = kv.Value})))
                .ForMember(dest => dest.RecursoHumanoId, opt => opt.MapFrom(src => src.RecursoId));
            CreateMap<Core.Models.Propostas.RecursoMaterial.AlocacaoRm,
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
                ;
            CreateMap<Etapa, EtapaDto>();
            CreateMap<RecursoHumano, RecursoHumanoDto>()
                .ForMember(dest => dest.Empresa, opt =>
                    opt.MapFrom(src => src.Empresa != null ? src.Empresa.Nome : src.CoExecutor.RazaoSocial ?? ""));
            ;
            CreateMap<RecursoMaterial, RecursoMaterialDto>()
                .ForMember(dest => dest.CategoriaContabil, opt =>
                    opt.MapFrom(src => src.CategoriaContabil.Nome));
            CreateMap<CoExecutor, CoExecutorDto>();

            CreateMap<RegistroFinanceiroRh, RegistroFinanceiroDto>()
                .ForMember(r => r.RecursoHumano, opt => opt.MapFrom(src => src.RecursoHumano.NomeCompleto));
            CreateMap<RegistroFinanceiroRm, RegistroFinanceiroDto>();
            CreateMap<RegistroFinanceiroInfo, RegistroFinanceiroInfoDto>();
            CreateMap<Produto, ProjetoProdutoDto>();
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
    }
}