using System.Linq;
using AutoMapper;
using PeD.Core.ApiModels.Propostas;
using PeD.Core.Models.Propostas;
using PeD.Core.Requests.Proposta;

namespace PeD.Mapping
{
    public class PropostaMapping : Profile
    {
        public PropostaMapping()
        {
            CreateMap<Proposta, PropostaDto>()
                .ForMember(dest => dest.Fornecedor, opt => opt.MapFrom(src => src.Fornecedor.Nome))
                .ForMember(dest => dest.Captacao, opt => opt.MapFrom(src => src.Captacao.Titulo))
                .ForMember(dest => dest.DataTermino, opt => opt.MapFrom(src => src.Captacao.Termino))
                .ForMember(dest => dest.Consideracoes, opt => opt.MapFrom(src => src.Captacao.Consideracoes))
                .ForMember(dest => dest.Arquivos,
                    opt => opt
                        .MapFrom(src => src.Captacao.Arquivos.Where(a => a.AcessoFornecedor)))
                ;

            CreateMap<PropostaContrato, PropostaContratoDto>()
                .ForMember(c => c.Titulo, opt => opt.MapFrom(src => src.Parent.Titulo));
            CreateMap<PropostaContrato, ContratoListItemDto>()
                .ForMember(c => c.Titulo, opt => opt.MapFrom(src => src.Parent.Titulo));
            CreateMap<PropostaContratoRevisao, ContratoRevisaoDto>();
            CreateMap<PropostaContratoRevisao, ContratoRevisaoListItemDto>()
                .ForMember(r => r.Name, opt => opt.MapFrom(src => src.Parent.Parent.Titulo));
            CreateMap<PlanoTrabalho, PlanoTrabalhoDto>()
                .ForMember(dest => dest.Arquivos, options =>
                    options.MapFrom(src => src.Proposta.Arquivos.Select(a => a.Arquivo)));
            CreateMap<PlanoTrabalhoRequest, PlanoTrabalho>();
        }
    }
}