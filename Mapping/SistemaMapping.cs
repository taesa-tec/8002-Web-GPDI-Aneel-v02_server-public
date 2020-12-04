using APIGestor.Dtos.Captacao.Fornecedor;
using APIGestor.Dtos.Sistema;
using APIGestor.Models.Captacao;
using APIGestor.Models.Fornecedores;
using APIGestor.Requests.Sistema.Fornecedores;
using AutoMapper;

namespace APIGestor.Mapping
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