using System.Collections.Generic;
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
            CreateMap<RecursoHumano.AlocacaoRh, AlocacaoRecurso>()
                .ForMember(c => c.CategoriaContabil, opt => opt.MapFrom(s => "RH"))
                .ForMember(c => c.EmpresaFinanciadoraCodigo, opt => opt
                    .MapFrom(s =>
                        s.EmpresaFinanciadoraId != null
                            ? s.EmpresaFinanciadora.Categoria.ToString() + "-" + s.EmpresaFinanciadoraId
                            : "CoExecutor-" + s.CoExecutorFinanciadorId
                    )
                ).ForMember(c => c.EmpresaFinanciadora, opt => opt
                    .MapFrom(s =>
                        s.EmpresaFinanciadoraId != null
                            ? s.EmpresaFinanciadora.Nome
                            : s.CoExecutorFinanciador.RazaoSocial
                    )
                ).ForMember(c => c.EmpresaRecebedoraCodigo, opt => opt
                    .MapFrom(s =>
                        s.Recurso.EmpresaId != null
                            ? s.Recurso.Empresa.Categoria.ToString() + "-" + s.Recurso.Empresa.Id
                            : "CoExecutor-" + s.Recurso.CoExecutorId)
                ).ForMember(c => c.EmpresaRecebedora, opt => opt
                    .MapFrom(s =>
                        s.Recurso.EmpresaId != null ? s.Recurso.Empresa.Nome : s.Recurso.CoExecutor.RazaoSocial
                    )
                );


            CreateMap<RecursoMaterial.AlocacaoRm, AlocacaoRecurso>()
                .ForMember(c => c.CategoriaContabil, opt => opt
                    .MapFrom(s => s.Recurso.CategoriaContabil.Valor)
                )
                .ForMember(c => c.EmpresaFinanciadoraCodigo, opt => opt
                    .MapFrom(s =>
                        s.EmpresaFinanciadoraId != null
                            ? s.EmpresaFinanciadora.Categoria.ToString() + "-" + s.EmpresaFinanciadoraId
                            : "CoExecutor-" + s.CoExecutorFinanciadorId
                    )
                )
                .ForMember(c => c.EmpresaFinanciadora, opt => opt
                    .MapFrom(s =>
                        s.EmpresaFinanciadoraId != null
                            ? s.EmpresaFinanciadora.Nome
                            : s.CoExecutorFinanciador.RazaoSocial
                    )
                )
                .ForMember(c => c.EmpresaRecebedoraCodigo, opt => opt
                    .MapFrom(s =>
                        s.EmpresaRecebedoraId != null
                            ? s.EmpresaRecebedora.Categoria.ToString() + "-" + s.EmpresaRecebedoraId
                            : "CoExecutor-" + s.CoExecutorRecebedorId
                    )
                )
                .ForMember(c => c.EmpresaRecebedora, opt => opt
                    .MapFrom(s =>
                        s.EmpresaRecebedoraId != null
                            ? s.EmpresaRecebedora.Nome
                            : s.CoExecutorRecebedor.RazaoSocial
                    )
                );

            CreateMap<Etapa, EtapaRelatorio>()
                .ForMember(d => d.MesInicio, opt => opt.MapFrom(s => s.Meses.Min()))
                .ForMember(d => d.MesFim, opt => opt.MapFrom(s => s.Meses.Max()))
                .ForMember(d => d.Alocacoes, opt => opt.MapFrom(src =>
                    (new List<Alocacao>()).Concat(src.RecursosHumanosAlocacoes)
                    .Concat(src.RecursosMateriaisAlocacoes)));
        }
    }
}