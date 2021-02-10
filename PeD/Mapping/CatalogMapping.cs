using AutoMapper;
using PeD.Core.ApiModels.Catalogos;
using PeD.Core.Models.Catalogos;

namespace PeD.Mapping
{
    public class CatalogMapping : Profile
    {
        public CatalogMapping()
        {
            CreateMap<Tema, TemaDto>()
                .ForMember(dest => dest.Parent, opt => opt.MapFrom(src => src.Parent.Nome));
        }
    }
}