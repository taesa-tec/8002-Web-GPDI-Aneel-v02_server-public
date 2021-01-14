using System.Linq;
using AutoMapper;
using PeD.Core.ApiModels.Captacao;
using PeD.Core.ApiModels.FornecedoresDtos;
using PeD.Core.Models.Captacao;
using PeD.Core.Models.Fornecedores;

namespace PeD.Mapping
{
    public class CaptacaoMapping : Profile
    {
        public CaptacaoMapping()
        {
            CreateMap<Captacao, CaptacaoPendenteDto>()
                .ForMember(c => c.Criador, opt => opt.MapFrom(src => src.Criador.NomeCompleto))
                .ForMember(c => c.Aprovacao, opt => opt.MapFrom(src => src.CreatedAt));

            CreateMap<Captacao, CaptacaoElaboracaoDto>().ForMember(c => c.UsuarioSuprimento,
                opt => opt.MapFrom(src => src.UsuarioSuprimento.NomeCompleto));

            CreateMap<Captacao, CaptacaoDto>()
                .ForMember(c => c.ConvidadosTotal, opt => opt.MapFrom(src => src.FornecedoresConvidados.Count));

            CreateMap<Captacao, CaptacaoDetalhesDto>()
                .ForMember(c => c.FornecedoresSugeridos,
                    opt => opt.MapFrom(src => src.FornecedoresSugeridos.Select(fs => fs.Fornecedor)));

            CreateMap<CaptacaoArquivo, CaptacaoArquivoDto>()
                .ForMember(dest => dest.Uri,
                    opt => opt.MapFrom(src => $"/api/Captacoes/{src.CaptacaoId}/Arquivos/{src.Id}"))
                ;
            CreateMap<Core.Models.Fornecedores.Fornecedor, FornecedorDto>();
            CreateMap<PropostaFornecedor, PropostaDto>()
                .ForMember(dest => dest.Fornecedor, opt => opt.MapFrom(src => src.Fornecedor.Nome))
                .ForMember(dest => dest.Captacao, opt => opt.MapFrom(src => src.Captacao.Titulo))
                .ForMember(dest => dest.DataTermino, opt => opt.MapFrom(src => src.Captacao.Termino));


            //.ForMember(c => c.PropostaTotal, opt => opt.MapFrom(src => src.Propostas.Count(proposta => proposta.Finalizado)));
        }
    }
}