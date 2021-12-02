using System.Linq;
using AutoMapper;
using PeD.Core.Models.Propostas;
using PeD.Core.Models.Relatorios.Fornecedores;

namespace PeD.Mapping
{
    public class RelatorioMapping : Profile
    {
        public RelatorioMapping()
        {
            CreateMap<AlocacaoRh, AlocacaoRecurso>()
                .ForMember(c => c.EmpresaFinanciadoraFuncao, o => o.MapFrom(s => s.EmpresaFinanciadora.Funcao))
                .ForMember(c => c.EmpresaRecebedoraFuncao, o => o.MapFrom(s => s.Recurso.Empresa.Funcao))
                .ForMember(c => c.CategoriaContabil, opt => opt.MapFrom(s => "RH"))
                .ForMember(c => c.EmpresaFinanciadora, opt => opt
                    .MapFrom(s => s.EmpresaFinanciadora.Nome)
                ).ForMember(c => c.EmpresaRecebedora, opt => opt
                    .MapFrom(s => s.Recurso.Empresa.Nome)
                );


            CreateMap<AlocacaoRm, AlocacaoRecurso>()
                .ForMember(c => c.CategoriaContabil, opt => opt
                    .MapFrom(s => s.Recurso.CategoriaContabil.Valor)
                )
                .ForMember(c => c.EmpresaFinanciadora, opt => opt
                    .MapFrom(s => s.EmpresaFinanciadora.Nome)
                )
                .ForMember(c => c.EmpresaRecebedora, opt => opt
                    .MapFrom(s => s.EmpresaRecebedora.Nome)
                );

            CreateMap<Etapa, EtapaRelatorio>()
                .ForMember(d => d.MesInicio, opt => opt.MapFrom(s => s.Meses.Min()))
                .ForMember(d => d.MesFim, opt => opt.MapFrom(s => s.Meses.Max()))
                .ForMember(d => d.Alocacoes, opt => opt.Ignore());
        }
    }
}