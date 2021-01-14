using System;
using AutoMapper;
using PeD.Core.ApiModels;
using PeD.Core.Models;

namespace PeD.Mapping
{
    public class ApplicationUserMapping : Profile
    {
        public ApplicationUserMapping()
        {
            CreateMap<ApplicationUser, ApplicationUserDto>()
                .ForMember(user => user.Empresa, opt =>
                    opt.MapFrom(src => src.CatalogEmpresa.Nome ?? src.RazaoSocial))
                .ForMember(user => user.StatusValor, opt => opt.MapFrom(src =>
                    (src.Status != null) ? Enum.GetName(typeof(UserStatus), src.Status) : null
                ))
                .ForMember(user => user.FotoPerfil,
                    opt => opt.MapFrom(src => String.Format("/api/Users/{0}/Avatar", src.Id)));
        }
    }
}