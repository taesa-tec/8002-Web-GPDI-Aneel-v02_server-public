using AutoMapper;
using PeD.Dtos.FornecedoresDtos;
using PeD.Dtos.Sistema;
using PeD.Models.Captacao;
using PeD.Models.Fornecedores;
using PeD.Requests.Sistema.Fornecedores;

namespace PeD.Mapping
{
    public class SistemaMapping : Profile
    {
        public SistemaMapping()
        {
            CreateMap<Fornecedor, FornecedorDto>()
                .ForMember(f => f.ResponsavelNome, opt => opt.MapFrom(src => src.Responsavel.NomeCompleto ?? ""))
                .ForMember(f => f.ResponsavelEmail, opt => opt.MapFrom(src => src.Responsavel.Email ?? ""))
                .ReverseMap();
            CreateMap<FornecedorCreateRequest, Fornecedor>().ReverseMap();
            CreateMap<FornecedorEditRequest, Fornecedor>().ReverseMap();
            CreateMap<CoExecutor, CoExecutorDto>()
                .ForMember(c => c.Fornecedor, opt => opt.MapFrom(src => src.Fornecedor.Nome))
                .ReverseMap();
            CreateMap<Clausula, ClausulaDto>().ReverseMap();
        }
    }
}