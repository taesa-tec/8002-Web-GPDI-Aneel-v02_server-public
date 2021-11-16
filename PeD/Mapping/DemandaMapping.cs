using AutoMapper;
using PeD.Core.ApiModels;
using PeD.Core.ApiModels.Demandas;
using PeD.Core.Models.Demandas;

namespace PeD.Mapping
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
            CreateMap<DemandaFormValues, DemandaFormValuesDto>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Object));
            CreateMap<DemandaComentario, DemandaComentarioDto>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User.NomeCompleto));
            CreateMap<DemandaFile, DemandaFileDto>();
            CreateMap<DemandaLog, DemandaLogDto>();
            CreateMap<DemandaFormFile, DemandaFormFileDto>();
        }
    }
}