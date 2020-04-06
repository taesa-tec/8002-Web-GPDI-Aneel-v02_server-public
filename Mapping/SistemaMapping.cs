using APIGestor.Dtos.Captacao.Fornecedor;
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
                .ForMember(f => f.Responsavel, opt => opt.MapFrom(src => src.Responsavel.NomeCompleto ?? ""))
                .ReverseMap();
            CreateMap<FornecedorCreateRequest, Fornecedor>().ReverseMap();
            CreateMap<FornecedorEditRequest, Fornecedor>().ReverseMap();
            CreateMap<CoExecutor, CoExecutorDto>()
                .ForMember(c => c.Fornecedor, opt => opt.MapFrom(src => src.Fornecedor.Nome))
                .ReverseMap();
        }
    }
}