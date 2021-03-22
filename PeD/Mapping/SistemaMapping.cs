using AutoMapper;
using PeD.Core.ApiModels;
using PeD.Core.ApiModels.Fornecedores;
using PeD.Core.ApiModels.Propostas;
using PeD.Core.ApiModels.Sistema;
using PeD.Core.Models;
using PeD.Core.Models.Fornecedores;
using PeD.Core.Models.Propostas;
using PeD.Core.Models.Sistema;
using PeD.Core.Requests.Sistema.Fornecedores;
using TaesaCore.Models;

namespace PeD.Mapping
{
    public class SistemaMapping : Profile
    {
        public SistemaMapping()
        {
            CreateMap<BaseEntity, BaseEntityDto>().ForMember(b => b.Name, opt => opt.MapFrom(src => src.ToString()));
            CreateMap<Core.Models.Fornecedores.Fornecedor, FornecedorDto>()
                .ForMember(f => f.ResponsavelNome, opt => opt.MapFrom(src => src.Responsavel.NomeCompleto ?? ""))
                .ForMember(f => f.ResponsavelEmail, opt => opt.MapFrom(src => src.Responsavel.Email ?? ""))
                .ReverseMap();
            CreateMap<FornecedorCreateRequest, Core.Models.Fornecedores.Fornecedor>().ReverseMap();
            CreateMap<FornecedorEditRequest, Core.Models.Fornecedores.Fornecedor>().ReverseMap();
            CreateMap<CoExecutor, CoExecutorDto>()
                .ReverseMap();
            CreateMap<Clausula, ClausulaDto>().ReverseMap();
            CreateMap<ItemAjuda, ItemAjudaDto>()
                .ForMember(i => i.HasContent, opt => opt.MapFrom(
                    src => !string.IsNullOrWhiteSpace(src.Conteudo)
                ));
        }
    }
}