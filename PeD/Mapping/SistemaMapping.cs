using AutoMapper;
using PeD.Core.ApiModels.FornecedoresDtos;
using PeD.Core.ApiModels.Sistema;
using PeD.Core.Models.Captacao;
using PeD.Core.Models.Fornecedores;
using PeD.Core.Requests.Sistema.Fornecedores;

namespace PeD.Mapping
{
    public class SistemaMapping : Profile
    {
        public SistemaMapping()
        {
            CreateMap<Core.Models.Fornecedores.Fornecedor, FornecedorDto>()
                .ForMember(f => f.ResponsavelNome, opt => opt.MapFrom(src => src.Responsavel.NomeCompleto ?? ""))
                .ForMember(f => f.ResponsavelEmail, opt => opt.MapFrom(src => src.Responsavel.Email ?? ""))
                .ReverseMap();
            CreateMap<FornecedorCreateRequest, Core.Models.Fornecedores.Fornecedor>().ReverseMap();
            CreateMap<FornecedorEditRequest, Core.Models.Fornecedores.Fornecedor>().ReverseMap();
            CreateMap<CoExecutor, CoExecutorDto>()
                .ForMember(c => c.Fornecedor, opt => opt.MapFrom(src => src.Fornecedor.Nome))
                .ReverseMap();
            CreateMap<Clausula, ClausulaDto>().ReverseMap();
        }
    }
}