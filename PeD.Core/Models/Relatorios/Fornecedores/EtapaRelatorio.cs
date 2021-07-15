using System.Collections.Generic;
using System.Linq;
using PeD.Core.Models.Propostas;

namespace PeD.Core.Models.Relatorios.Fornecedores
{
    public class EtapaRelatorio
    {
        public static List<AlocacaoRecurso> AgruparPorCategoria(List<AlocacaoRecurso> list)
        {
            return list.GroupBy(a => a.CategoriaContabil)
                .Select(i => new AlocacaoRecurso()
                {
                    CategoriaContabil = i.Key,
                    Valor = i.Sum(a => a.Valor)
                })
                .ToList();
        }

        public static Dictionary<int, List<AlocacaoRecurso>> AgruparPorEmpresaRecebedora(List<AlocacaoRecurso> list)
        {
            return list.GroupBy(i => i.EmpresaRecebedoraId)
                .ToDictionary(i => i.Key, i => i.ToList());
        }

        public static decimal CustoPorCategoria(List<AlocacaoRecurso> list, string categoria)
        {
            return list.Where(a => a.CategoriaContabil == categoria).Sum(i => i.Valor);
        }

        public static decimal CustoEntreEmpresas(List<AlocacaoRecurso> list, int financiadoraId, int recebedoraId)
        {
            return list.Where(i =>
                    i.EmpresaFinanciadoraId == financiadoraId && i.EmpresaRecebedoraId == recebedoraId)
                .Sum(x => x.Valor);
        }

        public static decimal CustoFinanciadora(List<AlocacaoRecurso> list, int financiadoraId, bool interno = true)
        {
            return list.Where(i =>
                    i.EmpresaFinanciadoraId == financiadoraId &&
                    (interno || i.EmpresaRecebedoraId != financiadoraId))
                .Sum(x => x.Valor);
        }

        public static decimal CustoFinanciadora(List<EtapaRelatorio> list, int financiadoraId, bool interno = true)
        {
            return CustoFinanciadora(list.SelectMany(e => e.Alocacoes).ToList(), financiadoraId, interno);
        }

        public string DescricaoAtividades { get; set; }
        public short MesInicio { get; set; }
        public short MesFim { get; set; }
        public Produto Produto { get; set; }
        public List<int> Meses { get; set; }
        public short Ordem { get; set; }

        public decimal CustoTotal(bool interno = true)
        {
            if (interno)
                return Alocacoes.Sum(a => a.Valor);
            return Alocacoes.Where(i => i.EmpresaFinanciadoraId != i.EmpresaRecebedoraId).Sum(a => a.Valor);
        }

        public List<AlocacaoRecurso> Alocacoes { get; set; }

        public decimal AlocacoesInternasSum => AlocacoesInternas.Sum(a => a.Valor);

        public List<AlocacaoRecurso> AlocacoesInternas
        {
            get
            {
                return Alocacoes.Where(c => c.EmpresaFinanciadoraId == c.EmpresaRecebedoraId)
                    .ToList();
            }
        }

        public List<AlocacaoRecurso> AlocacoesExternas
        {
            get
            {
                return Alocacoes
                    .Where(c => c.EmpresaFinanciadoraFuncao == Funcao.Cooperada &&
                                c.EmpresaRecebedoraFuncao == Funcao.Executora)
                    .ToList();
            }
        }


        public Dictionary<int, Dictionary<int, List<AlocacaoRecurso>>> AlocacoesEntreEmpresas
        {
            get
            {
                return Alocacoes
                    .GroupBy(i => i.EmpresaFinanciadoraId)
                    .ToDictionary(i => i.Key, i => i
                        .GroupBy(x => x.EmpresaRecebedoraId)
                        .ToDictionary(x => x.Key, x => x.ToList())
                    );
            }
        }

        public List<AlocacaoRecurso> AlocacaoPorCategoria => AgruparPorCategoria(Alocacoes);
        public List<AlocacaoRecurso> AlocacoesInternasPorCategoria => AgruparPorCategoria(AlocacoesInternas);
        public List<AlocacaoRecurso> AlocacoesExternasPorCategoria => AgruparPorCategoria(AlocacoesExternas);
    }
}