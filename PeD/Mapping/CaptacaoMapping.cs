using System.Linq;
using AutoMapper;
using PeD.Core.ApiModels.Captacao;
using PeD.Core.ApiModels.Fornecedores;
using PeD.Core.ApiModels.Propostas;
using PeD.Core.Models.Captacoes;
using PeD.Core.Models.Propostas;
using PeD.Views.Email.Captacao;
using Contrato = PeD.Core.Models.Contrato;
using ContratoDto = PeD.Core.ApiModels.Captacao.ContratoDto;

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
                    opt => opt.MapFrom(src => src.FornecedoresSugeridos.Select(fs => fs.Fornecedor)))
                .ForMember(c => c.FornecedoresConvidados,
                    opt => opt.MapFrom(src => src.FornecedoresConvidados.Select(fs => fs.Fornecedor)))
                .ForMember(c => c.Contrato, opt => opt.MapFrom(src => src.Contrato.Titulo))
                .ForMember(c => c.ContratoSugerido, opt => opt.MapFrom(src => src.ContratoSugerido.Titulo))
                ;

            CreateMap<CaptacaoArquivo, CaptacaoArquivoDto>()
                .ForMember(dest => dest.Uri,
                    opt => opt.MapFrom(src => $"/api/Captacoes/{src.CaptacaoId}/Arquivos/{src.Id}"))
                ;
            CreateMap<Core.Models.Fornecedores.Fornecedor, FornecedorDto>();
            CreateMap<Contrato, ContratoDto>();


            //.ForMember(c => c.PropostaTotal, opt => opt.MapFrom(src => src.Propostas.Count(proposta => proposta.Finalizado)));
        }
    }
}