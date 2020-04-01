using APIGestor.Dtos;
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
        }
    }
}