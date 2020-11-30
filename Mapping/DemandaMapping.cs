using APIGestor.Dtos;
using APIGestor.Dtos.Demandas;
using APIGestor.Models.Demandas;
using AutoMapper;

namespace APIGestor.Mapping
{
    public class DemandaMapping : Profile
    {
        public DemandaMapping()
        {
            CreateMap<DemandaFormHistorico, DemandaFormHistoricoListItemDto>().ReverseMap();
            CreateMap<DemandaFormHistorico, DemandaFormHistoricoDto>().ReverseMap();
            CreateMap<Demanda, DemandaDto>()
                .ForMember(dest => dest.Criador, opt => opt.MapFrom(src => src.Criador.NomeCompleto))
                .ForMember(dest => dest.Revisor, opt => opt.MapFrom(src => src.Revisor.NomeCompleto))
                .ForMember(dest => dest.SuperiorDireto, opt => opt.MapFrom(src => src.SuperiorDireto.NomeCompleto))
                ;
            CreateMap<DemandaComentario, DemandaComentarioDto>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User.NomeCompleto));
        }
    }
}