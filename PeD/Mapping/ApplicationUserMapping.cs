using AutoMapper;
using PeD.Core.ApiModels;
using PeD.Core.Models;
using PeD.Core.Requests.Users;

namespace PeD.Mapping
{
    public class ApplicationUserMapping : Profile
    {
        public ApplicationUserMapping()
        {
            CreateMap<ApplicationUser, ApplicationUserDto>()
                .ForMember(user => user.Empresa, opt =>
                    opt.MapFrom(src => src.Empresa.Nome ?? src.RazaoSocial));

            CreateMap<NewUserRequest, ApplicationUser>()
                .ForMember(dest => dest.EmpresaId, opt =>
                    opt.MapFrom(src => src.EmpresaId == 0 ? null : src.EmpresaId));
            CreateMap<EditUserRequest, ApplicationUser>()
                .ForMember(dest => dest.EmpresaId, opt =>
                    opt.MapFrom(src => src.EmpresaId == 0 ? null : src.EmpresaId));
        }
    }
}