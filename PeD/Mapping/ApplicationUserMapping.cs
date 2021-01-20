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
                    opt.MapFrom(src => src.Empresa.Nome ?? src.RazaoSocial));
        }
    }
}