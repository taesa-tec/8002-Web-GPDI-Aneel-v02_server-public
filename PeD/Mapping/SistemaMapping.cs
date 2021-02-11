using AutoMapper;
using PeD.Core.ApiModels.Fornecedores;
using PeD.Core.ApiModels.Propostas;
using PeD.Core.ApiModels.Sistema;
using PeD.Core.Models;
using PeD.Core.Models.Fornecedores;
using PeD.Core.Models.Propostas;
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
                .ReverseMap();
            CreateMap<Clausula, ClausulaDto>().ReverseMap();
        }
    }
}